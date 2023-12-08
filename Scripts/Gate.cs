using System;
using DG.Tweening;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Transform barrier;
    private bool _unlocked;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
    }

    public void Unlock()
    {
        int mask = LayerMask.NameToLayer("Default");
        _unlocked = true;
        gameObject.layer = mask;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_unlocked) return;
        barrier.transform.DOLocalRotate(new Vector3(0, 90, 0),.25f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_unlocked) return;
        barrier.transform.DOLocalRotate(new Vector3(0, 0, 0),.25f);
    }
}