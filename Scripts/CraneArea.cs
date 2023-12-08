using System;
using _Project.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class CraneArea : MonoBehaviour
{
    public CraneStats craneStats;
    [SerializeField] private Button exitAreaButton;
    [SerializeField] private Button changeButton;

    public Action OnClickedExitArea;
    public CraneStats Stats => craneStats;

    private void OnEnable()
    {
        exitAreaButton.onClick.AddListener(OnClickExitArea);
    }

    private void OnDisable()
    {
        exitAreaButton.onClick.RemoveListener(OnClickExitArea);
    }

    public void Activate(bool status)
    {
        gameObject.SetActive(status);
    }

    public void ActivateButtons()
    {
        // exitAreaButton.gameObject.SetActive(true);
        // changeButton.gameObject.SetActive(true);
    }
    
    private void OnClickExitArea()
    {
        OnClickedExitArea?.Invoke();
        UIManager.Instance.ActivateBlackScreen(.6f, .6f, .5f, () => Activate(false), null, () => { GameEventPool.OnCraneAreaOnBoardCompleted?.Invoke(); });
    }
}