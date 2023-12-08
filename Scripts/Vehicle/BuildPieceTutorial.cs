using DG.Tweening;
using UnityEngine;

public class BuildPieceTutorial : BuildPiece
{
    [SerializeField] private CanExitSO canExitSo;

    public override void PullPiece(Transform target)
    {
        if (!isFall) return;
        _collider.isTrigger = true;
        rb.isKinematic = true;
        transform.SetParent(target);
        transform.DOLocalMove(Vector3.zero, 0.25f).OnComplete(() =>
        {
            _craneStats.CollectedPieces++;
            if (_craneStats.CollectedPieces > 10)
            {
                
                    TutorialManager.Instance.SetTutorialPanel(canExitSo, new[]
                    {
                        "Awesome! You have collected enough piece!",
                        "Now Exit Button will appear. Touch it and leave the area."
                    }, null, null, false);
            }

            gameObject.SetActive(false);
        });
    }
}