using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingOptionEnter : EnterOptions
{
    public override void EnterOption(OptionsPanelData optionData, GameObject prePage)
    {
        if (optionData.Option == "Joystick")
        {
            optionData.OptionPage.SetActive(true);
            prePage.SetActive(false);
        }
        else if (optionData.Option == "Keyboard")
        {
            optionData.OptionPage.SetActive(true);
            prePage.SetActive(false);
        }
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
