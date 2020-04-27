using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MessageBoardTester : MonoBehaviour, IDragHandler
{
    //測試留言板用的
    // Start is called before the first frame update

    [SerializeField]
    GameObject Txt_Message;

    [SerializeField]
    GameObject MessageBoardContent;

    [SerializeField]
    ScrollRect scrollCtrl;

    /*處理不換行 參考 https://www.itread01.com/content/1541751628.html */
    string no_breaking_space = "\u00A0"; //不換行處理

    //但其實瀏覽模式還沒做好
    bool isShootMessage = false; //發射一條訊息到聊天室時，留言板調到最下面
    bool isBrowseMessageMode = false; //瀏覽訊息模式: 瀏覽訊息模式中 當留言板更新不會拉到最下面

    string[] _randomPlayBall = new string[] { "劉建宏: 陳偉凱打球",
        "劉建宏: 陳偉凱打球拉打球拉打球拉打球拉",
        "劉建宏: 陳偉凱打球打球拉打球拉打球拉打球拉打球拉打球打球拉拉",
        "陳☆偉★凱: 從來沒有",
        "陳☆偉★凱: 從來沒...嘿嘿嘿呵呵呵",
        "陳☆偉★凱: 不要把邏輯...寫在View裡",
        "陳☆偉★凱: 這是兩碼子事吧",
        "陳☆偉★凱: 你們這個邏輯不要寫在View裡，應該寫在Presentation Model，然後每按一次按鈕或什麼動作，就從Presentation Model抓取變數，一次更新，當然啦，這樣做效率是很差...嘿嘿嘿，但是微軟的介面也是這樣做的",
        "陳☆偉★凱: 天下沒有白吃的 午餐拉吼...嘿嘿呵呵呵，既然要做Data Binding就要寫很多綁定式",
        "陳☆偉★凱: 張無忌在練降龍十八掌的時候，練到第七招時越練越不對勁，乾脆連後面十九招都不練了嘿嘿呵呵呵",
        "陳彥霖: 我看到各位上課在睡覺，心裡很是欣慰",
        "♥♦郭忠義: 同學~~你在幹嘛",
        "某人: 靠～夭",
        "另一個某人: ㄟ~~~",
        "Ahhhhh~~~"
    };

    void Test()
    {
        Debug.Log("qerasdf");
    }

    
    public void OnDrag(PointerEventData data)
    {
        scrollCtrl.OnDrag(data);
        Test();
        if (scrollCtrl.verticalNormalizedPosition <= 0.0f)
            isBrowseMessageMode = false;
        else
            isBrowseMessageMode = true;
    }

    void BuildMessage() 
    {
        GameObject message = Instantiate(Txt_Message, MessageBoardContent.transform);
        string messageString = _randomPlayBall[Random.Range(0, _randomPlayBall.Length)].Replace(" ", no_breaking_space);
        message.GetComponent<Text>().text = messageString;
        StartCoroutine("ScrollToBottom");
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollCtrl.verticalNormalizedPosition = 0.000f;
    }

    IEnumerator SetBrowseMode()
    {
        yield return new WaitForEndOfFrame();
        if (scrollCtrl.verticalNormalizedPosition <= 0.0f)
            isBrowseMessageMode = false;
        else
            isBrowseMessageMode = true;
    }

    void Start()
    {
        InvokeRepeating("BuildMessage", 0.5f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("isBrowseMessageMode: " + isBrowseMessageMode.ToString());
    }
}
