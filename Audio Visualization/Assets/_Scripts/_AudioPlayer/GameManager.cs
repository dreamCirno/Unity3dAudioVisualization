using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    #region 变量
    AudioSource _gameControllerAudio;
    public AudioSource _audioSource;
    public AudioClip _welcomeClip;
    public AudioClip _seeyaClip;
    public AudioClip _enterClip;

    public Image _fadePanel;
    public GameObject _musicPanel;
    Color _color = new Color(0, 0, 0, 255);
    bool _isOver = false;

    float _staticTimer = 0;
    public float _waitStaticTime;
    public static bool _isStatic = false;
    public static Vector2 _oldMousePosition = Vector2.zero;
    public static Vector2 _newMousePosition = Vector2.one;
    Color[] _definePanelColor;
    Color[] _fadePanelColor;
    Image[] _panelImages;
    Text[] _textAudioInfo;
    public Coroutine _delayFadeIn;
    public Coroutine _delayFadeOut;

    public FileManager _fileManager;

    bool _isover = false;
    #endregion
    #region 方法
    private void Start()
    {
        _gameControllerAudio = GetComponent<AudioSource>();
        StartCoroutine(DelayWelcome());
        DefinePanelColor(_musicPanel);
    }

    private void Update()
    {
        //程序退出
        MonitorEscape();
        //监视用户在限定时间内有无动作
        MonitorAction();
    }

    private void OnGUI()
    {
        GUI.TextArea(new Rect(Screen.width - 300, Screen.height - 150, 300, 150), "回车键：载入歌曲文件夹（音频仅支持mp3,ogg,wav）\r\n空格键：播放/继续\r\n左方向键：上一首\r\n右方向键：下一首\r\nEsc键：退出（请正常通过Esc退出，因在本机音乐文件夹存有缓存，正常关闭后自动删除缓存文件夹。）\r\n上方工具栏的最后一个歌曲列表选项不可用。");
    }

    private void OnApplicationQuit()
    {
        StartCoroutine(DelayQuit());
    }
    #endregion
    #region 自定义方法
    #region 监听退出
    void MonitorEscape()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator DelayWelcome()
    {
        //出现Welcome声音
        _audioSource.clip = _enterClip;
        _audioSource.Play();
        AudioManager._songInfo.text = _audioSource.clip.name;
        _gameControllerAudio.PlayOneShot(_welcomeClip);
        //渐变
        for (int i = 255; i > 0; i -= 5)
        {
            _fadePanel.color = new Color(0, 0, 0, i / 255f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator DelayQuit()
    {
        if (!_isover)
        {
            Application.CancelQuit();
            _isover = true;
        }
        //停止音源播放
        _audioSource.Stop();
        //如果存在缓存文件夹，删除
        while (Directory.Exists(_fileManager._saveFolder))
        {
            Directory.Delete(_fileManager._saveFolder, true);
        }
        //出现Seeu声音
        _gameControllerAudio.PlayOneShot(_seeyaClip);
        //渐变
        for (int i = 0; i <= 255; i += 3)
        {
            _fadePanel.color = new Color(0, 0, 0, i / 255f);
            yield return new WaitForSeconds(0.01f);
        }
        Application.Quit();
    }
    #endregion
    #region 面板淡入淡出
    void MonitorAction()
    {
        _staticTimer += Time.deltaTime;
        _oldMousePosition = _newMousePosition;
        _newMousePosition = Input.mousePosition;
        //如果用户无动作
        if (_oldMousePosition == _newMousePosition)
        {
            if (_staticTimer > _waitStaticTime && !_isStatic)
            {
                StopAllCoroutines();
                _delayFadeOut = StartCoroutine(HideOut(_musicPanel));
                _isStatic = true;
            }
        }
        else
        {
            _delayFadeIn = StartCoroutine(HideIn(_musicPanel));
            _staticTimer = 0;
            _isStatic = false;
        }
    }

    IEnumerator HideOut(GameObject panel)
    {
        if (_delayFadeIn != null)
        {
            StopCoroutine(_delayFadeIn);
        }
        while (_panelImages[_panelImages.Length - 1].color != _fadePanelColor[_panelImages.Length - 1])
        {
            _textAudioInfo[0].color = Color.Lerp(_textAudioInfo[0].color, new Color(1, 1, 1, 0), 0.1f);
            foreach (Image img in _panelImages)
            {
                img.color = Color.Lerp(img.color, new Color(img.color.r, img.color.g, img.color.b, 0), 0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }

        //images[0].color = Color.Lerp(images[0].color, _fadePanelColor[0], 0.1f);
        //images[1].color = Color.Lerp(images[1].color, _fadePanelColor[1], 0.1f);
        //images[2].color = Color.Lerp(images[2].color, _fadePanelColor[2], 0.1f);
        //images[3].color = Color.Lerp(images[3].color, _fadePanelColor[3], 0.1f);
        //images[4].color = Color.Lerp(images[4].color, _fadePanelColor[4], 0.1f);
        //images[5].color = Color.Lerp(images[5].color, _fadePanelColor[5], 0.1f);
        //images[6].color = Color.Lerp(images[6].color, _fadePanelColor[6], 0.1f);
        //images[7].color = Color.Lerp(images[7].color, _fadePanelColor[7], 0.1f);
        //images[8].color = Color.Lerp(images[8].color, _fadePanelColor[8], 0.1f);
        //for (int i = 1; i <= 255; i++)
        //{
        //    panel.transform.Translate(new Vector3(0, 5, 0));
        //    yield return null;
        //}
    }

    IEnumerator HideIn(GameObject panel)
    {
        if (_delayFadeOut != null)
        {
            StopCoroutine(_delayFadeOut);
        }
        while (_panelImages[_panelImages.Length - 1].color != _definePanelColor[_panelImages.Length - 1])
        {
            _textAudioInfo[0].color = Color.Lerp(_textAudioInfo[0].color, new Color(1, 1, 1, 1), 0.1f);
            for (int i = 0; i < _panelImages.Length; i++)
            {
                _panelImages[i].color = Color.Lerp(_panelImages[i].color, _definePanelColor[i], 0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        //for (int i = 1; i <= 255; i++)
        //{
        //    panel.transform.Translate(new Vector3(0, -5, 0));
        //    yield return null;
        //}
    }

    void DefinePanelColor(GameObject panel)
    {
        _panelImages = panel.GetComponentsInChildren<Image>();
        _textAudioInfo = panel.GetComponentsInChildren<Text>();
        _definePanelColor = new Color[_panelImages.Length];
        _fadePanelColor = new Color[_panelImages.Length];
        for (int i = 0; i < _panelImages.Length; i++)
        {
            _definePanelColor[i] = new Color(_panelImages[i].color.r, _panelImages[i].color.g, _panelImages[i].color.b, _panelImages[i].color.a);
            //Debug.Log(_panelImages[i].color);
        }
        for (int i = 0; i < _definePanelColor.Length; i++)
        {
            _fadePanelColor[i] = new Color(_definePanelColor[i].r, _definePanelColor[i].g, _definePanelColor[i].b, 0);
        }
    }
    #endregion
    #endregion
}
