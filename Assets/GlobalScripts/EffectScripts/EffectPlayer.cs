using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

using System;

public class EffectPlayer : MonoBehaviour
{
    private List<EffectLibrary.Effect> _effects;
    // Start is called before the first frame update
    void Start()
    {
        _effects = new List<EffectLibrary.Effect>();
    }

    // Update is called once per frame
    void Update()
    {
        ClearFinishedEffects();
    }

    public int PlayEffect(ref EffectLibrary.Effect effect, Transform parent, Vector3 position, Vector3 rotation, float size, float speed)
    {
        GameObject entity;
        entity = effect.Setup(position, rotation, size, speed);
        if (parent != null)
        {
            entity.transform.parent = parent;
        }
        else
        {
            entity.transform.parent = transform;
        }
        _effects.Add(effect);
        effect.Play();
        return effect.GetID;
    }

    public void StopEffect(int ID)
    {
        if (_effects != null)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                if(_effects[i].GetID == ID)
                {
                    _effects[i].Stop();
                }
            }
        }
    }

    private void ClearFinishedEffects()
    {
        bool isFinished = true;
        if(_effects != null)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                isFinished = true;

                if (_effects[i].GetEntity == null)
                {
                    _effects.RemoveAt(i);
                    continue;
                }

                for (int j = 0; j < _effects[i].GetEntity.GetComponentsInChildren<ParticleSystem>().Length; j++)
                {
                    if (_effects[i].GetEntity.GetComponentsInChildren<ParticleSystem>()[j].IsAlive())
                    {
                        isFinished = false;
                    }
                }

                if (_effects[i].GetEntity.GetComponentsInChildren<VisualEffect>().Length > 0)
                {
                    if (Time.time - _effects[i].GetStartTime < _effects[i].GetTotalTime)
                    {
                        isFinished = false;
                    }
                }

                if (isFinished)
                {
                    Destroy(_effects[i].GetEntity);
                    _effects.RemoveAt(i);
                }
            }
        }
    }
}
