using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class TutorialPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform chatPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private float letterInverval = .1f;
    [SerializeField] private float textsInterval = 1f;
    [SerializeField] private float waitTimeBeforeStart = 1f;


    private WaitForSeconds currentLetterIntervalWfs;
    private WaitForSeconds currentTextIntervalWfs;
    private readonly WaitForSeconds zeroTimeWfs = new WaitForSeconds(0f);
    private WaitForSeconds letterIntervalWfs;
    private WaitForSeconds textsIntervalWfs;

    private WaitForSeconds waitTimeBeforeWfs;

    private CanvasGroup _group;

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        letterIntervalWfs = new WaitForSeconds(letterInverval);
        textsIntervalWfs = new WaitForSeconds(textsInterval);
        waitTimeBeforeWfs = new WaitForSeconds(waitTimeBeforeStart);
    }

    public void Activate()
    {
        _group.alpha = 1f;
        _group.interactable = true;
        _group.blocksRaycasts = true;
    }

    public void Deactivate()
    {
        _group.alpha = 0f;
        _group.interactable = false;
        _group.blocksRaycasts = false;
    }

    private bool _isCurrentlyTyping;

    public void StartTutorialText(string[] texts, Action OnFinish = null, Action OnStart = null, bool shouldStartPlayerMove = true)
    {
        currentLetterIntervalWfs = letterIntervalWfs;
        
        // Activate pane
        Activate();
        
        AreaManager.Instance.PlayerWorker.Stop(true);

        OnStart?.Invoke();
        
        // Set default text
        tutorialText.text = "";
        string tempText = "";

        chatPanel.anchoredPosition = new Vector2(chatPanel.anchoredPosition.x, -350f);

        StartCoroutine(IE_StartText());

        IEnumerator IE_StartText()
        {
            chatPanel.DOAnchorPosY(312f, waitTimeBeforeStart - .2f);
            yield return waitTimeBeforeWfs;
            
            foreach (string text in texts)
            {
                _isCurrentlyTyping = true;
                
                foreach (var c in text)
                {
                    tempText += c;
                    tutorialText.text = tempText;
                    yield return currentLetterIntervalWfs;
                }
                
                _isCurrentlyTyping = false;
                
                // This is where a sentence is finished
                currentLetterIntervalWfs = letterIntervalWfs;
                
                yield return currentTextIntervalWfs;

                currentTextIntervalWfs = textsIntervalWfs;
                
                // Reset texts.
                tutorialText.text = "";
                tempText = "";
            }

            if (shouldStartPlayerMove) AreaManager.Instance.PlayerWorker.Stop(false);
            Deactivate();
            OnFinish?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isCurrentlyTyping)
        {
            currentLetterIntervalWfs = zeroTimeWfs;
            currentTextIntervalWfs = zeroTimeWfs;
        }
    }
}