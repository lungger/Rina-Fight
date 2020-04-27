using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//讀取頁面
public class LoadingSceneController : MonoBehaviour
{
    [Header("讀取條本體")]
    public Image LoadingBar;

    [Header("讀取條的%數")]
    public Text LoadingValueText;

    [Header("下個場景的名字")]
    static public string NextScene = "terrainBuild";

    [Header("讀取條上方Icon")]
    public GameObject runner;

    Vector3 runnerInitPos;

    private void Awake()
    {
        //自動查找
        if (LoadingBar == null)
            LoadingBar = ChildrenFinder.FindByName(gameObject, "Image_LoadingBar", 0).GetComponent<Image>();
        if (LoadingValueText == null)
            LoadingValueText = ChildrenFinder.FindByName(gameObject, "Text_LoadingValue", 0).GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        runnerInitPos = runner.transform.localPosition;
        StartCoroutine("LoadingScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetRunnerPosition(float loadingValue)
    {
        float deltax = gameObject.transform.GetComponent<RectTransform>().rect.width;
        Vector3 newRunnerPos = new Vector3(runnerInitPos.x + deltax * loadingValue, runnerInitPos.y, runnerInitPos.z);
        runner.transform.localPosition = newRunnerPos;
    }

    IEnumerator LoadingScene()
    {
        float loadingValue = 0.0f;
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(NextScene);
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone) //場景載入未完成
        {
            if (asyncScene.progress < 0.9f) //載入中
                loadingValue = asyncScene.progress;
            else //超過0.9 算載入完成
                loadingValue = 1.0f;

            LoadingBar.fillAmount = loadingValue; //設置圖片填滿%數
            LoadingValueText.text = (loadingValue * 100).ToString("F2") + " %"; //設置目前進度%數的文字
            SetRunnerPosition(loadingValue);

            if (loadingValue >= 0.9f)
            {
                asyncScene.allowSceneActivation = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
