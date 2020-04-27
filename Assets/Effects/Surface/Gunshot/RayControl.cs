using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayControl : MonoBehaviour
{
    private LineRenderer ray;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        ray = GetComponent<LineRenderer>();
        ray.startWidth = 2f;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (ray.startWidth > 0f)
        {
            if (Time.time - startTime > 0.01f)
            {
                ray.startWidth -= 0.2f;
                startTime = Time.time;
            }
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }
}
