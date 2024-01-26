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

            if(trailEffectSecond != null && Time.timeScale > 0.1f)
            {
                // // PC INPUTS
                trailEffectSecond.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
                if(Input.GetMouseButtonDown(0))
                    trailEffectSecond.SetActive(true);
                if(Input.GetMouseButtonUp(0))
                    trailEffectSecond.SetActive(false);

                // MOBILE INPUTS
                // if(Input.touchCount > 0)
                // {
                //     trailEffectSecond.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                //     if(Input.GetTouch(0).phase == TouchPhase.Began)
                //         trailEffectSecond.SetActive(true);
                //     if(Input.GetTouch(0).phase == TouchPhase.Ended)
                //         trailEffectSecond.SetActive(false);
                // }
            }
            else
            {
                if(BladesAndDojos.Instance._selectedBlade != null)
                {
                    if(trailEffectSecond == null)
                    {
                        BladesAndDojos.Instance.SelectABlade();
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
