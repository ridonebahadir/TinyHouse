using System;
using Unity.Mathematics;
using UnityEngine;
public class BuildSpawner : MonoBehaviour
{
    [SerializeField] private GameObject homesPrefab;
    [SerializeField] private Transform firstPosSpawnBuild;

    [SerializeField] private int spawnCount;
    [SerializeField] private CraneArea craneArea;
    
    private void Start()
    {
        //SpawnBuild(firstPosSpawnBuild.position);
    }

    public BoxCollider SpawnBuild(Vector3 startPos,bool close)
    {
        GameObject clone= Instantiate(homesPrefab,startPos,Quaternion.Euler(0,180,0) ,transform);
        BuildComplex buildComplex = clone.GetComponent<BuildComplex>();
        BoxCollider col = buildComplex.GetComponent<BoxCollider>();
        col.enabled = close;
        buildComplex.buildSpawner = this;
        buildComplex.Init(craneArea.craneStats);
        buildComplex.SetID();
        buildComplex.SetStatsArea(craneArea);
        clone.transform.position = startPos;
        spawnCount++;

        return col;
    }
    
    
}