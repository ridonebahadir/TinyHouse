using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ChatBubble : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI bubbleText;

    private void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public void Deactivate()
    {
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public void SetText(string text)
    {
        bubbleText.text = text;
    }
}
