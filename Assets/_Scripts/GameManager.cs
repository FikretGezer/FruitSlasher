using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public enum GameSituation{
        Play,
        Stop
    }
    public class GameManager : MonoBehaviour
    {
        public static GameSituation Situation;
        private void Awake() {
            Situation = GameSituation.Play;
        }
    }
}
