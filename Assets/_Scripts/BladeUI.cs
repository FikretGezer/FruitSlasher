using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class BladeUI : MonoBehaviour
    {
        private Camera cam;
        [SerializeField] private GameObject trailEffect;
        private void Awake() {
            cam = Camera.main;
        }
        private void Update() {
            RenderTrailEffect();
        }
        private void RenderTrailEffect()
        {
            if(trailEffect != null && Time.timeScale > 0.1f)
            {

                trailEffect.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
                if(Input.GetMouseButtonDown(0))
                    trailEffect.SetActive(true);
                if(Input.GetMouseButtonUp(0))
                    trailEffect.SetActive(false);
            }
            else
                trailEffect.SetActive(false);
        }
    }
}
