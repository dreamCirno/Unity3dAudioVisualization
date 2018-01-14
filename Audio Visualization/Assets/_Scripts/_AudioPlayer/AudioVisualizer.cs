using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizer : MonoBehaviour
{
    AudioSource _audioSource;
    public FFTWindow _fftWindow;
    //麦克风输入
    public AudioClip _audioClip;
    public bool _useMicrophone;
    public string _selectDevice;
    //频谱数据
    public static float[] _samples = new float[512];
    public int _samplesCount;
    //Add
    public static float[] _samplesBandBuffer = new float[512];
    float[] _samplesBufferDecrease = new float[512];

    //频段
    public static float[] _freqBand = new float[8];
    public int _freqBandCount;
    //缓冲器
    public static float[] _bandBuffer = new float[8];
    //缓冲减少量
    float[] _bufferDecrease = new float[8];

    //频段最高值
    float[] _freqBandHeighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];

    public static float _amplitude, _amplitudeBuffer;
    float _amplitudeHighest = 0;
    //音频配置文件
    public float _audioProfile;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioProfile(_audioProfile);
        //麦克风输入
        if (_useMicrophone)
        {
            if (Microphone.devices.Length > 0)
            {
                _selectDevice = Microphone.devices[0].ToString();
                _audioSource.clip = Microphone.Start(_selectDevice, true, 10, AudioSettings.outputSampleRate);
            }
            else
            {
                _useMicrophone = false;
            }
        }
        if (!_useMicrophone)
        {
            _audioSource.clip = _audioClip;
            _audioSource.Play();
        }
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
        BandBuffer1();
    }
    /// <summary>
    /// 防止数值跳动频率过快
    /// </summary>
    /// <param name="audioProfile"></param>
    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            _freqBandHeighest[i] = _audioProfile;
        }
    }
    void BandBuffer1()
    {
        for (int g = 0; g < 512; ++g)
        {
            if (_samples[g] > _samplesBandBuffer[g])
            {
                _samplesBandBuffer[g] = _samples[g];
                _samplesBufferDecrease[g] = 0.005f;
            }
            if (_samples[g] < _samplesBandBuffer[g])
            {
                _samplesBandBuffer[g] -= _samplesBufferDecrease[g];
                _samplesBufferDecrease[g] *= 1.2f;
            }
        }
    }
    /// <summary>
    /// 获得振幅
    /// </summary>
    void GetAmplitude()
    {
        float _currentAmplitude = 0;
        float _currentAmplitudeBuffer = 0;
        for (int i = 0; i < _freqBandCount; i++)
        {
            _currentAmplitude += _audioBand[i];
            _currentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if (_currentAmplitude > _amplitudeHighest)
        {
            _amplitudeHighest = _currentAmplitude;
        }
        _amplitude = _currentAmplitude / _amplitudeHighest;
        _amplitudeBuffer = _currentAmplitudeBuffer / _amplitudeHighest;
    }
    /// <summary>
    /// 创建音频频段
    /// </summary>
    void CreateAudioBands()
    {
        for (int i = 0; i < _freqBandCount; i++)
        {
            if (_freqBand[i] > _freqBandHeighest[i])
            {
                _freqBandHeighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHeighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHeighest[i]);
        }
    }
    /// <summary>
    /// 获取频谱音频源
    /// </summary>
    void GetSpectrumAudioSource()
    {
        //获取频谱数据
        _audioSource.GetSpectrumData(_samples, 0, _fftWindow);
    }
    /// <summary>
    /// 制作频段
    /// </summary>
    void MakeFrequencyBands()
    {
        /* 假设采样率22050，已获取512个样本
         * 22050 / 512 = 43赫兹每样本
         * 20 - 60 赫兹
         * 60 - 250 赫兹
         * 500 - 2000 赫兹
         * 2000 - 4000 赫兹
         * 4000 - 6000 赫兹
         * 6000 - 20000 赫兹
         */
        int count = 0;
        for (int i = 0; i < _freqBandCount; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }
            average /= count;
            _freqBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        for (int g = 0; g < _freqBandCount; ++g)
        {
            if (_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                //_bufferDecrease[g] = 0.005f;
                _bufferDecrease[g] = 0.005f;
            }
            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                //_bufferDecrease[g] *= 1.2f;
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }
}
