using System.Collections;
using UnityEngine;
public class ChangeToolTutorial : ChangeTool
{
    protected override void EnableButtons()
    {
        if (crane.canPullPieceSo.ShownTutorial) changeToolButton.gameObject.SetActive(true);
        if (crane.canExitSo.ShownTutorial)   exitAreaButton.gameObject.SetActive(true);
        
    }
    protected override void ExitArea()
    {
        AreaManager.Instance.PlayerWorker.Stop(false);
        crane.craneTutorialCompletedSo.craneTutorialCompleted = true;
    }
}