using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//控制選項的Panel 顯示效果 
public class OptionsPanelUI : MonoBehaviour
{
    Image _background;
    Color _initBgColor;

    #region 函數實作
    // 進入此Panel
    public void EnterOption()
    {
        _background.color = _initBgColor;
    }

    // 離開此Panel
    public void ExitOption()
    {
        _background.color = new Color(_initBgColor.r, _initBgColor.g, _initBgColor.b, 0);
    }
    #endregion

    //初始化參數
    private void Awake() 
    {
        _background = gameObject.transform.GetComponent<Image>();
        _initBgColor = _background.color;
        ExitOption();
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
