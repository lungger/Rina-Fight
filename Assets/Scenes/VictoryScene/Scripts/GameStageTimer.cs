using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageTimer
{
    //計算每關時間 ()
    // Start is called before the first frame update
    public static float StartTime;
    public static float Stage1Time; //第一關完成時間
    public static float Stage2Time;
    public static float Stage3Time;
    public static float Stage4Time;
    public static float Stage5Time;

    public static void Initialize()
    {
        StartTime = 0f;
        Stage1Time = 0f;
        Stage2Time = 0f;
        Stage3Time = 0f;
        Stage4Time = 0f;
        Stage5Time = 0f;
    }
}
