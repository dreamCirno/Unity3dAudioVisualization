using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NAudio;
using NAudio.Wave;
using SFB;
using System.Windows.Forms;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class FileManager : MonoBehaviour
{
    #region 变量
    string _windowsUsername;
    [HideInInspector]
    public string _saveFolder;
    public Image _ImgLoading;
    public static string name = "";
    public static bool _isLoading = false;
    WWW www;
    #endregion
    #region 方法
    private void Awake()
    {
        //获取当前Windows用户系统用户名
        _windowsUsername = System.Environment.UserName;
        //指定缓存路径
        _saveFolder = @"C:\Users\" + _windowsUsername + @"\Music\Audio Visualization Audio\";
    }

    private void Update()
    {
        _ImgLoading.transform.Rotate(Vector3.back);
    }
    #endregion
    #region 自定义方法
    #region 打开文件夹
    /// <summary>
    /// 动态打开文件夹
    /// </summary>
    /// <returns></returns>
    public ArrayList OpenFileBrowser()
    {
        var extensions = new[] { new ExtensionFilter("Audio Files", "mp3", "wav", "ogg"), /*new ExtensionFilter("All Files", "*"), */};
        string[] filePaths = StandaloneFileBrowser.OpenFolderPanel("Open Audio Folder", "", false);
        //将可用格式音频记录到ArrayList
        ArrayList arrayList = new ArrayList();
        if (filePaths != null && filePaths.Length != 0)
        {
            DirectoryInfo folder = new DirectoryInfo(filePaths[0]);
            arrayList = new ArrayList();
            foreach (FileInfo file in folder.GetFiles("*.mp3"))
            {
                arrayList.Add(file);
            }
            foreach (FileInfo file in folder.GetFiles("*.wav"))
            {
                arrayList.Add(file);
            }
            foreach (FileInfo file in folder.GetFiles("*.ogg"))
            {
                arrayList.Add(file);
            }
        }
        return arrayList;
    }
    /// <summary>
    /// 打开已指定路径文件夹
    /// </summary>
    /// <param name="strPath">指定路径文件夹</param>
    /// <returns></returns>
    public ArrayList OpenFileBrowser(string strPath)
    {
        var extensions = new[] { new ExtensionFilter("Audio Files", "mp3", "wav", "ogg"), /*new ExtensionFilter("All Files", "*"), */};
        string[] filePaths = StandaloneFileBrowser.OpenFolderPanel("Open Audio Folder", strPath, false);
        DirectoryInfo folder = new DirectoryInfo(filePaths[0]);
        //将可用格式音频记录到ArrayList
        ArrayList arrayList = new ArrayList();
        if (filePaths != null && filePaths.Length != 0)
        {
            //附加支持格式：mp3
            foreach (FileInfo file in folder.GetFiles("*.mp3"))
            {
                arrayList.Add(file);
            }
            //附加支持格式：wav
            foreach (FileInfo file in folder.GetFiles("*.wav"))
            {
                arrayList.Add(file);
            }
            //附加支持格式：ogg
            foreach (FileInfo file in folder.GetFiles("*.ogg"))
            {
                arrayList.Add(file);
            }
        }
        return arrayList;
    }
    #endregion
    #region 加载音频
    public IEnumerator LoadAudio(FileInfo fileInfo)
    {
        //文件绝对路径
        string filepath = fileInfo.FullName;
        //文件拓展名
        string extension = fileInfo.Extension;
        //文件转换格式后的保存路径
        string savepath = "";
        _ImgLoading.gameObject.SetActive(true);
        name = fileInfo.Name.Replace(fileInfo.Extension, string.Empty);
        //指定Mp3ToWAV的保存路径
        if (fileInfo.Extension == ".mp3")
        {
            savepath = _saveFolder + name + ".wav";
        }

        //MP3
        if (extension == ".mp3")
        {
            FileStream stream = File.Open(filepath, FileMode.Open);
            Mp3FileReader reader = new Mp3FileReader(stream);
            //如果不存在缓存文件夹，则创建
            if (!Directory.Exists(_saveFolder))
            {
                Directory.CreateDirectory(_saveFolder);
            }
            //如果不存在缓存音频，则写入到指定目录
            //否则不再重复写入
            if (!Directory.Exists(savepath))
            {
                FileInfo file = new FileInfo(savepath);
                AudioManager.fileNameList[AudioManager.currentNum] = file;
                WaveFileWriter.CreateWaveFile(savepath, reader);
            }
            www = new WWW("file://" + savepath);
        }
        //WAV | OGG
        if (extension == ".wav" || extension == ".ogg")
        {
            www = new WWW("file://" + filepath);
        }
        yield return www;
        //将指定路径的AudioClip赋予给AudioSource
        AudioClip clip = WWWAudioExtensions.GetAudioClip(www);
        AudioManager._audioSource.clip = clip;
        //加载完成，Loading图片不再显示
        _ImgLoading.gameObject.SetActive(false);

        AudioManager._audioIsLoadDone = true;
    }
    #endregion
    #endregion
}
