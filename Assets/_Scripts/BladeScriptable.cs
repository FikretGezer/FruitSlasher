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
        [Min(0)] public int bladeLevel;
        [Min(0)] public int bladePrice;
        public GameObject bladeObj;
    }
}
