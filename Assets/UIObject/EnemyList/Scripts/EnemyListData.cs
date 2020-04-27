using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListData : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    
    public List<Enemy> GetEnemyList()
    {
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemyArray.Length; i++)
        {
            for (int j = 0; j < enemyArray.Length - 1; j++)
            {
                if (GetDistance(enemyArray[j]) > GetDistance(enemyArray[j + 1]))
                {
                    GameObject temp = enemyArray[j + 1];
                    enemyArray[j + 1] = enemyArray[j];
                    enemyArray[j] = temp;
                }
            }
        }

        List<Enemy> enemyList = new List<Enemy>();
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyList.Add(enemyArray[i].GetComponent<Enemy>());
        }
        return enemyList;
    }

    float GetDistance(GameObject enemy)
    {
        Vector3 dir = enemy.transform.position - Player.transform.position;
        return dir.magnitude;
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
