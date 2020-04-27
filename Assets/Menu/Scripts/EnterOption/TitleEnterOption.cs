using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleEnterOption : EnterOptions
{
    const string GAME_SCENE_NAME = "terrainBuild";
    const string LOAD_SCENE = "LoadingScene";

    public override void EnterOption(OptionsPanelData optionData, GameObject prePage)
    {
        if (optionData.Option == "Continue") //繼續遊戲
        {
            LoadingSceneController.NextScene = GAME_SCENE_NAME;
            SceneManager.LoadScene(LOAD_SCENE);
        }
        else if(optionData.Option == "Options")
        {
            optionData.OptionPage.SetActive(true);
            prePage.SetActive(false);
        }
        else if (optionData.Option == "Tutorial")
        {
            optionData.OptionPage.SetActive(true);
            prePage.SetActive(false);
        }
        else if(optionData.Option == "Exit")
        {
            Application.Quit();
            Debug.Log("Enter Exit");
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
