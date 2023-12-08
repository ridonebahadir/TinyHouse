using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class BuildComplex : MonoBehaviour
{
    public Transform spawnPoint;
    [HideInInspector] public BuildSpawner buildSpawner;
    [SerializeField] private List<BuildHealth> buildHealths;
    private CraneStats _craneStats;

    public void Init(CraneStats craneStats)
    {
        _craneStats = craneStats;
    }
    
    private bool isFirstTime = true;
   

    public void SetID()
    {
        for (int i = 0; i < buildHealths.Count; i++)
        {
            var complexId = transform.GetSiblingIndex().ToString();
            buildHealths[i].buildId = _craneStats.id +complexId + i.ToString() + gameObject.name;
        }
       
        
    }

    public void SetStatsArea(CraneArea craneArea)
    {
        foreach (var item in buildHealths)
        {
            item.Init(craneArea.craneStats);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FollowObj followObj))
        {
            if (isFirstTime)
            {
                isFirstTime = false;
                buildSpawner.SpawnBuild(spawnPoint.position,true);
            }
        }
    }
}