using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Blades Holder", menuName = "Blades And Dojos/Blades Holder")]
    public class BladesHolder : ScriptableObject
    {
        public BladeScriptable[] blades;
    }
}
