using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectSong : MonoBehaviour
{
    AudioManager _audioManager;

    private void OnEnable()
    {
        _audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
    }

    public void PlaySelectedAudio()
    {
        AudioManager.currentNum = Convert.ToInt32(name);
        _audioManager.PlaySong(AudioManager.PlayButton.CURRENT);
    }
}
