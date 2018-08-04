using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int _band;
    public float _startScale, _maxScale;
    Material _material;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
    }

    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, (AudioVisualizer._bandBuffer[_band] * _maxScale) + _startScale, transform.localScale.z);
        Color _color = new Color(_material.GetColor("_EmissionColor").r, AudioVisualizer._audioBandBuffer[_band], _material.GetColor("_EmissionColor").b);
        _material.SetColor("_EmissionColor", _color);
    }
}
