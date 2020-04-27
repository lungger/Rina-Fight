using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    public GameObject _camera;
    GameObject _cube;
    Vector3 _distance; //預設 相機與準星的 距離
    float _parentScaleX; //父物件的scale x
    float _parentScaleY; //父物件的scale y

    void ResizeScale()
    {
        Vector3 newDistance = _camera.transform.position - transform.position; //新的 相機與準星的距離
        float scale = newDistance.magnitude / _distance.magnitude; // 計算scale
        scale = Mathf.Clamp(scale, 0.5f, 1.0f); //scale 限制在區間 [0.5f, 1.0f] 
        gameObject.transform.localScale = new Vector3(scale / _parentScaleX, scale / _parentScaleY, 1.0f);
        //Debug.Log(scale);
    }

    // Start is called before the first frame update
    void Start()
    {
        _cube = gameObject.transform.parent.parent.gameObject;
        _distance = _camera.transform.position - transform.position;
        _parentScaleX = _cube.transform.localScale.x;
        _parentScaleY = _cube.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //設回原點 > LookAt 攝影機 > 往前一步
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        gameObject.transform.LookAt(_camera.transform.position);
        gameObject.transform.localPosition += gameObject.transform.forward * 1;
        ResizeScale();
    }
}
