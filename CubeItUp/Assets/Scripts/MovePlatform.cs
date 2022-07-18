using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public Transform pos1, pos2;
    private float direction = 5f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < pos1.position.x)
        {
            direction = 5f;
        }
        if(transform.position.x > pos2.position.x)
        {
            direction = -5f;
        }

        transform.position = new Vector3(transform.position.x + direction * Time.deltaTime, transform.position.y, transform.position.z);
    }


}
