using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTips : MonoBehaviour
{
    public Image thisImage;
    //髒髒臭臭
    public GameCharatcer player;

    //髒髒臭臭v2
    public EnemyCreator enemyCreator;

    [Header("失敗畫面")]
    public Sprite LoseImg;

    [Header("勝利畫面")]
    public Sprite WinImg;

    [Header("持續時間")]
    public float continueTime = 2.0f;

    [Header("幾秒後開始出現畫面")]
    public float showDelay = 1.33f;

    [Header("彈幕攻擊")]
    public YuTongTree_Attack attack;

    bool isFadeIn = false;



    #region 函數實作
    void FadeIn(Image image)
    {
        if (isFadeIn)
        {
            image.color += new Color(0, 0, 0, Time.deltaTime * 0.55f);

            if (image.color.a >= 1f)
            {
                isFadeIn = false;
                Invoke("JumpSceneToVictory", continueTime);
            }
        }
    }

    void JumpSceneToTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleView");
    }

    void JumpSceneToVictory()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("VictoryScene");
    }

    void SetOrderToLast()
    {
        gameObject.transform.SetAsLastSibling();
    }

    void BulletScreenAttack()
    {
        attack.Attack();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        thisImage = gameObject.GetComponent<Image>();
        attack = gameObject.GetComponent<YuTongTree_Attack>();
        enemyCreator = GameObject.Find("EnemyCreator").GetComponent<EnemyCreator>();
        isFadeIn = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<GameCharatcer>();
        //StartCoroutine("SetFadeIn");
        StartCoroutine("SetWinSpite");
        StartCoroutine("BulletScreenOnDie");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FadeIn(thisImage);
        //SetOrderToLast();
    }

    IEnumerator SetFadeIn()
    {
        yield return new WaitUntil(() => player.isDie == true);
        thisImage.sprite = LoseImg;
        yield return new WaitForSeconds(showDelay);
        isFadeIn = true;
    }

    IEnumerator SetWinSpite()
    {
        yield return new WaitUntil(() => enemyCreator.count >= 6);
        //thisImage.sprite = WinImg;
        yield return new WaitForSeconds(showDelay);
        isFadeIn = true;
    }

    IEnumerator BulletScreenOnDie()
    {
        yield return new WaitUntil(() => player.isDie == true);
        InvokeRepeating("BulletScreenAttack", 0.0f, 1.0f);
        Invoke("JumpSceneToTitle", 4.0f);
    }
}
