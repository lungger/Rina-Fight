using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollowV2 : MonoBehaviour
{
    [Header("操作角色的相機")]
    [SerializeField]
    Camera _camera;

    [Header("目標敵人")]
    [SerializeField]
    GameCharatcer _target;

    public HpUpdater HpView;

    //切換鎖定的目標 (敵人)
    public void LockTarget(GameCharatcer target)
    {
        if (target != null)
        {
            _target = target;
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);
        }
        else
        {
            _target = target;
            gameObject.SetActive(false);
        }
    }

    //取消鎖定目標
    public void UnLockTarget()
    {
        LockTarget(null);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.IsPause && _target != null) //有目標存在才換位
        {
            Vector3 _targetPos = _target.CenterPosition;
            Vector2 thisPos = RectTransformUtility.WorldToScreenPoint(_camera, _targetPos);
            gameObject.transform.position = thisPos;
        }
        if (_target == null)
        {
            if (gameObject.activeInHierarchy == true)
                gameObject.SetActive(false);
        }
    }
}
