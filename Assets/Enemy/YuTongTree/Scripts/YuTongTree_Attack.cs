using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuTongTree_Attack : MonoBehaviour
{
    [Header("View，須在外部設定")]
    [SerializeField]
    GameObject canvas;

    public BulletScreen_Data bulletData;
    public int bullentCount = 3;

    //攻擊 - 創造一個彈幕
    public void Attack()
    {
        CreateBullet(bullentCount);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (bulletData == null)
            bulletData = gameObject.GetComponent<BulletScreen_Data>();

        canvas = GameObject.Find("UICanvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateBullet(int bulletCount)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            BulletScreen newBullet = Instantiate(bulletData.BulletScreen, canvas.transform).GetComponent<BulletScreen>();
            newBullet.SetText(bulletData.Text_Database);
            newBullet.SetFontSize(bulletData.MinFontSize, bulletData.MaxFontSize);
            newBullet.SetColor(bulletData.FontColor_Database);
            newBullet.SetSpeed(bulletData.MinSpeed, bulletData.MaxFontSize);
        }
    }
}