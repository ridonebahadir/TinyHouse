using _Project.Scripts;
using UnityEngine;
public class BuildHealthTutorial : BuildHealth
{
    [SerializeField] private CanPullPieceSO canPull;
    public override void HealthControl(Vector3 target)
    {
        if (die) return;
        health--;
        if (health % 50 == 0)  ResourceManager.Instance.SpawnDustParticle(target).GetComponent<ParticleSystem>().Play(true);
        if (health > percentDestroy) return;
        die = true;
        foreach (var item in buildsPieces)
        {
            item.FallPiece(true);
        }

        canPull.ShownTutorial = true;

    }
}