using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterOptions : MonoBehaviour
{
    /// <summary> 確定進入選項 (option: 選項代號, prePage: 呼叫EnterOption的那一頁) </summary>
    public virtual void EnterOption(OptionsPanelData optionData, GameObject prePage)
    {
        //子類別決定要進入哪個選項
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
