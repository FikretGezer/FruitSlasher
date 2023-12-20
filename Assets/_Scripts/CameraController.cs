using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class CameraController : MonoBehaviour
    {
        private Camera cam;
        public Transform SFruit { get; set; }
        public bool IsActive { get; set; }
        private float currentSize = 5f, targetSize = 4f;
        // cam size 3; x: 3.5, y: 2
        // cam size 4; x: 1.8, y: 1

        [SerializeField] private Vector2 min, max;
        [SerializeField] private float lerpSpeed = 1f;

        public static CameraController Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
            cam = GetComponent<Camera>();
        }
        private void Update() {
            if(Input.GetKeyDown(KeyCode.W))
            {
                IsActive = !IsActive;
            }
            if(IsActive)
            {
                if(SFruit != null)
                    ChangeSize(SFruit.transform);
            }
            else
            {
                SFruit = null;
                cam.orthographicSize = currentSize;
                var pos = transform.position;
                pos.x = pos.y = 0f;
                transform.position = pos;
                elapsedTime = 0f;
            }
        }
        private void ChangeSize(Transform obj)
        {
            cam.orthographicSize = targetSize;

            var pos = obj.transform.position;
            pos.z = transform.position.z;

            pos.x = Mathf.Clamp(pos.x, min.x, max.x);
            pos.y = Mathf.Clamp(pos.y, min.y, max.y);

            transform.position = pos;
            Time.timeScale = 0.5f;
            Timer(3f, obj.gameObject);
        }
        private float elapsedTime = 0f;
        private void Timer(float time, GameObject sFruit)
        {
            if(elapsedTime < time)
            {
                elapsedTime += Time.unscaledDeltaTime;;
            }
            else
            {
                sFruit.SetActive(false);
            }
        }
    }
}
