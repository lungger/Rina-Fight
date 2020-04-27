using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class FunctionTools
{
    /// <summary>
    /// 取得按下的按鍵
    /// </summary>
    /// <returns>KeyCode</returns>
    static public KeyCode GetKeyboardKeyDown()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    return kc;
                }
            }
        }
        return KeyCode.None;
    }
    
    /// <summary>
    /// 寫下Json檔案
    /// </summary>
    /// <param name="savePath">檔案儲存路徑 (不需加入.json)</param>
    /// <param name="json">要寫下的Json物件</param>
    static public void WriteJsonData<T>(string savePath, T json)
    {
        string saveData = JsonConvert.SerializeObject(json);
        //Debug.Log("saveData: " + saveData);
        StreamWriter sw = new StreamWriter(savePath + ".json");
        sw.Write(saveData);
        sw.Close();
        //Debug.Log("write json!");
    }

    /// <summary>
    /// 讀取Json檔案 (回傳讀取後的物件)
    /// </summary>
    /// <param name="readPath">檔案讀取路徑 (不需加入.json)</param>
    /// /// <returns>Json object</returns>
    static public T ReadJsonData<T>(string readPath)
    {
        string readSetting = "";
        try
        {
            StreamReader sr = new StreamReader(readPath + ".json");
            readSetting = sr.ReadToEnd();
            //Debug.Log(readSetting);
            sr.Close();
        }
        catch (System.Exception e)
        {
            FunctionTools.WriteJsonData<string>(Application.streamingAssetsPath + "/errorCode", e.Message);
        }

        return JsonConvert.DeserializeObject<T>(readSetting);
    }
}
