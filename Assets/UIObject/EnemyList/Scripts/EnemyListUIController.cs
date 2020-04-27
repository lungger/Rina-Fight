using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets;

public class EnemyListUIController : MonoBehaviour
{
    [Header("Enemy List 的 Base Panel")]
    [SerializeField]
    GameObject _panel;

    public List<Enemy> _enemyList = new List<Enemy>();

    List<GameObject> _enemyListUI;

    bool _isLock = false;
    int _nowLockEnemyIndex = -1;
    int _iterIndex = 0;

    const int MAX_SIZE = 3;

    #region 屬性
    public int NowLockEnemyIndex
    {
        get
        {
            return _nowLockEnemyIndex;
        }
        set
        {
            _nowLockEnemyIndex = value;
            if (_nowLockEnemyIndex < 0)
                _nowLockEnemyIndex = _enemyList.Count - 1;
            else if (_nowLockEnemyIndex >= _enemyList.Count)
                _nowLockEnemyIndex = 0;
        }
    }

    public int IconIterIndex
    {
        get
        {
            return _iterIndex;
        }
        set
        {
            _iterIndex = value;
            if (_iterIndex < 0)
                _iterIndex = 0;
            else if (_enemyList.Count >= MAX_SIZE && _iterIndex >= MAX_SIZE)
                _iterIndex = MAX_SIZE - 1;
            else if (MAX_SIZE >= _enemyList.Count && _iterIndex >= _enemyList.Count)
                _iterIndex = 0;
        }
    }

    public int LeftLimit
    {
        get
        {
            int limit = NowLockEnemyIndex - IconIterIndex;
            /*if (_enemyList.Count < MAX_SIZE)
                return limit;
            else */
            if (limit < 0)
                return _enemyList.Count + limit;
            else
                return limit;
        }
    }
    #endregion

    #region 函數實作
    public void Initialize(List<Enemy> enemyList)
    {
        _isLock = false;
        NowLockEnemyIndex = 0;
        _iterIndex = 0;
        SetEnemyList(enemyList);
    }
    
    public void SetEnemyList(List<Enemy> newEnemyList)
    {
        _enemyList = newEnemyList;
        SetEnemyIcon();
    }

    public void SetIconIndex(int step)
    {
        if (step < 0)
        {
            SetIterVisable(false);
            IconIterIndex--;
            SetIterVisable(true);
        }
        else if(step > 0)
        {
            SetIterVisable(false);
            IconIterIndex++;
            SetIterVisable(true);
        }
    }

    public void LockEnemy()
    {
        NowLockEnemyIndex = 0;
        IconIterIndex = 0;
        _isLock = true;
        SetEnemyIcon();
        SetIterVisable(true);
    }

    public void UnLockEnemy()
    {
        SetIterVisable(false);
        _isLock = false;
    }

    //切換左邊敵人
    public void SwitchLeftEnemy()
    {
        SetIterVisable(false);

        #region 換Icon / iter
        NowLockEnemyIndex--;
        IconIterIndex--;
        #endregion

        SetIterVisable(true);
        SetEnemyIcon();
    }

    //切換右邊敵人
    public void SwitchRightEnemy()
    {
        SetIterVisable(false);

        #region 換Icon / iter
        NowLockEnemyIndex++;
        IconIterIndex++;
        #endregion

        SetIterVisable(true);
        SetEnemyIcon();
    }

    
    public void SetLockEnemyIndex(int enemyIndex)
    {
        if (enemyIndex != -1 && _isLock)
        {
            if (IsSwitchLeft(enemyIndex))
            {
                SwitchLeftEnemy();
            }
            else
            {
                SwitchRightEnemy();
            }
        }
        else
        {
            _isLock = false;
            UnLockEnemy();
        }
    }

    bool IsSwitchLeft(int enemyIndex)
    {
        if (_nowLockEnemyIndex == 0 && enemyIndex == _enemyList.Count - 1)
            return true;
        else if (_nowLockEnemyIndex == _enemyList.Count - 1 && enemyIndex == 0)
            return false;
        else if (enemyIndex < _nowLockEnemyIndex)
            return true;
        else
            return false;
    }

    bool IsSwitchRight(int enemyIndex)
    {
        if (_nowLockEnemyIndex == _enemyList.Count - 1 && enemyIndex == 0)
            return true;
        else if (enemyIndex > _nowLockEnemyIndex)
            return true;
        else
            return false;
    }

    //Enemy死亡時呼叫
    public void OnEnemyDie(List<Enemy> enemyList, int strategy)
    {
        if (strategy == 0)
            SetIterVisable(false);
        SetEnemyList(enemyList);
        SetEnemyIcon();
    }

    public void ResetIterFalse()
    {
        for(int i = 0; i < MAX_SIZE; i++)
        {
            SetIterVisable(false);
        }
    }

    void SetIterVisable(bool visable)
    {
        if (_isLock)
        {
            _enemyListUI[_iterIndex].transform.GetChild(0).gameObject.SetActive(visable);
        }
    }

    public void SetEnemyIcon()
    {   
        for (int i = 0; i < MAX_SIZE; i++)
        {
            if (i < _enemyList.Count)
            {
                _enemyListUI[i].SetActive(true);
                _enemyListUI[i].GetComponent<Image>().sprite = _enemyList[GetEnemyIndex(i)].iconSprite;
            }
            else
            {
                if (IconIterIndex == i)
                {
                    SetIterVisable(false);
                    IconIterIndex = i - 1;
                    SetIterVisable(true);
                }
                _enemyListUI[i].SetActive(false);
            }
        }
    }

    int GetEnemyIndex(int index)
    {
        int enemyIndex = LeftLimit + index;
        if (enemyIndex < _enemyList.Count)
            return enemyIndex;
        else
            return enemyIndex - _enemyList.Count;
    }
    #endregion

    void Awake()
    {
        _panel = this.gameObject;
        _enemyListUI = ChildrenFinder.FindByName(_panel, "Img_EnemyIconUI");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
