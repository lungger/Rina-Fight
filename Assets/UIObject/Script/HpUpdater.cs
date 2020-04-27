using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUpdater : MonoBehaviour
{
    //扣血直接設置NowHp

    #region 變數宣告

    Image _imageHp;

    #region - 最大HP -
    [SerializeField]
    float _maxHp = 100.0f;

    public float MaxHp
    {
        get
        {
            return _maxHp;
        }
        set
        {
            if (value <= 0)
                _maxHp = 1;
            else
                _maxHp = value;
        }
    }
    #endregion

    #region - 目前HP - 
    [SerializeField]
    float _nowHp = 100.0f;

    public float NowHp
    {
        get
        {
            return _nowHp;
        }
        set
        {
            if (value <= 0)
                _nowHp = 0;
            else
                _nowHp = value;
        }
    }
    #endregion

    //是否直接設置Hp
    public bool IsSetHp = false;

    [Header("扣血的速度")]
    public float LerpSpeed = 12.0f;

    #endregion
        
    #region 函數實作
    float GetFillAmount()
    {
        return NowHp / MaxHp;
    }

    /// <summary> 直接扣血 </summary>
    public void SetHpWithNoAnimation(float maxHp, float nowhp)
    {
        IsSetHp = true;
    }
    #endregion

    void Awake()
    {
        _imageHp = gameObject.GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSetHp == false)
        {
            _imageHp.fillAmount = Mathf.Lerp(_imageHp.fillAmount, GetFillAmount(), LerpSpeed * Time.deltaTime);
        }
        else
        {
            _imageHp.fillAmount = GetFillAmount();
            IsSetHp = false;
        }
    }
}
