using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class Background : MonoBehaviour
    {
        private Camera cam;
        private SpriteRenderer _renderer;

        public static Background Instance;
        private void Awake() {
            if(Instance == null) Instance = this;

            cam = Camera.main;
            _renderer = GetComponent<SpriteRenderer>();
        }
        private void Start() {
            FitBgToScreen();
        }
        public void FitBgToScreen()
        {
            // Reset the local scale before resizing
            _renderer.transform.localScale = Vector3.one;

            float cameraHeight = cam.orthographicSize * 2f;
            float cameraWidth = cameraHeight * cam.aspect;

            // Scale the sprite to fit the camera dimensions
            float spriteWidth = _renderer.bounds.size.x;
            float spriteHeight = _renderer.bounds.size.y;

            float scaleWidth = cameraWidth / spriteWidth;
            float scaleHeight = cameraHeight / spriteHeight;

            float scale = Mathf.Max(scaleWidth, scaleHeight);

            _renderer.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
