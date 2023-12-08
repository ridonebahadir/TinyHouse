using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class AreaManager : SingletonClass.Singleton<AreaManager>
{
    [Title("Area")] [SerializeField] private AreaController[] unlockableAreas;
    [SerializeField] private AreaController defaultArea;


    [Title("Player Reference")] [SerializeField]
    private PlayerWorker playerWorker;

    [Title("Clamp Values")] [SerializeField]
    private float minxClamp = 7f;

    [SerializeField] private float minZClamp = -13f;
    [SerializeField] private float maxZClamp = 21f;

    public int LastUnlockedAreaIndex
    {
        get => ES3.Load("Unlock_Area_Index", -1);
        set => ES3.Save("Unlock_Area_Index", value);
    }

    public PlayerWorker PlayerWorker => playerWorker;

    public AreaController DefaultArea => defaultArea;

    public void SetPlayerClampValues(float xValue)
    {
        PlayerWorker.SetPlayerClampValues(minxClamp, xValue, minZClamp, maxZClamp);
    }

    public void UnlockNextArea(int idx)
    {
        UnlockArea(idx);
        LastUnlockedAreaIndex = idx;
    }

    private void UnlockArea(int idx)
    {
        var unlockableArea = unlockableAreas[idx];
        unlockableArea.UnlockArea();
    }
    
}