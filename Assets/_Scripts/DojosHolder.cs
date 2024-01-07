using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Dojos Holder", menuName = "Blades And Dojos/Dojos Holder")]
    public class DojosHolder : ScriptableObject
    {
        public DojoScriptable[] dojos;
    }
}
