using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictorySceneUIController : MonoBehaviour
{
    [Header("白幕")]
    public Image WhiteImage;

    [Header("勝利畫面")]
    public GameObject VictoryView;

    [Header("每一關時間的Layout")]
    public GameObject StageLayout;

    [Header("每一關時間的文字(自動查找)")]
    public List<GameObject> StageTextList;

    [Header("出現一個Stage文字的間隔時間")]
    public float showTextDelay = 1.5f;

    [Header("亂數跑幾禎")]
    public int randomSize = 80;

    public List<float> StageTime;

    public bool showVictory = false;

    bool isFadeOut = true;

    public int stage = 0;
    // Start is called before the first frame update
    void Start()
    {
        stage = 0;
        if (StageLayout != null)
        {
            StageTextList = ChildrenFinder.FindByName(StageLayout, "Text_Stage");
        }
        WhiteImage = ChildrenFinder.FindByName(gameObject, "Image_WhiteImage", 0).GetComponent<Image>();
        #region 創建 Stage Time List
        StageTime.Add(GameStageTimer.Stage1Time - GameStageTimer.StartTime);
        StageTime.Add(GameStageTimer.Stage2Time - GameStageTimer.Stage1Time);
        StageTime.Add(GameStageTimer.Stage3Time - GameStageTimer.Stage2Time);
        StageTime.Add(GameStageTimer.Stage4Time - GameStageTimer.Stage3Time);
        StageTime.Add(GameStageTimer.Stage5Time - GameStageTimer.Stage4Time);
        #endregion

        StartCoroutine("ShowVictory");
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeOut)
        {
            if (WhiteImage.color.a > 0.0f)
            {
                WhiteImage.color -= new Color(0, 0, 0, Time.deltaTime * 0.55f);
            }
            else
            {
                isFadeOut = false;
                Invoke("SetShowVictory", 1.0f);
            }
        }
    }

    void SetShowVictory()
    {
        showVictory = true;
    }

    IEnumerator ShowVictory()
    {
        yield return new WaitUntil(() => showVictory);
        VictoryView.SetActive(true);
        yield return new WaitForSeconds(showTextDelay);
        StartCoroutine("ShowStageText");
    }

    IEnumerator ShowStageText()
    {
        for (int i = 0; i < StageTextList.Count; i++)
        {
            StageTextList[i].SetActive(true);
            string StageIndex = "Stage " + (i + 1).ToString() + ": ";
            Text nowText = StageTextList[i].GetComponent<Text>();
            for (int k = 0; k < randomSize; k++)
            {
                float random = Random.Range(0f, 100.0f);
                nowText.text = StageIndex + random.ToString("F2") + " s";
                yield return null;
            }
            nowText.text = StageIndex + StageTime[i].ToString("F2") + " s";
            yield return new WaitForSeconds(showTextDelay);
        }
        Invoke("JumpSceneToTitle", 8.0f);
    }

    void JumpSceneToTitle()
    {
        SceneManager.LoadScene("TitleView");
    }
}
