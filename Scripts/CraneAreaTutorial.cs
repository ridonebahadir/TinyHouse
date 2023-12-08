using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;

public class CraneAreaTutorial : MonoBehaviour
{
    [SerializeField] private TutorialData craneTutorialData;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        
        TutorialManager.Instance.SetTutorialPanel(craneTutorialData, new []
        {
            "Welcome! I am O-ralet! Your helper.",
            "You are now in demolish area where you can destroy buildings.",
            "You can move the crane and demolish buildings by moving your finger on screen.",
            "By releasing, you can switch to vacuum to collect pieces you demolished!",
            "Now, move your finger on the screen and hit to the building!"
        }, () => {}, () => {}, false);
    }

}
