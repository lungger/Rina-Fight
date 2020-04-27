using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScreen_Data : MonoBehaviour
{
    [Header("彈幕本體")]
    public GameObject BulletScreen;

    [Header("彈幕文本資料庫")]
    public List<string> Text_Database;

    [Header("最小字體Size")]
    public int MinFontSize;
    [Header("最大字體Size")]
    public int MaxFontSize;

    [Header("字體顏色庫")]
    public List<Color> FontColor_Database;

    [Header("最小飛行速度")]
    public float MinSpeed;
    [Header("最大飛行速度")]
    public float MaxSpeed;
}
