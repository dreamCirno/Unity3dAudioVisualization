using NAudio.Wave;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioSource _audioSource;
    public Image _progressBar;
    ArrayList _audioPlayList = new ArrayList();

    public static int currentNum = 0;
    public static ArrayList fileNameList = new ArrayList();
    public static Text _songInfo;
    public FileManager fileManager;

    Coroutine _playAudio;
    public static bool _audioIsLoadDone = true;

    public enum PlayButton
    {
        CURRENT,
        NEXT,
        PREV,
        PAUSE,
        STOP,
        RESUME
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _songInfo = GameObject.Find("SongInfo").GetComponent<Text>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (_audioIsLoadDone)
        {
            //回车键 - 打开文件夹后，开始播放音乐列表
            if (Input.GetKeyUp(KeyCode.Return))
            {
                fileNameList = fileManager.OpenFileBrowser();
                PlaySong(PlayButton.CURRENT);
            }
            //左箭头键 - 上一首
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                PlaySong(PlayButton.PREV);
            }
            //右箭头键 - 下一首
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                PlaySong(PlayButton.NEXT);
            }
            //空格键 - 播放/暂停/继续
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (_audioSource.clip != null)
                {
                    if (_audioSource.isPlaying)
                    {
                        _audioSource.Pause();
                    }
                    else
                    {
                        if (_audioSource.time != 0)
                        {
                            _audioSource.UnPause();
                        }
                        else
                        {
                            _audioSource.Play();
                        }
                    }
                }
            }
        }

        if (_audioSource.clip != null)
        {
            //音频进度条百分比
            _progressBar.fillAmount = _audioSource.time / _audioSource.clip.length;
            //播放结束后，自动下一首
            if (_audioSource.time == _audioSource.clip.length)
            {
                PlaySong(PlayButton.NEXT);
            }
        }
    }
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="playButton">播放状态</param>
    public void PlaySong(PlayButton playButton)
    {
        //如果列表不为null或者空
        if (fileNameList != null && fileNameList.Count != 0)
        {
            _playAudio = StartCoroutine(DelayPlayAudio(playButton));
        }
        else
        {
            MessageBox.Show("没有播放列表", "Audio Visualization");
        }
    }

    IEnumerator DelayPlayAudio(PlayButton playButton)
    {
        //确保加载和转换音频过程中，没有二次操作
        _audioIsLoadDone = false;
        switch (playButton)
        {
            //播放当前
            case PlayButton.CURRENT:

                break;
            //播放下一首
            case PlayButton.NEXT:
                if (fileNameList != null)
                {
                    if (currentNum < fileNameList.Count - 1)
                    {
                        currentNum++;
                    }
                    else
                    {
                        currentNum = 0;
                    }
                }
                break;
            //播放上一首
            case PlayButton.PREV:
                if (fileNameList != null)
                {
                    if (currentNum == 0)
                    {
                        currentNum = fileNameList.Count - 1;
                    }
                    else
                    {
                        currentNum--;
                    }
                }
                break;
            //暂停播放
            case PlayButton.PAUSE:
                _audioSource.Pause();
                break;
            //停止播放
            case PlayButton.STOP:
                _audioSource.Stop();
                break;
            //继续播放
            case PlayButton.RESUME:
                _audioSource.UnPause();
                break;
        }

        //如果播放状态：上一首 下一首 当前，则开始加载音频。
        //否则只对AudioSource进行直接控制。
        if (playButton != PlayButton.RESUME && playButton != PlayButton.PAUSE && playButton != PlayButton.STOP)
        {
            FileInfo fileInfo = (FileInfo)fileNameList[currentNum];
            //等待音乐加载完毕
            yield return StartCoroutine(fileManager.LoadAudio(fileInfo));
            //加载完毕，播放
            _audioSource.Play();
            //显示歌曲信息
            _songInfo.text = FileManager.name;
            //模拟用户鼠标移动，触发工具栏淡入显示
            GameManager._newMousePosition = new Vector2(9999, 9999);
        }
    }
}
