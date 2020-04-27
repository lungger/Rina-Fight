using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinaNormalAttackControl_3 : MonoBehaviour
{
    private Material _attackMaterial;
    private float _offset;
    private float _startTime;
    private float _deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem[] tempParticleSystem = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < tempParticleSystem.Length; i++)
        {
            if (string.Compare(tempParticleSystem[i].name, "RinaNormalAttact_3") == 0)
            {
                _attackMaterial = tempParticleSystem[i].GetComponent<Renderer>().material;
            }
        }

        _offset = 0f;
        _startTime = Time.time;
        _deltaTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _startTime > 0.20f && Time.time - _deltaTime > 0.003f)
        {
            if(_offset - 0.015f <= -1f)
            {
                _offset = -1f;
            }
            else
            {
                _offset -= 0.03f;
            }
            _attackMaterial.SetFloat("Vector1_21770E65", _offset);
            _deltaTime = Time.time;
        }
    }

}
