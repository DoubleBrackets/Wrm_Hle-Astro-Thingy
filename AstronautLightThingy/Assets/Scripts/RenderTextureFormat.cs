using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureFormat : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Camera>().targetTexture.format = UnityEngine.RenderTextureFormat.ARGB32;
    }

}
