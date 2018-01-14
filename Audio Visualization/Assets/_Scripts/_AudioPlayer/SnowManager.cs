using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowManager : MonoBehaviour
{
    public float _startScale, _maxScale;
    public bool _useBuffer;
    ParticleSystem _snowParticle;

    void Start()
    {
        _snowParticle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!_useBuffer)
        {
            transform.localScale = new Vector3((AudioVisualizer._amplitude * _maxScale) + _startScale, (AudioVisualizer._amplitude) * _maxScale + _startScale, (AudioVisualizer._amplitude) * _maxScale + _startScale);
            Color _color = new Color(AudioVisualizer._amplitude, AudioVisualizer._amplitude, AudioVisualizer._amplitude);
        }
        else
        {
            ParticleSystem.MainModule _mainModule = _snowParticle.main;
            _mainModule.startSpeed = (AudioVisualizer._amplitudeBuffer * _maxScale) + _startScale;
            ParticleSystem.EmissionModule _emission = _snowParticle.emission;
            _emission.rateOverTime = (AudioVisualizer._amplitudeBuffer * _maxScale * 3);
        }
    }
}
