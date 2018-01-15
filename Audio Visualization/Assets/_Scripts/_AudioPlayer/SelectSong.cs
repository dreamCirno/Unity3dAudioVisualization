using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectSong : MonoBehaviour
{
    AudioManager _audioManager;
    //Button _button;
    //Color _defineColor = new Color(1, 1, 1, 0);
    //Color _selectColor = new Color(0, 1, 1, 128f / 255f);
    //ColorBlock colorBlock;

    private void OnEnable()
    {
        _audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        //_button = GetComponent<Button>();
        //colorBlock = _button.colors;
    }

    public void PlaySelectedAudio()
    {
        AudioManager.currentNum = Convert.ToInt32(name);
        _audioManager.PlaySong(AudioManager.PlayButton.CURRENT);
    }

    //private void Update()
    //{
    //    if (AudioManager.currentNum.ToString() == name)
    //    {
    //        colorBlock.normalColor = _selectColor;
    //    }
    //    else
    //    {
    //        colorBlock.normalColor = _defineColor;
    //    }
    //}
}
