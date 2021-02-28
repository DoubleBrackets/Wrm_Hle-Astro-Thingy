using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ammoData", order = 1)]
public class AmmoData : ScriptableObject
{
    public LayerMask enemyMask;
    public LayerMask terrainMask;
    public LayerMask hitMask;
    public LayerMask playerMask;
}
