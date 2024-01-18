using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGun : MonoBehaviour
{
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private Vector3 createPos;
    [SerializeField] private Vector3 createRot;
    [SerializeField] private Vector3 createScal;

    public Vector3 CreatePos { get { return createPos; } }
    public Vector3 CreateRot { get { return createRot; } }
    public Vector3 CreateScal { get { return createScal; } }

    [SerializeField] private float fireRate;
    private float nextFire;

    [SerializeField] private int damage;
    void Start()
    {
        gameObject.transform.localPosition = createPos;
        gameObject.transform.localRotation = Quaternion.Euler(createRot);
        gameObject.transform.localScale = createScal;

    }

    void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        else if (nextFire < 0)
        {
            nextFire = 0;
        }

        Debug.DrawRay(shootPoint.position, shootPoint.forward * 50f);
    }

    public void ShootWeapon()
    {
        if(nextFire == 0)
        {
            BasicProjectile temp = ammoPrefab.GetComponent<BasicProjectile>();
            Instantiate(temp, shootPoint.position, shootPoint.rotation);
            nextFire = fireRate;
        }
    }
}
