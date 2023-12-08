using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using Cinemachine;
using UnityEngine;

public class TutorialManager : SingletonClass.Singleton<TutorialManager>
{
    [SerializeField] private CraneTutorialCompletedSO completedCraneTutorial;
    [SerializeField] private GameObject craneTutorialArea;
    [SerializeField] private TutorialPanel tutorialPanel;
    [SerializeField] private TutorialLine tutorialLine;
    
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private GameObject fingerObj;



    private WaitForSeconds _seconds;

    private CinemachineBrain _brain;

    private int _priority;

    private void Start()
    {
        fingerObj.SetActive(false);
        tutorialLine.Deactivate();
        Camera cam = Camera.main;
        _brain = cam.GetComponent<CinemachineBrain>();
        _priority = virtualCam.m_Priority;

        if (completedCraneTutorial.craneTutorialCompleted)
        {
            AreaManager.Instance.PlayerWorker.Stop(false);
            craneTutorialArea.SetActive(false);
        }
        else
        {
            UIManager.Instance.ActivateBlackScreen(.2f, .2f, .5f, () =>
            {
                craneTutorialArea.SetActive(true);
                AreaManager.Instance.PlayerWorker.Stop(true);
            });
        }
    }

    public TutorialLine SetTutorialLine(Transform start, Vector3 end)
    {
        return tutorialLine.InitializeLine(start, end);
    }

    public void SetTutorialPanel(TutorialData data, string[] texts, Action OnComplete = null, Action OnStart = null, bool shouldAllowPlayerMove = true)
    {
        if (!data.ShownTutorial)
        {
            tutorialPanel.StartTutorialText(texts, OnComplete, OnStart, shouldAllowPlayerMove);
            data.ShownTutorial = true;
        }
    }

    public void SetCameraTO(Transform point, float blendTime = 2f, float firstDelay = 2f, Action OnComplete = null)
    {
        var defaultBlendTime = _brain.m_DefaultBlend.m_Time;

        _seconds = new WaitForSeconds(firstDelay);

        StartCoroutine(IE_SetCameraTO());

        IEnumerator IE_SetCameraTO()
        {
            AreaManager.Instance.PlayerWorker.Stop(true);

            var pointWithOffset =
                new Vector3(point.position.x, virtualCam.transform.position.y, point.position.z - 10f);

            // Set virtual cam position
            virtualCam.transform.position = pointWithOffset;

            _brain.m_DefaultBlend.m_Time = blendTime;
            // Set priority
            virtualCam.m_Priority = 100;

            yield return new WaitForSeconds(blendTime);

            // Stay time on a place
            yield return new WaitForSeconds(.75f);

            // Set priority back
            virtualCam.m_Priority = _priority;

            // Wait until arrive
            yield return new WaitForSeconds(blendTime);

            // Set default values back
            _brain.m_DefaultBlend.m_Time = defaultBlendTime;

            AreaManager.Instance.PlayerWorker.Stop(false);

            // Wait until blend and set it back to default
            OnComplete?.Invoke();
        }
    }

    public void SetFingerObj(bool status)
    {
        fingerObj.SetActive(status);
    }
}