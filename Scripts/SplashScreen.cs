using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private Image splashImage;
    [SerializeField] private float fadeOutTime;
    
    public Tween FadeOut()
    {
        return splashImage.DOFade(0f, fadeOutTime).OnComplete(() => gameObject.SetActive(false));
    }
}
