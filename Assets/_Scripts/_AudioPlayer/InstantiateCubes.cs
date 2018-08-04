using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCubes : MonoBehaviour
{
    public AudioVisualizer audioVisualizer;
    public GameObject _sampleCubePrefeb;
    GameObject[] _sampleCubes;
    public int _farAwayFromCenter;
    public int maxScale;
    public float initScale;

    private void Awake()
    {
        _sampleCubes = new GameObject[audioVisualizer._samplesCount];
    }

    private void Start()
    {
        InstantiateCube();
    }

    private void Update()
    {
        for (int i = 0; i < audioVisualizer._samplesCount; i++)
        {
            float newLength;
            if ((AudioVisualizer._samplesBandBuffer[i] * maxScale + initScale) > 0)
            {
                if (Mathf.Abs((AudioVisualizer._samplesBandBuffer[i] * maxScale + initScale) - _sampleCubes[i].transform.localScale.y) < 3f)
                {
                    newLength = Mathf.Abs(Mathf.Lerp(_sampleCubes[i].transform.localScale.y, ((AudioVisualizer._samplesBandBuffer[i] * maxScale) + initScale), 0.1f));
                }
                else
                {
                    newLength = Mathf.Abs((AudioVisualizer._samplesBandBuffer[i] * maxScale + initScale));
                }
                _sampleCubes[i].transform.localScale = new Vector3(transform.localScale.x, newLength, transform.localScale.z);
            }
        }
    }

    void InstantiateCube()
    {
        for (int i = 0; i < audioVisualizer._samplesCount; i++)
        {
            GameObject _instanceSampleCube = Instantiate(_sampleCubePrefeb);
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "SampleCube " + i;
            this.transform.eulerAngles = new Vector3(0, -180f / audioVisualizer._samplesCount * i, 0);
            _instanceSampleCube.transform.position = Vector3.forward * _farAwayFromCenter;
            _sampleCubes[i] = _instanceSampleCube;
        }
        if (transform.name == "InstantiateCubes")
        {
            transform.eulerAngles = new Vector3(transform.localRotation.x, 0f, transform.localRotation.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.localRotation.x, -180f, transform.localRotation.z);
        }
    }
}
