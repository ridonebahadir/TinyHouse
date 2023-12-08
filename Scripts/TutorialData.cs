using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(menuName = "Data/TutorialData")]
    public class TutorialData : ScriptableObject
    {
        public bool ShownTutorial
        {
            get => ES3.Load(name + nameof(ShownTutorial), false);
            set => ES3.Save(name + nameof(ShownTutorial), value);
        }
        
    }
}