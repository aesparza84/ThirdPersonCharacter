using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float speed;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        body.velocity = speed * gameObject.transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
