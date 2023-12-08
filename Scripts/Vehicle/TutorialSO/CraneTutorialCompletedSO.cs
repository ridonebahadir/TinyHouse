using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CraneTutorialCompletedSO", menuName = "CraneTutorial/CraneTutorialCompletedSO", order = 1)]
public class CraneTutorialCompletedSO : ScriptableObject
{
    public bool craneTutorialCompleted
    {
        get => ES3.Load($"{name}_craneTutComplete", false);
        set => ES3.Save($"{name}_craneTutComplete", value);
    }
}