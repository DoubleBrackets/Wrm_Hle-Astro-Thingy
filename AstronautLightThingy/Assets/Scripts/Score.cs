using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public GameObject gamemanager;
    public GameManagerScript gamemanagerscript;

    TMP_Text score;

    void Start()
    {
        gamemanagerscript = GameManagerScript.instance;
        score = GetComponent<TMP_Text>();
        UpdateScore();
    }

    // Update is called once per frame
    void UpdateScore()
    {
        score.text = "Wrm_Hles|Hopped:" + gamemanagerscript.portalsHopped;
    }
}
