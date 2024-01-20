using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BasicProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float speed;

    [SerializeField] private int gunDamage;
    void Start()
    {
        Destroy(gameObject, 3f);
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
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable obj))
        {
            obj.TakeDamage(gunDamage);
        }
        Destroy(gameObject);
    }

    public void SetDamage(int damage)
    {
        gunDamage = damage;
    }
}
