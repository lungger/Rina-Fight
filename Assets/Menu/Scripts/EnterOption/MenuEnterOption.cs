using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuEnterOption : EnterOptions
{

    public override void EnterOption(OptionsPanelData optionData, GameObject prePage)
    {
        if (optionData.Option == "Setting")
        {
            optionData.OptionPage.SetActive(true);
            prePage.SetActive(false);
        }
        else if (optionData.Option == "Volume")
        {
            optionData.OptionPage.SetActive(true);
            prePage.SetActive(false);
        }
        else if (optionData.Option == "Resolution")
        {
            Debug.Log("no page");
        }
        else if (optionData.Option == "Other")
        {
            Debug.Log("no page");
        }
        else if (optionData.Option == "Return")
        {
            GameState.ReStartGame();
            LoadingSceneController.NextScene = "TitleView";
            SceneManager.LoadScene("LoadingScene");
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
