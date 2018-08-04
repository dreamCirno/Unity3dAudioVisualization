using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlaylistManager : MonoBehaviour
{
    ArrayList _playlist;
    public GameObject _songInfoPrefeb;
    public GameObject _content;
    public string[] _songName;
    public string[] _songAuthor;

    public void GetPlaylistInfo()
    {
        if (AudioManager.fileNameList != null && AudioManager.fileNameList.Count != 0)
        {
            _playlist = AudioManager.fileNameList;
            _songName = new string[_playlist.Count];
            _songAuthor = new string[_playlist.Count];
            FileInfo fileInfo;
            for (int i = 0; i < _playlist.Count; i++)
            {
                fileInfo = (FileInfo)_playlist[i];
                _songName[i] = fileInfo.Name.Replace(fileInfo.Extension, string.Empty);
                GameObject songinfoInstance = Instantiate(_songInfoPrefeb);
                songinfoInstance.transform.SetParent(_content.transform);
                songinfoInstance.transform.name = i.ToString();
                //加空格以免MouseOver无法启用
                songinfoInstance.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = _songName[i] + "                                                                  ";
            }
        }
    }
}
