using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildHealth : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected List<BuildPiece> buildsPieces;
    public CraneStats _craneStats;
    public string buildId;

    public int fallPiece;
    public int percentDestroy;
    protected bool die;

    public void Init(CraneStats craneStat)
    {
        _craneStats = craneStat;
        foreach (var item in buildsPieces)
        {
            item._craneStats = craneStat;
        }
    }
    private void Start()
    {
        percentDestroy =health - ((health * 60) / 100);
        
        fallPiece=ES3.Load($"{buildId}", 0);
        
        health -= fallPiece;
        
        if (health<percentDestroy)
        {
            gameObject.SetActive(false);
            return;
        }
        for (int i = buildsPieces.Count- 1; i > buildsPieces.Count-1-fallPiece; i--)
        {
            buildsPieces[i].FallPiece(Vector3.zero,0,false);
        }
        
    }

    [Button]
    public void SetPiece()
    {
        if (buildsPieces.Count > 0)
        {
            buildsPieces.Clear();
            health = 0;
        }
        
        foreach (var item in GetComponentsInChildren<BuildPiece>(true))
        {
            buildsPieces.Add(item);
            item.buildHealth = this;
            item._craneStats = _craneStats;
        }
        health = buildsPieces.Count;
    }
    
    public virtual void HealthControl(Vector3 target)
    {
        if (die) return;
        health--;
        fallPiece++;
        
        if (health % 50 == 0)  ResourceManager.Instance.SpawnDustParticle(target).GetComponent<ParticleSystem>().Play(true);
        if (health > percentDestroy) return;
        die = true;
        
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

        foreach (var item in buildsPieces)
        {
            item.FallPiece(true);
        }
        
        fallPiece = buildsPieces.Count;

    }

    private void OnDisable()
    {
        SaveBuild();
    }

    private void OnApplicationQuit()
    {
        SaveBuild();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        SaveBuild();
    }

    private void SaveBuild()
    {
        ES3.Save($"{buildId}", fallPiece);
    }
}