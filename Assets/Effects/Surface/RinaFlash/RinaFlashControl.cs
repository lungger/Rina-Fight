using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinaFlashControl : MonoBehaviour
{
    private Material _flashMaterial;
    private float _threshold;
    private float _startTime;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem[] tempParticleSystem = GetComponentsInChildren<ParticleSystem>();
        for(int i = 0; i < tempParticleSystem.Length; i++)
        {
            if(string.Compare(tempParticleSystem[i].name, "Flash") == 0)
            {
                _flashMaterial = tempParticleSystem[i].GetComponent<Renderer>().material;
            }
        }
        
        _threshold = 0f;
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _startTime > 0.01f)
        {
            _threshold += 0.015f;
            _flashMaterial.SetFloat("Vector1_4D7D33DC", _threshold);
            _startTime = Time.time;
        }
    }

    public void flashstart()
    {
        _threshold = 0f;
        _startTime = Time.time;
    }
}
