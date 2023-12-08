using UnityEngine;
public class VehiclePull : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BuildPiece buildPiece))
        {
            buildPiece.PullPiece(transform);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out BuildPiece buildPiece))
        {
            buildPiece.PullPiece(transform);
        }
    }
}