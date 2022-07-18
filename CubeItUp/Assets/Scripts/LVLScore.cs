using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LVLScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text lvlScore;

    // Update is called once per frame
    void Update()
    {
        lvlScore.text = AudioManager.score.ToString();
    }
}
