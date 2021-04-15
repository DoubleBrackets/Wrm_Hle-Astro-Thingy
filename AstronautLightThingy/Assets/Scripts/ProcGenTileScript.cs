using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProcGenTileScript : MonoBehaviour
{
    public static ProcGenTileScript instance;

    public tileset[] tilesets;
    int weightTotal = 0;

    [System.Serializable]

    public struct tileset
    {
        public tile[] tiles;
        public int weightTotal;
    }

    [System.Serializable]
    public struct tile
    {
        public Tile tileobj;
        public int weight;
    }

    public NoiseLayer[] noiseLayers;
    [System.Serializable]

    public struct NoiseLayer
    {
        public float minFreq;
        public float maxFreq;
        public float minCoeff;
        public float maxCoeff;
        public float minThreshholdLower;
        public float maxThreshholdLower;
        public float minThreshholdUpper;
        public float maxThreshholdUpper;
        public bool negative;
        public bool enabled;
        public float activationProbability;

        [HideInInspector]
        public float freq, coeff, threshholdlower, threshholdupper,randomoffset;

        public void Randomize()
        {
            freq = Random.Range(minFreq, maxFreq);
            coeff = Random.Range(minCoeff, maxCoeff);
            threshholdlower = Random.Range(minThreshholdLower, maxThreshholdLower);
            threshholdupper = Random.Range(minThreshholdUpper, maxThreshholdUpper);
            randomoffset = Random.Range(0, 10000f);
            float randomActivated = Random.Range(0, 1f);
            if (activationProbability < randomActivated)
                enabled = false;
            else
                enabled = true;
        }
    }

    public GameObject destroyTileParticles;
    private Tilemap tileMap;

    public int width;
    public int height;

    public GameObject portal;

    private int portalMargin = 35;

    private int minDist = 75;

    public float freq = 0.1f;

    private float tileSize = 0.16f;

    private int seed;

    private List<GameObject> destroyParticles;
    int count = 50;
    int counter = 0;

    private void Awake()
    {
        instance = this;
        tileMap = GetComponent<Tilemap>();


        seed = System.DateTime.Now.Millisecond;

        Random.InitState(seed);
        for (int x = 0; x < tilesets.Length; x++)
        {
            for(int i = 0;i < tilesets[x].tiles.Length;i++)
            {
                tilesets[x].weightTotal += tilesets[x].tiles[i].weight;
            }
        }
        GenerateMap();
        destroyParticles = new List<GameObject>();
        //Creates destroyparticles pool
        for (int x = 0;x < count;x++)
        {
            GameObject g = (Instantiate(destroyTileParticles, Vector2.zero, Quaternion.identity));
            g.SetActive(false);
            destroyParticles.Add(g);           
        }


    }

    private void Start()
    {
        MoveScript.instance.gameObject.transform.position = Vector2.zero;
    }


    void GenerateMap()
    {
        int tileset = Random.Range(0, tilesets.Length);
        int xOffset = width / 2;
        int yOffset = height / 2;
        //randomness
        int layercount = noiseLayers.Length;
        for(int c = 0;c < layercount;c++)
        {
            noiseLayers[c].Randomize();
        }

        for (int x = 0; x < width; x++)
        {
            for(int y = 0;y < height;y++)
            {
                Tile settile;
                bool hasTile = false;
                //base shapes
                for(int c = 0;c<layercount;c++)
                {
                    NoiseLayer layer = noiseLayers[c];
                    if(layer.enabled)
                    {
                        float noiseValue = Mathf.PerlinNoise(layer.randomoffset + x * layer.freq, y * layer.freq);
                        //transformations
                        noiseValue = Mathf.Pow(noiseValue, layer.coeff);

                        if (noiseValue >= layer.threshholdlower && noiseValue <= layer.threshholdupper)
                        {
                            if (layer.negative)
                                hasTile = false;
                            else
                                hasTile = true;
                        }
                    }

                }
                if (hasTile)
                    settile = GetRandomTile(tileset);
                else
                    settile = null;
                tileMap.SetTile(new Vector3Int(x-xOffset, y-yOffset, 0), settile);
            }
        }


        //places portal, makes sure within range
        Vector3Int portalPos = new Vector3Int(Random.Range(-xOffset + portalMargin, xOffset - portalMargin), Random.Range(-yOffset + portalMargin, yOffset - portalMargin), 0);
        if(Mathf.Abs(portalPos.x) <= minDist)
        {
            portalPos.x = minDist *(int)Mathf.Sign( portalPos.x);
        }
        if (Mathf.Abs(portalPos.y) <= minDist)
        {
            portalPos.y = minDist * (int)Mathf.Sign(portalPos.y);
        }
        portal.transform.position = tileMap.CellToWorld(portalPos);
        //clears area around spawn and portal
        for(int x = -4;x <= 4;x++)
        {
            for (int y = -4; y <= 4; y++)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), null);
                tileMap.SetTile(new Vector3Int(x+portalPos.x, y+portalPos.y, 0), null);
            }
        }
    }

    public bool DestroyTile(Vector2 pos)
    {
        Vector3Int tilecoords = tileMap.WorldToCell(pos);
        return DestroyTile(tilecoords);
    }

    public bool DestroyTile(Vector3Int tilecoords)
    {
        if (tileMap.GetTile(tilecoords) == null)
            return false;
        tileMap.SetTile(tilecoords, null);
        GameObject g = destroyParticles[counter];
        g.SetActive(true);
        g.transform.position = TileToPosition(tilecoords);
        counter++;
        if (counter == count)
            counter = 0;
        return true;
    }

    Tile GetRandomTile(int index)
    {
        int weightTotal = tilesets[index].weightTotal;
        int threshhold = Random.Range(0, weightTotal);
        int c = 0;
        tile[] tiles = tilesets[index].tiles;
        foreach(tile t in tiles)
        {
            c += t.weight;
            if (c >= threshhold)
                return t.tileobj;
        }
        return tiles[0].tileobj;
    }


    public List<Vector3Int> TileRayCastAll(Vector2 origin, Vector2 dir, float distance)
    {
        var hits = new List<Vector3Int>();
        float xSize = tileMap.cellSize.x;
        float ySize = tileMap.cellSize.y;

        Vector3Int currentTile = tileMap.WorldToCell(origin);
        if (tileMap.GetTile(currentTile) != null)
            hits.Add(currentTile);

        float theta = dir.Angle() * Mathf.Deg2Rad;
        float sinInverse = Mathf.Abs(1 / Mathf.Sin(theta));
        float cosInverse = Mathf.Abs(1 / Mathf.Cos(theta));

        //Calculating distances

        int xDirSign = (int)Mathf.Sign(dir.x);
        int yDirSign = (int)Mathf.Sign(dir.y);
        int xPosSign = (int)Mathf.Sign(origin.x);
        int yPosSign = (int)Mathf.Sign(origin.y);

        float xDist = cosInverse * Mathf.Abs(origin.x - ((int)(origin.x / xSize) * xSize + xPosSign * 0.5f * (1 + xDirSign * xPosSign) * xSize));
        float yDist = sinInverse * Mathf.Abs(origin.y - ((int)(origin.y / ySize) * ySize + yPosSign * 0.5f * (1 + yDirSign * yPosSign) * ySize));

        float yDistIncrement = sinInverse * ySize;
        float xDistIncrement = cosInverse * xSize;

        for (int x = 0; x <= 10000; x++)
        {

            if (xDist < yDist && xDist <= distance)
            {
                currentTile.x += xDirSign;
                if (tileMap.GetTile(currentTile) != null)
                    hits.Add(currentTile);
                xDist += xDistIncrement;
            }
            else if (xDist >= yDist && yDist <= distance)
            {
                currentTile.y += yDirSign;
                if (tileMap.GetTile(currentTile) != null)
                    hits.Add(currentTile);
                yDist += yDistIncrement;
            }
            else
                break;
        }

        return hits;
    }
    public Vector2 TileToPosition(Vector3Int p)
    {
        return tileMap.CellToWorld(p);
    }

}
