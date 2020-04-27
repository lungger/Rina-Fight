using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputTester : MonoBehaviour
{
    [SerializeField]
    Input_Manager input;

    KeyboardConfig keyboardConfig = new KeyboardConfig();

    // Start is called before the first frame update
    void Start()
    {
        //Write();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            GameObject[] totalEnemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in totalEnemy)
            {
                e.GetComponent<GameCharatcer>().currentHp = 0;
            }
        }
    }

    void Read()
    {
        var filePath = Application.streamingAssetsPath + "/keyboardConfig";
        keyboardConfig = FunctionTools.ReadJsonData<KeyboardConfig>(filePath);
        Debug.Log("read");
    }

    void Write()
    {
        var filePath = Application.streamingAssetsPath + "/keyboardConfig";
        FunctionTools.WriteJsonData<KeyboardConfig>(filePath, keyboardConfig);
        Debug.Log("write");
    }
}
