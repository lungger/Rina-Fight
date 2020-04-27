using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_die : MonoBehaviour
{
    public float minForce = 750.0f;
    public float maxForce = 1500.0f;
    public float radius = 5f;
    public float Destroydelay = 0.1f;

    void Start()
    {
        Explode();
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Explode();
        //    Debug.Log("space");
        //}
    }

    public void Explode()
    {
        //Debug.Log("Explosionnnnnnnnnnnnnn~");
        foreach (Transform _transform in transform)
        {
            Rigidbody rb = _transform.GetComponent<Rigidbody>();

            if (rb != null)
            {
                //Debug.Log(rb.gameObject.name);
                //rb.isKinematic = false;
                rb.AddExplosionForce(Random.Range(minForce, maxForce), _transform.position, radius);
            }
            Destroy(_transform.gameObject, Destroydelay);
        }
    }

}
