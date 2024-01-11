using UnityEngine;

namespace Runtime
{
    public class Dojo : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private void Awake() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Start() {
            AsssignTheDojo();
        }
        private void AsssignTheDojo()
        {
            _spriteRenderer.sprite = BladesAndDojos.Instance._selectedDojo.dojoSprite;
        }
    }
}
