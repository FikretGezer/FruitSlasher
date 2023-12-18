using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class CameraController : MonoBehaviour
    {
        private Camera cam;
        public Transform sFruit;
        public bool isActive;

        public static CameraController Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
            cam = GetComponent<Camera>();
        }
        private void Update() {
            if(Input.GetKeyDown(KeyCode.W))
            {
                isActive = !isActive;
            }
            if(isActive)
            {
                if(sFruit != null)
                    ChangeSize(sFruit.transform);
            }
            else
            {
                sFruit = null;
                cam.orthographicSize = 5f;
                var pos = transform.position;
                pos.x = pos.y = 0f;
                transform.position = pos;
            }
        }
        private void ChangeSize(Transform obj)
        {
            cam.orthographicSize = 3f;
            var pos = obj.transform.position;
            pos.z = transform.position.z;
            transform.position = pos;
            Time.timeScale = 0.5f;
        }
    }
}
