using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class BladeUI : MonoBehaviour
    {
        private Camera cam;
        [SerializeField] private GameObject trailEffect;
        public GameObject trailEffectSecond;
        private BladeScriptable _currentBlade;
        private void Awake() {
            cam = Camera.main;
        }
        private void Update() {
            RenderTrailEffect();
        }
        private void RenderTrailEffect()
        {
            // if(trailEffect != null && Time.timeScale > 0.1f)
            // {
            //     trailEffect.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
            //     if(Input.GetMouseButtonDown(0))
            //         trailEffect.SetActive(true);
            //
            //     if(Input.GetMouseButtonUp(0))
            //         trailEffect.SetActive(false);
            // }
            //
            if(trailEffectSecond != null && Time.timeScale > 0.1f)
            {
                trailEffectSecond.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
                if(Input.GetMouseButtonDown(0))
                    trailEffectSecond.SetActive(true);
                if(Input.GetMouseButtonUp(0))
                    trailEffectSecond.SetActive(false);
            }
            else
            {
                if(BladesAndDojos.Instance._selectedBlade != null)
                {
                    if(trailEffectSecond == null)
                    {
                        _currentBlade = BladesAndDojos.Instance._selectedBlade;
                        Debug.Log(_currentBlade.bladeObj.name);
                        trailEffectSecond = Instantiate(_currentBlade.bladeObj);
                        trailEffectSecond.transform.SetParent(transform);
                        trailEffectSecond.SetActive(false);
                    }
                }
            }
            if(_currentBlade != BladesAndDojos.Instance._selectedBlade)
            {
                Destroy(trailEffectSecond);
                trailEffectSecond = null;
            }
        }
    }
}
