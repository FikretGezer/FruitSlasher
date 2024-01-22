using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Selected Missions", menuName = "Missions/Selected Missions")]
    public class SelectedMissionsScriptable : ScriptableObject
    {
        public List<Mission> missions = new List<Mission>();
        public List<Mission> selectedMissions = new List<Mission>();
    }
}
