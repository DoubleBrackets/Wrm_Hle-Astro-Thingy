using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public int portalsHopped;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }


    public void LevelPassed()
    {
        portalsHopped++;
        HighScore.instance.UpdateHighScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        portalsHopped = 0;
        StartCoroutine(GameOverCorout());
    }

    IEnumerator GameOverCorout()
    {
        yield return new WaitForSecondsRealtime(2f);
        MoveScript.instance.ResetPlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            GameOver();
    }

}
