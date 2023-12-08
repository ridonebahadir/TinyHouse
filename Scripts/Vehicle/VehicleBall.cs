using System;
using UnityEngine;
public class VehicleBall : MonoBehaviour
{
    [SerializeField] private Rigidbody ballRigidbody;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BuildPiece buildPiece))
        {
            buildPiece.FallPiece(transform.position,8,true);
           
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.TryGetComponent(out BuildPiece buildPiece))
    //     {
    //         buildPiece.FallPiece(transform.position,8);
    //     }
    // }
}