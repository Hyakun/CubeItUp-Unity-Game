using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private GameObject spear;
    [SerializeField]
    private GameObject shuriken;



    private float timeHolder;
    private float secondTimeHolder;

    private void Start()
    {
        timeHolder = Time.time+3;
        secondTimeHolder = Time.time + 3.25f;
    }

    void Update()
    {
        if (Boss.stage == 1)
        {
            if (Time.time >= timeHolder)
            {
                timeHolder += 3f;
                secondTimeHolder += 3f;
                shoot();
            }
        }
        if (Boss.stage == 2)
        {
            if (Time.time >= timeHolder)
            {
                timeHolder += 3f;
                shoot2();
            }
            if (Time.time >= secondTimeHolder)
            {
                secondTimeHolder += 3f;
                shoot2();
            }
        }
    }


    void shoot()
    {
        Instantiate(spear, firePoint.position, spear.transform.rotation);

    }
    void shoot2()
    {
        Instantiate(shuriken, firePoint.position, shuriken.transform.rotation);
    }
}
