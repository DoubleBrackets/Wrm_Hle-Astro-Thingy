using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    public GameObject enemyBaseObject;

    public GameObject[] subComponents;

    public float spawnCooldownMin;
    public float spawnCooldownMax;

    private float spawnTimer = 0;

    public int maxCluster;

    private void Awake()
    {
        //SpawnEnemy(Vector2.right * 2f);
    }
    private Vector2 prevPos = Vector2.zero;
    private float distanceCounter = 0;
    public float distanceThreshhold = 8f;
    private void Update()
    {
        Vector2 target = MoveScript.instance.transform.position;
        distanceCounter += (prevPos - target).magnitude;
        prevPos = target;
        if (spawnTimer >= 0)
        {
            spawnTimer -= Time.deltaTime;
            return;
        }
        if (distanceCounter <= distanceThreshhold)
            return;
        distanceCounter = 0f;

        Vector2 sourcePos = ((Vector2)ProcGenTileScript.instance.portal.transform.position - target).normalized * 5f + target;

        SpawnEnemyGroup(sourcePos);
    }

    void SpawnEnemyGroup(Vector2 sourcePos)
    {
        spawnTimer = Random.Range(spawnCooldownMin, spawnCooldownMax);
        int count = Random.Range(1, maxCluster + 1);
        for(int x = 0;x < count;x++)
        {
            Vector2 offset = Random.Range(0, 360f).UnitVector();
            SpawnEnemy(sourcePos + offset);
        }
    }

    void SpawnEnemy(Vector2 position)
    {
        GameObject newEnemy = Instantiate(enemyBaseObject, position, Quaternion.identity);
        int index1 = Random.Range(0, 2);
        GameObject comp1 = Instantiate(subComponents[index1], newEnemy.transform);
        int index2 = Random.Range(0, 2);
        if(index2 != index1)
            Instantiate(subComponents[index2], newEnemy.transform);
    }
}
