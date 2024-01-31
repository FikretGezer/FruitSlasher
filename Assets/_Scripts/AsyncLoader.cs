using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class AsyncLoader : MonoBehaviour
    {
        [SerializeField] private GameObject otherThings;
        [SerializeField] private GameObject loadingMenu;
        [SerializeField] private Slider _loadingSlider;

        public static AsyncLoader Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }

        public void LoadSceneAsync(string sceneToLoad)
        {
            if(otherThings != null)
                otherThings.SetActive(false);
            if(loadingMenu != null)
                loadingMenu.SetActive(true);

            // Start async loading
            StartCoroutine(LoadAsync(sceneToLoad));
        }
        public void LoadAScene(string sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        IEnumerator LoadAsync(string sceneToLoad)
        {

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);

            while(!loadOperation.isDone)
            {
                float loadingProgressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                _loadingSlider.value = loadingProgressValue;
                yield return null;
            }
        }
    }
}
