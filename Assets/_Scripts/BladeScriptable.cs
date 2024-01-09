using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "New Blade", menuName = "Blades And Dojos/New Blade")]
    public class BladeScriptable : ScriptableObject
    {
        public Sprite bladeSprite;
        public string bladeName;
        [TextArea(1,5)]
        public string bladeExplanation;
        public int bladeLevel;
        public GameObject bladeObj;
    }
}
