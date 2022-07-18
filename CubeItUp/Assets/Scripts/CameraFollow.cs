using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private int offset;
    private Vector3 tempPos;
    [SerializeField]
    private float limit1, limit2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void LateUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {

            tempPos = transform.position;
            tempPos.x = player.position.x;
            tempPos.y = player.position.y+offset;
            if (tempPos.x < limit1)
                tempPos.x = limit1;
            if (tempPos.x > limit2)
                tempPos.x = limit2;

            transform.position = tempPos;
        }
    }
}