using UnityEngine;
using System.Collections;

public class UnityEnginePlusTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Omector3 gravity;
        Omector3 move;
        gravity = new Omector3(9.81f, 0, -90);
        move = new Omector3(20, 60, 0);
        Debug.Log("重力" + gravity.ToString());
        Debug.Log("移動" + move.ToString());
        Debug.Log("真實移動" + FunctionDriver.GetRealOmector(move, gravity).ToString());
        Debug.Log("真實Vector" + (Vector3)(FunctionDriver.GetRealOmector(move, gravity)));
        Debug.Log("嘗試改變重力");
        gravity = new Omector3(9.81f, 0, -60);
        Debug.Log("重力" + gravity.ToString());
        Debug.Log("移動" + move.ToString());
        Debug.Log("真實移動" + FunctionDriver.GetRealOmector(move, gravity).ToString());
        Debug.Log("真實Vector" + (Vector3)(FunctionDriver.GetRealOmector(move, gravity)));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
