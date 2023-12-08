using System;
using _Project.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Crane : MonoBehaviour
{
    [SerializeField] private GameObject maxText;
    [SerializeField] private GameObject leftRightMoveObj;
    [SerializeField] private GameObject forwardBackMoveObj;
    [SerializeField] private Transform pullObj;
    [SerializeField] private Transform ballObj;
    [SerializeField] private CraneArea craneArea;
    
    private CraneStats craneStats;
    private float maxCraneMoveZ;
    private bool scale = true;
    
    
    [Title("Tutorial")] 
    public CanPullPieceSO canPullPieceSo;
    public CanExitSO canExitSo; 
    public CraneTutorialCompletedSO craneTutorialCompletedSo;

    
    [Title("Save")]
    private float armScaleZ;
    private float cranePosZ;
    [SerializeField] private string id;
    

    private Vector3 initialPullObjScale;
    private Vector3 initialBallScale;
    public BuildSpawner buildSpawner;
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        if (maxText!=null) maxText.gameObject.SetActive(false);
        SetCranePos();
    }

    private void Init()
    {
        craneStats = craneArea.craneStats;
        initialPullObjScale = pullObj.transform.localScale;
        initialBallScale = ballObj.transform.localScale;
    }
    private void OnEnable()
    {
        SetUpgradeValues();
    }

    private void OnDisable()
    {
        if(maxText!=null) SetSave();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        SetSave();
    }

    private void OnApplicationQuit()
    {
        SetSave();
    }

  

    private void SetCranePos()
    {
        armScaleZ = ES3.Load("armScaleZ"+id, 0.03f);
        cranePosZ = ES3.Load("cranePosZ"+id, -20f);
        var forwardObj = forwardBackMoveObj.transform.localScale;
        forwardBackMoveObj.transform.DOScale(new Vector3(forwardObj.x, forwardObj.y, armScaleZ), 3f);
        if (buildSpawner==null) return;
        var offset = 30;
        var count=cranePosZ/offset;
        Collider col = null;
        for (int i = 0; i < count+2; i++)
        {
          col=buildSpawner.SpawnBuild(new Vector3(transform.position.x,transform.position.y,(i)*offset),false);  
        }
        col.enabled = true;
        //transform.DOMoveZ(cranePosZ, 3f);
        transform.position = new Vector3(transform.position.x, transform.position.y, cranePosZ);
    }
    private void SetSave()
    {
        ES3.Save("armScaleZ"+id,forwardBackMoveObj.transform.localScale.z);
        ES3.Save("cranePosZ"+id,transform.position.z);
        
    }
    private void SetUpgradeValues()
    {
       
        SetArmLength();
        SetVacuumScale();
        SetBallScale();
    }

    private void SetVacuumScale()
    {
        pullObj.transform.localScale = new Vector3(initialPullObjScale.x + craneStats.CurrentVacuumScale, initialPullObjScale.y +craneStats.CurrentVacuumScale, initialPullObjScale.z + craneStats.CurrentVacuumScale);
    }
    
    private void SetBallScale()
    {
        ballObj.transform.localScale = new Vector3(initialBallScale.x + craneStats.CurrentBallScale, initialBallScale.y + craneStats.CurrentBallScale, initialBallScale.z + craneStats.CurrentBallScale);
    }

    private void SetArmLength()
    {
        maxCraneMoveZ = craneStats.CurrentArmLength;
    }

    private void FixedUpdate()
    {
        ForwardBackMove();
        LeftRightMove();

    }

    // Its called Horizontal my brother Rıdvan!

    private void LeftRightMove()
    {
        var target = MobileJoystick.Instance.GetJoystickVector(0).x * 25f;
        leftRightMoveObj.transform.rotation = Quaternion.RotateTowards(leftRightMoveObj.transform.rotation,
            Quaternion.Euler(new Vector3(0, target, 0)), 15f * Time.deltaTime);
    }


   // Its called Vertical my brother Rıdvan!
    private void ForwardBackMove()
    {
        var move = MobileJoystick.Instance.GetJoystickVector(0).z;
        var localScale = forwardBackMoveObj.transform.localScale;
        if (scale)
        {
            if (localScale.z>=0.07f)
            {
                scale = false;
                return;
            }

            if (move == 0) return;
            float newScale = Mathf.MoveTowards(localScale.z, move, 0.015f * Time.deltaTime);
            newScale = Mathf.Clamp(newScale, 0.03f,0.08f);
            forwardBackMoveObj.transform.localScale = new Vector3(forwardBackMoveObj.transform.localScale.x,forwardBackMoveObj.transform.localScale.y, newScale);



        }
        else
        {
            var craneObjPos = transform.position;
            float xBondrey = Mathf.Clamp(value: craneObjPos.z+move, min:-21, max: maxCraneMoveZ);
            transform.position = Vector3.Lerp(craneObjPos, new Vector3(craneObjPos.x,craneObjPos.y ,xBondrey ), 3 * Time.deltaTime);
            if (xBondrey<-20)
            {
                forwardBackMoveObj.transform.localScale -= new Vector3(0,0,0.02f*Time.deltaTime);
                scale = true;
            }
            if(xBondrey==maxCraneMoveZ && maxText!=null)
            {
                maxText.gameObject.SetActive(true);
            }
            else
            {
                if (maxText!=null)  maxText.gameObject.SetActive(false);
            }
        }
       
    }
    
}