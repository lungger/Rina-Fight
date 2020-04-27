using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [Header("史萊姆")]
    public GameObject Slime;

    [Header("燃燒鼠")]
    public GameObject FireMouse;

    [Header("李相鱷")]
    public GameObject PeralCrocodile;

    [Header("金童樹")]
    public GameObject JinTongTree;

    [Header("目前怪物波數")]
    public int count = 0;

    bool isNoEnemy = false;
    bool canCount = false;
    float initMouseY = 1.167f;

    Dictionary<string, GameObject> enemyDict = new Dictionary<string, GameObject>();

    public List<KeyValuePair<string, Vector3>> Counter1 = new List<KeyValuePair<string, Vector3>>();
    public List<KeyValuePair<string, Vector3>> Counter2 = new List<KeyValuePair<string, Vector3>>();
    public List<KeyValuePair<string, Vector3>> Counter3 = new List<KeyValuePair<string, Vector3>>();
    public List<KeyValuePair<string, Vector3>> Counter4 = new List<KeyValuePair<string, Vector3>>();
    public List<KeyValuePair<string, Vector3>> Counter5 = new List<KeyValuePair<string, Vector3>>();

    int totalEnemyCount = 2;

    void SetDictionary()
    {
        initMouseY = FireMouse.transform.position.y;
        #region 預設怪物
        enemyDict["Slime"] = Slime;
        enemyDict["FireMouse"] = FireMouse;
        enemyDict["PeralCrocodile"] = PeralCrocodile;
        enemyDict["JinTongTree"] = JinTongTree;
        #endregion

        #region 第一波
        Counter1.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(90.01f, initMouseY, 123.1619f)));
        Counter1.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(108.02f, initMouseY, 116.42f)));

        Counter1.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(108.02f, initMouseY, 123.1619f)));
        Counter1.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(90.01f, initMouseY, 116.42f)));
        #endregion

        #region 第二波
        Counter2.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(72.99f, 0.0f, 121.84f)));
        Counter2.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(97.59f, 0.0f, 140.45f)));
        Counter2.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(123.4f, 0.0f, 121.12f)));
        Counter2.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(112.79f, 0.0f, 90.61f)));
        Counter2.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(82.77f, 0.0f, 90.97f)));

        Counter2.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(88.5f, 0.0f, 125.1f)));
        Counter2.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(106.18f, 0.0f, 116.42f)));
        Counter2.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(83.21f, 0.0f, 108.1f)));
        Counter2.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(97.46f, 0.0f, 96.85f)));
        Counter2.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(111.64f, 0.0f, 107.72f)));
        #endregion

        #region 第三波
        Counter3.Add(new KeyValuePair<string, Vector3>("FireMouse", new Vector3(89.46f, initMouseY, 123.1619f)));
        Counter3.Add(new KeyValuePair<string, Vector3>("FireMouse", new Vector3(108.02f, initMouseY, 123.1619f)));
        Counter3.Add(new KeyValuePair<string, Vector3>("FireMouse", new Vector3(108.02f, initMouseY, 116.42f)));
        Counter3.Add(new KeyValuePair<string, Vector3>("FireMouse", new Vector3(89.46f, initMouseY, 108.28f)));
        #endregion

        #region 第四波
        Counter4.Add(new KeyValuePair<string, Vector3>("JinTongTree", new Vector3(90.01f, 0.0f, 123.1619f)));
        Counter4.Add(new KeyValuePair<string, Vector3>("JinTongTree", new Vector3(108.02f, 0.0f, 123.1619f)));
        Counter4.Add(new KeyValuePair<string, Vector3>("JinTongTree", new Vector3(108.02f, 0.0f, 116.42f)));
        Counter4.Add(new KeyValuePair<string, Vector3>("JinTongTree", new Vector3(89.46f, 0.0f, 108.28f)));

        Counter4.Add(new KeyValuePair<string, Vector3>("FireMouse", new Vector3(106.67f, initMouseY, 132.01f)));
        Counter4.Add(new KeyValuePair<string, Vector3>("FireMouse", new Vector3(88.28f, initMouseY, 132.01f)));

        Counter4.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(114.97f, 0.0f, 111.41f)));
        Counter4.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(77.04f, 0.0f, 111.41f)));
        #endregion

        #region 第五波
        Counter5.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(100.61f, 0.0f, 120.08f)));
        Counter5.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(93.41f, 0.0f, 120.08f)));
        Counter5.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(100.61f, 0.0f, 105.67f)));
        Counter5.Add(new KeyValuePair<string, Vector3>("Slime", new Vector3(93.41f, 0.0f, 105.67f)));

        Counter5.Add(new KeyValuePair<string, Vector3>("JinTongTree", new Vector3(102.95f, 0.0f, 101.56f)));
        Counter5.Add(new KeyValuePair<string, Vector3>("JinTongTree", new Vector3(77.4f, 0.0f, 101.56f)));

        Counter5.Add(new KeyValuePair<string, Vector3>("FireMouse", new Vector3(104.31f, initMouseY, 127.31f)));
        Counter5.Add(new KeyValuePair<string, Vector3>("FireMouse", new Vector3(90.02f, initMouseY, 127.31f)));

        Counter5.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(96.87f, 0.0f, 131.28f)));
        Counter5.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(72.3f, 0.0f, 114.29f)));
        Counter5.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(115.61f, 0.0f, 114.29f)));
        Counter5.Add(new KeyValuePair<string, Vector3>("PeralCrocodile", new Vector3(96.87f, 0.0f, 95.51f)));
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        isNoEnemy = false;
        canCount = false;
        SetDictionary();
        GameStageTimer.Initialize();
        GameStageTimer.StartTime = Time.time;
        StartCoroutine("CreaterEnemy1");
        StartCoroutine("CreaterEnemy2");
        StartCoroutine("CreaterEnemy3");
        StartCoroutine("CreaterEnemy4");
        StartCoroutine("CreaterEnemy5");
        StartCoroutine("CreaterEnemy6");
        count = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCount)
        {
            totalEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        if (isNoEnemy)
        {
            isNoEnemy = false;
            count++;
        }

        if (totalEnemyCount == 0)
        {
            isNoEnemy = true;
            canCount = false;
            totalEnemyCount = 2;
        }
    }

    void CreateEnemy(List<KeyValuePair<string, Vector3>> counter)
    {
        foreach (KeyValuePair<string, Vector3> kv in counter)
        {
            Instantiate(enemyDict[kv.Key], kv.Value, enemyDict[kv.Key].transform.rotation);
        }
        canCount = true;
    }

    IEnumerator CreaterEnemy1()
    {
        yield return new WaitUntil(() => count == 1);
        CreateEnemy(Counter1);
    }

    IEnumerator CreaterEnemy2()
    {
        yield return new WaitUntil(() => count == 2);
        GameStageTimer.Stage1Time = Time.time;
        CreateEnemy(Counter2);
    }

    IEnumerator CreaterEnemy3()
    {
        yield return new WaitUntil(() => count == 3);
        GameStageTimer.Stage2Time = Time.time;
        CreateEnemy(Counter3);
    }

    IEnumerator CreaterEnemy4()
    {
        yield return new WaitUntil(() => count == 4);
        GameStageTimer.Stage3Time = Time.time;
        CreateEnemy(Counter4);
    }

    IEnumerator CreaterEnemy5()
    {
        yield return new WaitUntil(() => count == 5);
        GameStageTimer.Stage4Time = Time.time;
        CreateEnemy(Counter5);
    }

    IEnumerator CreaterEnemy6()
    {
        yield return new WaitUntil(() => count == 6);
        GameStageTimer.Stage5Time = Time.time;
    }
}
