using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class ResourceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txt;
        [SerializeField] private Image resourceImage;
        [SerializeField] private ResourceSo resource;

        private void Start()
        {
            SetResourceImage();
            SetResourceText(resource.Current);
        }

        private void OnEnable()
        {
            resource.OnValueChanged += SetResourceText;
        }

        private void OnDisable()
        {
            resource.OnValueChanged -= SetResourceText;
        }

        private void SetResourceText(int value)
        {
            txt.text = value.ToCurrency();
        }

        private void SetResourceImage()
        {
            resourceImage.sprite = resource.ResourceSprite;
        }
    }
}