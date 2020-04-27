using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScreenShooter : MonoBehaviour
{
    string no_breaking_space = "\u00A0"; //不換行處理
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

    [SerializeField]
    GameObject danmu;

    void CreateDanmu()
    {
        string messageString = _randomPlayBall[Random.Range(0, _randomPlayBall.Length)].Replace(" ", no_breaking_space);
        Debug.Log(messageString);
        Color[] colorArray = new Color[] { Color.red, Color.blue, Color.green, Color.black, Color.white, Color.cyan };
        Color newColor = colorArray[Random.Range(0, colorArray.Length)];
        GameObject newDanmu = Instantiate(danmu, transform);
        BulletScreen danmuScript = newDanmu.GetComponent<BulletScreen>();
        danmuScript.Speed = Random.Range(250.0f, 300.0f);
        danmuScript.SetFontSize((int)Random.Range(72.0f, 144.0f));
        danmuScript.SetColor(newColor);
        danmuScript.SetText(messageString);
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CreateDanmu", 0.3f, 2.0F);
        //Invoke("CreateDanmu", 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
