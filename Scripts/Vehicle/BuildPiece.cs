using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
public class BuildPiece : MonoBehaviour
{
    protected Rigidbody rb;
     protected bool isFall;
     protected Collider _collider;
     [HideInInspector] public BuildHealth buildHealth;
     public CraneStats _craneStats;
    
     private void Awake()
    {
        _collider = GetComponent<MeshCollider>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // All Piece Fall 
    public void FallPiece(bool isParent)
    {
        rb.isKinematic = false;
        isFall = true;
    }
    
    //Normal Fall
    public void FallPiece(Vector3 ball,float speed,bool control)
    {
        if (control)
        {
            buildHealth.HealthControl(ball);
        }
        rb.isKinematic = false;
        Vector3 direct = (ball-transform.position).normalized;
        rb.AddForce(-direct*speed,ForceMode.Impulse);
        isFall = true;
    }
    public virtual void PullPiece(Transform target)
    {
        if (!isFall) return;
        _collider.isTrigger = true;
        rb.isKinematic = true;
        transform.SetParent(target);
        transform.DOLocalMove(Vector3.zero, 0.25f).OnComplete(() =>
        {
            _craneStats.Increase();
            gameObject.SetActive(false);
        });
    }
    
}