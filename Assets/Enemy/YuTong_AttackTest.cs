using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuTong_AttackTest : MonoBehaviour
{
    [Header("View，須在外部設定")]
    [SerializeField]
    GameObject canvas;

    public BulletScreen_Data bulletData;

    //攻擊 - 創造一個彈幕
    void Attack()
    {
        BulletScreen newBullet = Instantiate(bulletData.BulletScreen, canvas.transform).GetComponent<BulletScreen>();
        newBullet.SetText(bulletData.Text_Database);
        newBullet.SetFontSize(bulletData.MinFontSize, bulletData.MaxFontSize);
        newBullet.SetColor(bulletData.FontColor_Database);
        newBullet.SetSpeed(bulletData.MinSpeed, bulletData.MaxFontSize);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (bulletData == null)
            bulletData = gameObject.GetComponent<BulletScreen_Data>();

        InvokeRepeating("Attack", 0.5f, 1.21f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}