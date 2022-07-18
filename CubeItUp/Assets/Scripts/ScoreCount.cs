using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    private float score;
    public static bool started;
    public static bool isAlive;
    private void Start()
    {
        score = 0;
        started = false;
        isAlive = true;
    }


    void Update()
    {
        if (started && isAlive)
        {
            scoreText.text = ((int)score).ToString();
            score += EndlessSpawner.dificultyIncrease * Time.deltaTime;
        }
    }
}
