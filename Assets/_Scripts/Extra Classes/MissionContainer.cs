using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Runtime
{
    [System.Serializable]
    public class MissionContainer
    {
        public Image image;
        public TMP_Text explanationT;
        public TMP_Text pointsT;
        public TMP_Text progressT;
        public GameObject completedLine;
    }
}
