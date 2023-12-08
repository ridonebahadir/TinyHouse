using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TinyHouseCar))]
public class TinyHouseCarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI requirementText;
    [SerializeField] private Image tickImage;
    
    private TinyHouseCar _car;

    private void Awake()
    {
        _car = GetComponent<TinyHouseCar>();
    }

    private void OnEnable()
    {
        _car.Requirements.OnCurrentChanged += SetTinyHouseCarUI;
    }

    private void Start()
    {
        tickImage.enabled = false;
        SetTinyHouseCarUI(_car.Requirements.Current, _car.Requirements.total);
    }

    private void SetTinyHouseCarUI(int arg1, int arg2)
    {
        requirementText.text = $"{arg1}/{arg2}";

        if (arg1 >= arg2)
        {
            requirementText.text = "";
            tickImage.enabled = true;
        }
    }

    private void OnDisable()
    {
        _car.Requirements.OnCurrentChanged -= SetTinyHouseCarUI;
    }
}