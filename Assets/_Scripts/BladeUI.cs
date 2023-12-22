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
            if(trailEffect != null)
            {
                trailEffect.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
                if(Input.GetMouseButton(0))
                {
                    trailEffect.SetActive(true);
                }
                if(Input.GetMouseButtonUp(0))
                    trailEffect.SetActive(false);
            }
        }
    }
}
