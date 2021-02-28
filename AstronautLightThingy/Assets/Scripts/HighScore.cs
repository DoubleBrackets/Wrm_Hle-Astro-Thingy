using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HighScore : MonoBehaviour
{
    public static HighScore instance;
    public GameObject gamemanager;
    public GameManagerScript gamemanagerscript;
    TMP_Text highScore;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gamemanagerscript = GameManagerScript.instance;
        highScore = GetComponent<TMP_Text>();
        if (!PlayerPrefs.HasKey("highScore")) //if first time playing, create new high score aka 0
        {
            PlayerPrefs.SetInt("highScore", gamemanagerscript.portalsHopped);
        }
        UpdateHighScore();
    }

    // Update is called once per frame
    public void UpdateHighScore()
    {
        if (gamemanagerscript.portalsHopped > PlayerPrefs.GetInt("highScore"))
        {
            highScore.text = "Wrm_Hle|Max|Record:" + gamemanagerscript.portalsHopped; //set new high score if current score greater than high score
            PlayerPrefs.SetInt("highScore", gamemanagerscript.portalsHopped);
        } 
        else
        {
            highScore.text = "Wrm_Hle|Max|Record:" + PlayerPrefs.GetInt("highScore"); //otherwise same

        }
    }
}
