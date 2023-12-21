using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Runtime
{
    public class UIRaycaster : MonoBehaviour
    {
        GraphicRaycaster _Raycaster;
        PointerEventData _PointerEventData;
        EventSystem _EventSystem;

        private bool continueButtonClicked;
        private Vector3 startPos, endPos;
        void Awake()
        {
            _Raycaster = GetComponent<GraphicRaycaster>();
            _EventSystem = GetComponent<EventSystem>();
            _PointerEventData = new PointerEventData(_EventSystem);
        }
        private void OnEnable() {
            continueButtonClicked = false;
        }

        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                endPos = Input.mousePosition;


                _PointerEventData.position = endPos;

                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                _Raycaster.Raycast(_PointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    if(endPos != startPos)
                    {
                        // Detect Continue Button
                        if(!continueButtonClicked && result.gameObject.CompareTag("continueButton")){
                            continueButtonClicked = true;
                            StartCoroutine(nameof(ContinueCor));
                        }
                    }
                }
            }
        }
        IEnumerator ContinueCor()
        {
            yield return new WaitForSeconds(0.15f);
            UIUpdater.Instance.LoadSceneAgain();
        }
    }
}
