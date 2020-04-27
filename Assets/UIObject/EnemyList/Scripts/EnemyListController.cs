using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListController : MonoBehaviour
{
    [Header("鎖定怪物死亡時策略")]
    [Tooltip("0: 解除鎖定\n1: 鎖定下一隻")]
    public int LockEnemyDieStrategy = 1;
    
    [Header("鎖定怪物離開鎖定範圍時策略")]
    [Tooltip("0: 解除鎖定\n1: 鎖定下一隻")]
    public int LockEnemyExitDistanceStrategy = 0;

    [Header("角色能鎖定怪物的最大距離")]
    [Tooltip("在範圍內會進入鎖定清單")]
    public float MaxDistanceLimit = 15.0f;

    public GameObject Player;

    public List<Enemy> _enemyList = new List<Enemy>();

    [SerializeField]
    EnemyListUIController _enemyListUIController;
    
    int _targetIndex = 0;

    bool isLock = false;

    bool isLockEnemyDieTrigger = false; //鎖定怪物死亡時觸發器
    
    #region 函數實作
    public Enemy GetTargetEnemy()
    {
        if (isLock && _enemyList.Count > 0 && _targetIndex < _enemyList.Count)
        {
            return _enemyList[_targetIndex];
        }
        return null;
    }

    public void UnLockTarget()
    {
        _targetIndex = 0;
        _enemyListUIController.UnLockEnemy();
        isLock = false;
    }

    public void LockTarget()
    {
        if (_enemyList.Count > 0)
        {
            _targetIndex = 0;
            _enemyList = GetEnemyList();
            _enemyListUIController.LockEnemy();
            isLockEnemyDieTrigger = false;
            isLock = true;
        }
    }

    public void SwitchLeftEnemy()
    {
        if (isLock) //鎖定中
        {
            _targetIndex--;
            if (_targetIndex < 0)
                _targetIndex = _enemyList.Count - 1;
            _enemyListUIController.SetLockEnemyIndex(_targetIndex);
            isLockEnemyDieTrigger = false;
        }
    }

    public void SwitchRightEnemy()
    {
        if (isLock) //鎖定中
        {
            _targetIndex++;
            if (_targetIndex >= _enemyList.Count)
                _targetIndex = 0;
            _enemyListUIController.SetLockEnemyIndex(_targetIndex);
            isLockEnemyDieTrigger = false;
        }
    }

    //Enemy Die
    public Enemy OnLockEnemyDie()
    {
        if (isLockEnemyDieTrigger == false)
        {
            isLockEnemyDieTrigger = true;
            _enemyList.Remove(GetTargetEnemy());

            #region 鎖定怪物死亡時策略 - 解除鎖定
            if (LockEnemyDieStrategy == 0 || _enemyList.Count == 0) //策略0 或是 鎖定清單沒有敵人時跳出
            {
                _enemyListUIController.OnEnemyDie(_enemyList, 0);
                _enemyListUIController.ResetIterFalse(); //重設Iter 為不顯示
                _targetIndex = 0;
                isLock = false;
                return null;
            }
            #endregion

            #region 鎖定怪物死亡時策略 - 鎖定下一隻
            else if (LockEnemyDieStrategy == 1)
            {
                isLockEnemyDieTrigger = false;
                if (_targetIndex >= _enemyList.Count)
                {
                    _targetIndex = 0;
                    _enemyListUIController.NowLockEnemyIndex = _targetIndex;
                }
                _enemyListUIController.OnEnemyDie(_enemyList, 1);
                return GetTargetEnemy();
            }
            #endregion
        }
        return null;
    }

    public Enemy OnLockEnemyExitDistance()
    {
        _enemyList.Remove(GetTargetEnemy());

        #region 鎖定怪物離開鎖定範圍時策略 - 解除鎖定
        if (LockEnemyExitDistanceStrategy == 0 || _enemyList.Count == 0) //策略0 或是 鎖定清單沒有敵人時跳出
        {
            _enemyListUIController.OnEnemyDie(_enemyList, 0);
            _enemyListUIController.ResetIterFalse(); //重設Iter 為不顯示
            _targetIndex = 0;
            isLock = false;
            return null;
        }
        #endregion

        #region 鎖定怪物死亡時策略 - 鎖定下一隻
        else if (LockEnemyExitDistanceStrategy == 1)
        {
            if (_targetIndex >= _enemyList.Count)
            {
                _targetIndex = 0;
                _enemyListUIController.NowLockEnemyIndex = _targetIndex;
            }
            _enemyListUIController.OnEnemyDie(_enemyList, 1);
            return GetTargetEnemy();
        }
        #endregion
        return null;
    }

    // 鎖定狀態下更新EnemyList (主要用於範圍內有新敵人出現時加入list)
    void UpdateEnemyListOnLock()
    {
        List<Enemy> _updateEnemyList = new List<Enemy>();
        _updateEnemyList = GetEnemyList();
        //感覺Remove效能很差 
        //for (int i = 0; i < _enemyList.Count; i++)
        //{
        //    _updateEnemyList.Remove(_enemyList[i]);
        //}

        //if (_updateEnemyList.Count > 0)
        //{
        //    for (int i = 0; i < _updateEnemyList.Count; i++)
        //    {
        //        _enemyList.Add(_updateEnemyList[i]);
        //    }
        //}
        bool isAddEnemy = false;
        for (int i = 0; i < _updateEnemyList.Count; i++)
        {
            bool inList = false;
            for (int j = 0; j < _enemyList.Count; j++)
            {
                if (_updateEnemyList[i] == _enemyList[j])
                {
                    inList = true;
                    break;
                }
            }

            if (!inList)
            {
                isAddEnemy = true;
                _enemyList.Add(_updateEnemyList[i]);
            }
        }

        if (isAddEnemy)
        {
            _enemyListUIController.SetEnemyList(_enemyList);
        }
    }

    //非鎖定狀態下更新EnemyList
    public void UpdateEnemyList()
    {
        _enemyList = GetEnemyList();
        _enemyListUIController.Initialize(_enemyList);
    }
    
    //離開鎖定範圍
    public bool IsExitDistance(GameCharatcer enemy)
    {
        return GetDistance(enemy, Player) > MaxDistanceLimit;
    }

    public List<Enemy> GetEnemyList()
    {
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        List<Enemy> enemyList = new List<Enemy>();
        for (int i = 0; i < enemyArray.Length; i++)
        {
            Enemy enemy = enemyArray[i].GetComponent<Enemy>();
            if (!enemy.isDie && GetDistance(enemy, Player) <= MaxDistanceLimit)
            {
                enemyList.Add(enemy);
            }
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            for (int j = 0; j < enemyList.Count - 1; j++)
            {
                if (GetDistance(enemyList[j], Player) > GetDistance(enemyList[j + 1], Player))
                {
                    Enemy temp = enemyList[j + 1];
                    enemyList[j + 1] = enemyList[j];
                    enemyList[j] = temp;
                }
            }
        }
        
        return enemyList;
    }

    float GetDistance(GameCharatcer enemy, GameObject Player)
    {
        Vector3 dir = enemy.transform.position - Player.transform.position;
        return dir.magnitude;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        UpdateEnemyList();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLock)
        {
            for (int i = 0; i < _enemyList.Count; i++)
            {
                if (i != _targetIndex &&(_enemyList[i].isDie || GetDistance(_enemyList[i], Player) > MaxDistanceLimit ))
                {
                    _enemyList.RemoveAt(i);

                    if (_targetIndex > i || IsMoveIconIter(i))
                        _enemyListUIController.SetIconIndex(-1);

                    if (_targetIndex > i)
                        _targetIndex--;
                    _enemyListUIController.NowLockEnemyIndex = _targetIndex;
                    _enemyListUIController.SetEnemyList(_enemyList);
                }
            }
            UpdateEnemyListOnLock();
        }
    }

    //髒髒
    bool IsMoveIconIter(int deleteIndex)
    {
        if (_enemyListUIController.IconIterIndex == 1)
        {
            return (deleteIndex == _enemyListUIController.LeftLimit);
        }
        else if (_enemyListUIController.IconIterIndex == 2)
        {
            return (deleteIndex == _enemyListUIController.LeftLimit) ||
                    deleteIndex == _enemyListUIController.LeftLimit + 1;
        }
        return false;
    }
}
