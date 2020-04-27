using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectLibrary : MonoBehaviour
{
    public GameObject BoomPrefab;
    public GameObject Boom2Prefab;
    public GameObject JumpPrefab;
    public GameObject DoubleJumpPrefab;
    public GameObject TornadoPrefab;
    public GameObject WeaponTrailPrefab;
    public GameObject HitPrefab;
    public GameObject Hit2Prefab;
    public GameObject GunshotPrefab;
    public GameObject RinaFlashPrefab;
    public GameObject RinaNormalAttackPrefab;
    public GameObject RinaNormalAttackHitPrefab;
    public GameObject FireMouseDieExplosionPrefab;
    public GameObject FireMouseDieExplosionChargePrefab;
    public GameObject MobDeathSmokePrefab;

    public class MobDeathSmoke : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().MobDeathSmokePrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();

            _entity.transform.position = position;

            for (int i = 0; i < _particleSystems.Length; i++)
            {
                _particleSystems[i].transform.localScale = new Vector3(size, size, size);
            }

            return _entity;
        }
    }

    public class FireMouseDieExplosionCharge : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().FireMouseDieExplosionChargePrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();

            _entity.transform.position = position;

            return _entity;
        }
    }

    public class FireMouseDieExplosion : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().FireMouseDieExplosionPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();

            _entity.transform.position = position;

            return _entity;
        }
    }

    public class RinaNormalAttackHit : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().RinaNormalAttackHitPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();

            _entity.transform.position = position;

            return _entity;
        }
    }

    public class RinaNormalAttack : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().RinaNormalAttackPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();

            _entity.transform.eulerAngles = new Vector3(rotation.x, rotation.y, rotation.z);
            Vector3 temp = new Vector3(-0.2f, 0.9f, 0.1f);
            temp = Quaternion.Euler(0f, rotation.y, 0f) * temp;
            _entity.transform.position = position + temp;

            return _entity;
        }
    }

    public class RinaFlash : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\Jump\\Jump", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().RinaFlashPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();

            for (int i = 0; i < _particleSystems.Length; i++)
            {
                if (string.Compare(_particleSystems[i].transform.name, "Flash") == 0)
                {
                    ParticleSystem.MainModule mainModule = _particleSystems[0].main;
                    mainModule.startRotationY = (rotation.y * Mathf.PI) / 180f;
                }
            }


            _entity.transform.eulerAngles = new Vector3(rotation.x, rotation.y, rotation.z);
            _entity.transform.position = position;

            return _entity;
        }
    }

    public class Gunshot : Effect
    {
        private Vector3 _targetPosition;

        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\Boom_2\\Boom_2", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().GunshotPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();
            LineRenderer ray = _entity.GetComponentInChildren<LineRenderer>();

            Vector3 tempVector3 = _targetPosition - position;
            if (tempVector3.magnitude > 20f)
            {
                float tempMagnitude = tempVector3.magnitude;
                tempVector3.x *= 20f / tempMagnitude;
                tempVector3.y *= 20f / tempMagnitude;
                tempVector3.z *= 20f / tempMagnitude;
            }

            Vector3 tempExplodePoint = _targetPosition - position;
            RaycastHit hit;
            if (Physics.Raycast(position, _targetPosition - position, out hit))
            {
                if ((hit.point - position).magnitude <= 20f)
                {
                    tempExplodePoint = hit.point - position;
                    tempExplodePoint -= (_targetPosition - position) / ((_targetPosition - position).magnitude * 2);
                    tempVector3 = tempExplodePoint;
                    for (int i = 0; i < _particleSystems.Length; i++)
                    {
                        if (string.Compare(_particleSystems[i].transform.name, "Gunshot_Fire") != 0)
                        {
                            ParticleSystem.EmissionModule emissionModule = _particleSystems[i].emission;
                            emissionModule.enabled = true;
                        }
                    }
                }
            }

            ray.SetPosition(1, tempVector3);

            Debug.DrawRay(position, _targetPosition - position, Color.green, 1f);
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                if (string.Compare(_particleSystems[i].transform.name, "Gunshot_Fire") != 0)
                {
                    _particleSystems[i].transform.localPosition = tempExplodePoint;
                }
            }

            TotalTime = 2.5f;
            StartTime = Time.time;

            _entity.transform.position = position;


            return _entity;
        }

        public void SetTargetPosition(Vector3 position)
        {
            _targetPosition = position;
        }
    }

    public class Hit2 : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\Boom_2\\Boom_2", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().Hit2Prefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();
            TotalTime = 2.5f;
            StartTime = Time.time;

            _entity.transform.position = position;

            return _entity;
        }
    }

    public class Hit : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\Boom_2\\Boom_2", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().HitPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();
            TotalTime = 2.5f;
            StartTime = Time.time;

            _entity.transform.position = position;

            return _entity;
        }
    }

    public class WeaponTrail : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\Boom_2\\Boom_2", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().WeaponTrailPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();
            TotalTime = 1f;
            StartTime = Time.time;

            _entity.transform.position = position;
            _entity.transform.eulerAngles = rotation;

            return _entity;
        }

        public override void Play()
        {
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                _particleSystems[i].Play();
                ParticleSystem.TrailModule trailModule = _particleSystems[i].trails;
                trailModule.enabled = true;
            }

            for (int i = 0; i < _visualEffects.Length; i++)
            {
                _visualEffects[i].Play();
            }
        }
    }

    public class Boom2 : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\Boom_2\\Boom_2", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().Boom2Prefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();
            TotalTime = 2.5f;
            StartTime = Time.time;

            _entity.transform.position = position;
            _entity.transform.eulerAngles = rotation;

            for (int i = 0; i < _particleSystems.Length; i++)
            {
                if (string.Compare(_particleSystems[i].name, "Boom_2_FireBubble") == 0)
                {
                    ParticleSystem.MainModule mainModule = _particleSystems[i].main;
                    mainModule.startRotationZ = ClampAngle(360f - rotation.y - 180f, -360f, 360f) / 180f * Mathf.PI;
                }

                if (string.Compare(_particleSystems[i].name, "Boom_2_ExplodeBubble") == 0)
                {
                    ParticleSystem.MainModule mainModule = _particleSystems[i].main;
                    mainModule.startRotationZ = ClampAngle(360f - rotation.y, -360f, 360f) / 180f * Mathf.PI;
                }
            }

            _visualEffects[0].SetVector3("FireBurstRotation", new Vector3(-Camera.main.transform.rotation.y, rotation.y - 90f, 0f));
            _visualEffects[0].SetVector3("FireBurstPosition", Quaternion.AngleAxis(rotation.y, Vector3.up) * new Vector3(0f, 0f, 12f));



            return _entity;
        }
    }

    public class DoubleJump : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\DoubleJump\\DoubleJump", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().DoubleJumpPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();
            TotalTime = 2.5f;
            StartTime = Time.time;


            _entity.transform.position = position;

            return _entity;
        }
    }

    public class Jump : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\Jump\\Jump", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().JumpPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();

            _entity.transform.position = position;

            return _entity;
        }
    }

    public class Tornado : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            Material[] materialTornado;

            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\Tornado\\Tornado", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().TornadoPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();

            _entity.transform.position = position;

            for (int i = 0; i < _particleSystems.Length; i++)
            {

                if (string.Compare(_particleSystems[i].name, "Tornado_1") == 0)
                {
                    _particleSystems[i].transform.localScale = new Vector3(size, size, size);
                }
                else if (string.Compare(_particleSystems[i].name, "Tornado_2") == 0)
                {
                    _particleSystems[i].transform.localScale = new Vector3(size * 1.2f, size * 1.2f, size);
                }
                else if (string.Compare(_particleSystems[i].name, "Tornado_Dust") == 0)
                {
                    _particleSystems[i].transform.localScale = new Vector3(2f / 3f * size, 1f / 3f * size, 2f / 3f * size);
                }
                else if (string.Compare(_particleSystems[i].name, "Tornado_Fog") == 0)
                {
                    _particleSystems[i].transform.localScale = new Vector3(1f / 3f * size, 1f / 3f * size, 1f / 3f * size);
                }
                else
                {
                    _particleSystems[i].transform.localScale = new Vector3(0.68946f / 3f * size, 1f / 3f * size, 0.65615f / 3f * size);
                    _particleSystems[i].transform.position = new Vector3(_particleSystems[i].transform.position.x, (position.y + 6.5f) * (size / 3), _particleSystems[i].transform.position.z);
                }

                materialTornado = _particleSystems[i].GetComponent<Renderer>().materials;
                for (int j = 0; j < materialTornado.Length; j++)
                {
                    if (string.Compare(materialTornado[j].name, "M_tornado (Instance)") == 0 || string.Compare(materialTornado[j].name, "M_tornado2 (Instance)") == 0)
                    {
                        materialTornado[j].SetFloat("Vector1_289BC118", speed);
                    }
                }

                if (string.Compare(_particleSystems[i].name, "Tornado_Fog") == 0)
                {
                    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = _particleSystems[i].velocityOverLifetime;
                    velocityOverLifetime.orbitalZ = -5 * speed;
                }
                else if (string.Compare(_particleSystems[i].name, "Tornado_1") == 1 && string.Compare(_particleSystems[i].name, "Tornado_2") == 1)
                {
                    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = _particleSystems[i].velocityOverLifetime;
                    velocityOverLifetime.orbitalY = 5 * speed;
                }

            }

            return _entity;
        }
    }

    public class Boom : Effect
    {
        public override GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            //_entity = Instantiate((GameObject)Resources.Load("Prefabs\\Effects\\Boom\\Boom", typeof(GameObject)));
            _entity = Instantiate(GameObject.Find("EffectPlayer").GetComponent<EffectLibrary>().BoomPrefab);
            ID = _entity.gameObject.GetInstanceID();
            _particleSystems = _entity.GetComponentsInChildren<ParticleSystem>();
            _visualEffects = _entity.GetComponentsInChildren<VisualEffect>();

            _entity.transform.position = position;

            for (int i = 0; i < _particleSystems.Length; i++)
            {
                var velocityOverLifetime = _particleSystems[i].velocityOverLifetime;
                var colorOverLifetime = _particleSystems[i].colorOverLifetime;
                var sizeOverLifetime = _particleSystems[i].sizeOverLifetime;

                //依照倍率調整爆炸大小。
                _particleSystems[i].transform.localScale = new Vector3(size, size, size);

                //依照速度倍率調整velocityOverLifetime參數。
                AnimationCurve tempAnimeCurve = velocityOverLifetime.speedModifier.curve;
                if (tempAnimeCurve != null)
                {
                    for (int j = 0; j < velocityOverLifetime.speedModifier.curve.length; j++)
                    {
                        tempAnimeCurve.MoveKey(j, new Keyframe(_particleSystems[i].velocityOverLifetime.speedModifier.curve.keys[j].time / speed, _particleSystems[i].velocityOverLifetime.speedModifier.curve.keys[j].value * speed));
                    }
                    ParticleSystem.MinMaxCurve tempCurve = new ParticleSystem.MinMaxCurve(_particleSystems[i].velocityOverLifetime.speedModifier.curveMultiplier, tempAnimeCurve);
                    velocityOverLifetime.speedModifier = tempCurve;
                }

                //依照速度倍率調整colorOverLifetime參數。 
                Gradient tempGradient = colorOverLifetime.color.gradient;
                if (tempGradient != null)
                {
                    GradientColorKey[] tempGradient_ColorKeys = new GradientColorKey[tempGradient.colorKeys.Length];
                    GradientAlphaKey[] tempGradient_AlphaKeys = new GradientAlphaKey[tempGradient.alphaKeys.Length];
                    for (int j = 0; j < colorOverLifetime.color.gradient.colorKeys.Length; j++)
                    {
                        tempGradient_ColorKeys[j].color = tempGradient.colorKeys[j].color;
                        tempGradient_ColorKeys[j].time = tempGradient.colorKeys[j].time / speed;
                    }
                    for (int j = 0; j < colorOverLifetime.color.gradient.alphaKeys.Length; j++)
                    {
                        tempGradient_AlphaKeys[j].alpha = tempGradient.alphaKeys[j].alpha;
                        tempGradient_AlphaKeys[j].time = tempGradient.alphaKeys[j].time / speed;
                    }
                    tempGradient.SetKeys(tempGradient_ColorKeys, tempGradient_AlphaKeys);
                    colorOverLifetime.color = tempGradient;
                }

                //依照速度倍率調整sizeOverLifetime參數。
                tempAnimeCurve = sizeOverLifetime.size.curve;
                if (tempAnimeCurve != null)
                {
                    for (int j = 0; j < _particleSystems[i].sizeOverLifetime.size.curve.length; j++)
                    {
                        tempAnimeCurve.MoveKey(j, new Keyframe(_particleSystems[i].sizeOverLifetime.size.curve.keys[j].time / speed, _particleSystems[i].sizeOverLifetime.size.curve.keys[j].value));
                    }
                    ParticleSystem.MinMaxCurve tempCurve = new ParticleSystem.MinMaxCurve(_particleSystems[i].sizeOverLifetime.size.curveMultiplier, tempAnimeCurve);
                    sizeOverLifetime.size = tempCurve;
                }

                //依照速度倍率調整emission參數。
                ParticleSystem.Burst[] tempBursts = new ParticleSystem.Burst[_particleSystems[i].emission.burstCount];
                if (tempBursts != null)
                {
                    _particleSystems[i].emission.GetBursts(tempBursts);
                    for (int j = 0; j < tempBursts.Length; j++)
                    {
                        tempBursts[j].time /= speed;
                    }
                    _particleSystems[i].emission.SetBursts(tempBursts);
                }
            }

            return _entity;
        }
    }

    public class Effect
    {
        protected int ID;
        protected float TotalTime = 0f;
        protected float StartTime = 0f;
        protected GameObject _entity;
        protected ParticleSystem[] _particleSystems;
        protected VisualEffect[] _visualEffects;

        public virtual void Play()
        {
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                _particleSystems[i].Play();
            }

            for (int i = 0; i < _visualEffects.Length; i++)
            {
                _visualEffects[i].Play();
            }
        }

        public void Stop()
        {
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                _particleSystems[i].Clear();
                _particleSystems[i].Stop();

            }

            for (int i = 0; i < _visualEffects.Length; i++)
            {
                _visualEffects[i].Stop();
            }
        }

        public virtual GameObject Setup(Vector3 position, Vector3 rotation, float size, float speed)
        {
            return null;
        }

        public GameObject GetEntity
        {
            get
            {
                return _entity;
            }
        }

        public int GetID
        {
            get
            {
                return ID;
            }
        }
        public float GetTotalTime
        {
            get
            {
                return TotalTime;
            }
        }

        public float GetStartTime
        {
            get
            {
                return StartTime;
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle > 360f)
        {
            angle -= 360f;
        }

        if (angle < -360f)
        {
            angle += 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
