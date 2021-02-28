using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelClampScript : MonoBehaviour
{
    public Transform source;
    private Vector3 offset;
    private int ppu;

    private void Start()
    {
        offset = transform.rotation*transform.localPosition;
        ppu = CameraScript.cameraScript.ppu;
    }


    void Update()
    {
        transform.position = ((Vector2)(source.position + offset)).PixelClamp(ppu);
    }
}
