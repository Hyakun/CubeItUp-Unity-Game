using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessPlatforms : MonoBehaviour
{

    private float difficulty;

    private void Start()
    {
        difficulty = EndlessSpawner.dificultyIncrease;
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + -5f * difficulty*Time.deltaTime, transform.position.y, transform.position.z);
        if(transform.position.x < -20)
        {
            Destroy(gameObject);
        }
    }
    
}
