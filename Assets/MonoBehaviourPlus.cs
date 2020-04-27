using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourPlus : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //0度是右邊
        Omector3 moveOmector = new Omector3(10, 0,0);
        Omector3 gravityOmector = new Omector3(-9.81f,-90,0);
        Vector3 moveVector = (Vector3)FunctionDriver.GetRealOmector(moveOmector, gravityOmector);
        transform.position += (Vector3)gravityOmector * Time.deltaTime;


    }
}
