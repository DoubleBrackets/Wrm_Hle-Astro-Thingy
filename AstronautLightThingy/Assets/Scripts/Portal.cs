using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public bool startPortal;
    private bool triggered = false;

    private void Start()
    {
        if (startPortal)
            StartCoroutine(PortalCloseAnim());
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (startPortal || triggered)
            return;
        if (other.GetComponent<PlayerEntityScript>() != null)
        {
            triggered = true;
            StartCoroutine(PortalOpenAnim());
            Audio.instance.playPortal();
        }
    }

    float val = 60f;

    IEnumerator PortalOpenAnim()
    {
        Time.timeScale = 0f;
        MoveScript.rb.velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().sortingLayerName = "Overlay";
        for (int x = 10; x <= 300; x+=3)
        {
            transform.Rotate(new Vector3(0, 0, 36f * 0.01f));
            transform.localScale = Vector2.one * (x*0.2f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = 1f;
        GameManagerScript.instance.LevelPassed();
    }

    IEnumerator PortalCloseAnim()
    {
        Audio.instance.playPortal();
        Time.timeScale = 0f;
        transform.localScale = Vector2.one * (val);
        yield return new WaitForSecondsRealtime(1f);
        for (int x = 0; x <= 300; x += 3)
        {
            transform.Rotate(new Vector3(0, 0, 36f * 0.1f));
            transform.localScale = Vector2.one * (val-(x * 0.2f));
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

}
