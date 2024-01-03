using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class UIRaycaster : MonoBehaviour
    {
        [SerializeField] private GameObject _cutEffect;
        [SerializeField] private bool isItOnMenu;

        GraphicRaycaster _Raycaster;
        PointerEventData _PointerEventData;
        EventSystem _EventSystem;

        private bool continueButtonClicked;
        private bool startPlayerButtonClicked;
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
                            _cutEffect.SetActive(true);
                            _cutEffect.transform.position = result.gameObject.transform.position;
                            StartCoroutine(nameof(ContinueCor));
                        }
                        if(!startPlayerButtonClicked && result.gameObject.CompareTag("startButton"))
                        {
                            startPlayerButtonClicked = true;
                            _cutEffect.SetActive(true);
                            _cutEffect.transform.position = result.gameObject.transform.position;
                            StartCoroutine(nameof(ContinueCor));
                        }
                    }
                }
            }
        }
        IEnumerator ContinueCor()
        {
            yield return new WaitForSeconds(0.15f);
            if(!isItOnMenu)
            {
                if(continueButtonClicked)
                    UIUpdater.Instance.LoadSceneAgain();
                else if(startPlayerButtonClicked){
                    AsyncLoader.Instance.LoadSceneAsync("MainScene");
                }

            }
            else{
                AsyncLoader.Instance.LoadSceneAsync("PreGameScene");
            }
        }
    }
}
