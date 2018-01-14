using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnAudio : MonoBehaviour
{
    public int _band;
    public float _minitensity, _maxitensity;
    Light _light;
    void Start()
    {
        _light = GetComponent<Light>();
    }

    void Update()
    {
        _light.intensity = (AudioVisualizer._audioBandBuffer[_band] * (_maxitensity - _minitensity))+_minitensity;
    }
}