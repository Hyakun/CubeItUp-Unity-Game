using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] platforms;

    private float spawnCD;
    private float lastSpawn ;
    private Transform prevpos;

    public static int dificultyIncrease;
    private float startTime;


    // Start is called before the first frame update
    void Start()
    {
        spawnCD = 2f;
        lastSpawn = 0;
        dificultyIncrease = 1;
        startTime = Time.time;
        prevpos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (dificultyIncrease < 4)
        {
            difficultyIncrease();
        }
        spawnPlatforms();
    }

    private void spawnPlatforms()
    {
        float randomNumber = Random.Range(0f, 2.99f);

        int platformIndex = (int)Mathf.Floor(randomNumber);
        switch (platformIndex)
        {
            case 0:
                spawnCD = 2f;
                break;
            case 1:
                spawnCD = 3f;
                break;
            case 3:
                spawnCD = 3.5f;
                break;
        }

        if (Time.time >= (lastSpawn + (spawnCD / dificultyIncrease)))
        {
            transform.position = new Vector3(transform.position.x, posyRand(), transform.position.z);
            Instantiate(platforms[platformIndex], transform.position, Quaternion.identity);
            prevpos = transform;
            lastSpawn = Time.time;
        }

    }

    private float posyRand()
    {
        float positionYRand = Random.Range(-3.5f, 3.6f);
        if (prevpos.position.y + 1.7f < positionYRand || prevpos.position.y - 1.7f > positionYRand)
        {
            return posyRand();
        }
        else
        {
            return positionYRand;
        }
    }

    void difficultyIncrease()
    {
        if(Time.time >= startTime+30)
        {
            dificultyIncrease = 4;
        }else if(Time.time >= startTime + 20)
        {
            dificultyIncrease = 3;
        }
        else if (Time.time >= startTime + 10)
        {
            dificultyIncrease = 2;
        }
    }

}
