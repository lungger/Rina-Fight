using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class describeController : MonoBehaviour
{

    Text _thisText;

    List<string> _describeList = new List<string>();

    public void SetDescribeText(int index)
    {
        if (index < _describeList.Count)
        {
            _thisText.text = _describeList[index];
        }
    }

    private void Awake()
    {
        _thisText = gameObject.GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _describeList.Add("確定鍵/遊戲中跳躍");
        _describeList.Add("取消鍵/遊戲中攻擊");
        _describeList.Add("施放技能1");
        _describeList.Add("施放技能2");
        _describeList.Add("上方選擇/切換武器");
        _describeList.Add("下方選擇/切換武器");
        _describeList.Add("左方選擇/切換武器");
        _describeList.Add("右方選擇/切換武器");
        _describeList.Add("向前移動");
        _describeList.Add("向後移動");
        _describeList.Add("向左移動");
        _describeList.Add("向右移動");
        _describeList.Add("向上旋轉視角");
        _describeList.Add("向下旋轉視角");
        _describeList.Add("向左旋轉視角");
        _describeList.Add("向右旋轉視角");
        _describeList.Add("打開/關閉選單");
        _describeList.Add("擺出特殊動作1");
        _describeList.Add("切換成鎖定左邊敵人");
        _describeList.Add("切換成鎖定右邊敵人");
        _describeList.Add("跑步");
        _describeList.Add("防禦/閃避");
        _describeList.Add("切換鏡頭");
        _describeList.Add("擺出特殊動作2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
