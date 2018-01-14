using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public float _startScale, _maxScale, _limitScale;
    public bool _useBuffer;
    Material _material;
    float _newScale;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
    }

    void Update()
    {
        if (!System.Single.IsNaN(AudioVisualizer._amplitudeBuffer) && !System.Single.IsNaN(AudioVisualizer._amplitude))
        {
            if (!_useBuffer)
            {
                if ((AudioVisualizer._amplitude * _maxScale) + _startScale > _limitScale || (AudioVisualizer._amplitude * _maxScale) + _startScale < _startScale)
                {
                    _newScale = _limitScale;
                }
                else
                {
                    _newScale = (AudioVisualizer._amplitude * _maxScale) + _startScale;
                }
                transform.localScale = new Vector3(_newScale, _newScale, _newScale);
                Color _color = new Color(_material.GetColor("_EmissionColor").r, AudioVisualizer._amplitude, _material.GetColor("_EmissionColor").b, 1f);
                _material.SetColor("_EmissionColor", _color);
            }
            else
            {
                if ((AudioVisualizer._amplitudeBuffer * _maxScale) + _startScale > _limitScale || (AudioVisualizer._amplitudeBuffer * _maxScale) + _startScale < _startScale)
                {
                    _newScale = _limitScale;
                }
                else
                {
                    _newScale = (AudioVisualizer._amplitudeBuffer * _maxScale) + _startScale;
                }
                transform.localScale = new Vector3(_newScale, _newScale, _newScale);
                Color _color = new Color(_material.GetColor("_EmissionColor").r, AudioVisualizer._amplitudeBuffer, _material.GetColor("_EmissionColor").b, 1f);
            }
        }
    }
}
