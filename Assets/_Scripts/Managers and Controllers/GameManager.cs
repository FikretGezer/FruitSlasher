using UnityEngine;

namespace Runtime
{
    public class GameManager : MonoBehaviour
    {
        public static GameSituation Situation;
        private void Awake() {
            Situation = GameSituation.Play;
        }
    }
}
