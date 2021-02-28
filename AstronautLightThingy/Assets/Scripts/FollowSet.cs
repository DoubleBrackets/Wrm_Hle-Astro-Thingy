using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowSet : MonoBehaviour
{

    void Start()
    { 
        GetComponent<CinemachineVirtualCamera>().Follow=(MoveScript.instance.gameObject.transform);
    }
}
