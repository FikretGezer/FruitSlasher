using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "New Dojo", menuName = "Blades And Dojos/New Dojo")]
    public class DojoScriptable : ScriptableObject
    {
        public Sprite dojoSprite;
        public string dojoName;
        [TextArea(1,5)]
        public string dojoExplanation;
        [Min(0)] public int dojoLevel;
        [Min(0)] public int dojoPrice;
    }
}
