using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameObject endPortal;
    private float radius = 1f;
    private GameObject player;
    private SpriteRenderer ren;
    private void Start()
    {
        endPortal = ProcGenTileScript.instance.portal;
        player = MoveScript.instance.gameObject;
        ren = GetComponent<SpriteRenderer>();
    }

    bool en = true;

    // Update is called once per frame
    void Update()
    {
        Vector2 targetdir = endPortal.transform.position - player.transform.position;
        float angle = targetdir.Angle();
        if (targetdir.sqrMagnitude <= 2)
        {
            if(en)
                ren.enabled = false;
            en = false;
        }
        else if (!en)
        {
            ren.enabled = true;
            en = true;
        }
        transform.rotation = Quaternion.Euler(0, 0, angle-90f);
        transform.position = (Vector2)player.transform.position + angle.UnitVector() * radius;
    }
}
