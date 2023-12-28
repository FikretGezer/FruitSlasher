using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class First : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
