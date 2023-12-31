using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class PregameMenu : MonoBehaviour
    {
        [SerializeField] private Transform _startButton;

        // private void Awake() {
        //     _startButton.GetChild(0).GetComponent<Animator>().SetTrigger("rotateF");
        //     _startButton.GetChild(1).GetComponent<Animator>().SetTrigger("rotateC");
        // }
        private void OnEnable() {
            _startButton.GetChild(0).GetComponent<Animator>().SetTrigger("rotateF");
            _startButton.GetChild(1).GetComponent<Animator>().SetTrigger("rotateC");
        }
    }
}
