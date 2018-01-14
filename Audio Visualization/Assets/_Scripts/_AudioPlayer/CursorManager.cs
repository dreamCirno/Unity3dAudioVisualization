using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(ParticleSystem))]
public class CursorManager : MonoBehaviour
{
    public int x;
    public int y;
    public Image _imageCircle;
    public int _rotateSpeed;
    TrailRenderer _trailCursor;
    ParticleSystem _particleSystem;

    void Awake()
    {
        _trailCursor = GetComponent<TrailRenderer>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        //如果鼠标在指定时间内持续静止
        if ((GameManager._oldMousePosition == GameManager._newMousePosition) && GameManager._isStatic)
        {
            HideCursor();
        }
        else
        {
            ShowCursor();
            RotateCursorBackground();
            TrailWithCursor();
        }
    }
    /// <summary>
    /// 隐藏鼠标
    /// </summary>
    public void HideCursor()
    {
        _imageCircle.enabled = false;
        _trailCursor.enabled = false;
        _particleSystem.Stop();
        Cursor.visible = false;
    }
    /// <summary>
    /// 显示图标
    /// </summary>
    public void ShowCursor()
    {
        _imageCircle.enabled = true;
        _trailCursor.enabled = true;
        if (_particleSystem.isStopped)
        {
            _particleSystem.Play();
        }
        Cursor.visible = true;
    }
    /// <summary>
    /// 旋转鼠标后的圆形
    /// </summary>
    void RotateCursorBackground()
    {
        _imageCircle.transform.position = Input.mousePosition + new Vector3(_imageCircle.GetComponent<RectTransform>().sizeDelta.x / 16, -_imageCircle.GetComponent<RectTransform>().sizeDelta.y / 20, 0);
        _imageCircle.transform.Rotate(Vector3.back * _rotateSpeed);
    }

    void TrailWithCursor()
    {
        _trailCursor.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }
}
