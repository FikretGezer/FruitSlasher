using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class First : MonoBehaviour
    {
        public static First Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        public void LoadNewScene(string sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad);
            //
        }
    }
}
