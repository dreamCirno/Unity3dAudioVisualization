using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public AudioManager _audioManager;
    /// <summary>
    /// 播放当前/继续
    /// </summary>
    public void AudioPlayAudio()
    {
        if (_audioManager.GetComponent<AudioSource>().time == 0)
        {
            _audioManager.PlaySong(AudioManager.PlayButton.CURRENT);
        }
        else
        {
            _audioManager.PlaySong(AudioManager.PlayButton.RESUME);
        }
    }
    /// <summary>
    /// 播放下一首
    /// </summary>
    public void AudioPlayNext()
    {
        _audioManager.PlaySong(AudioManager.PlayButton.NEXT);
    }
    /// <summary>
    /// 播放上一首
    /// </summary>
    public void AudioPlayPrev()
    {
        _audioManager.PlaySong(AudioManager.PlayButton.PREV);
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void AudioPause()
    {
        _audioManager.PlaySong(AudioManager.PlayButton.PAUSE);
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void AudioStop()
    {
        _audioManager.PlaySong(AudioManager.PlayButton.STOP);
    }
}
