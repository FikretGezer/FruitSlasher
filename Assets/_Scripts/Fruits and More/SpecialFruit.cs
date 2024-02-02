using UnityEngine;

namespace Runtime
{
    public class SpecialFruit : MonoBehaviour
    {
        [SerializeField] private float spawnOffset = 1f;
        private Rigidbody2D rigid;
        private Camera cam;
        private bool jump;
        private bool isItAppearedFirst;
        private float jumpSpeed = 12f;
        private float minX, maxX;
        private float minY, maxY;

        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            cam = Camera.main;

            minX = cam.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
            maxX = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
            minY = cam.ScreenToWorldPoint(new Vector2(0f, 0f)).y;
            maxY = cam.ScreenToWorldPoint(new Vector2(0f, Screen.height)).y;
        }
        private void OnEnable() {
            float randomX = Random.Range(minX, maxX);
            transform.position = new Vector2(randomX, cam.ScreenToWorldPoint(new Vector2(0f, 0f)).y - spawnOffset);
        }

        void Update()
        {
            if(IsOutsideOfTheCamera() && isItAppearedFirst)
            {
                gameObject.SetActive(false);
            }
            if(!IsOutsideOfTheCamera())
            {
                isItAppearedFirst = true;
            }
        }
        void FixedUpdate()
        {
            if(!jump)
            {
                jump = true;
                var posX = transform.position.x / 10f;
                var rndX = posX < 0f ? Random.Range(0f, -posX) : Random.Range(-posX, 0f);
                var forceDir = new Vector2(rndX, 1f);
                rigid.AddForce(forceDir * jumpSpeed, ForceMode2D.Impulse);
            }
        }
        private bool IsOutsideOfTheCamera()
        {
            if(transform.position.x < minX - 1f
            || transform.position.x > maxX + 1f
            || transform.position.y < minY - 1f
            || transform.position.y > maxY + 1f)
                return true;

            return false;
        }
        private void OnDisable() {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = 0f;
            jump = false;
            isItAppearedFirst = false;

            CameraController.Instance.IsActive = false;
            Time.timeScale = 1f;
            EffectSpawner.Instance.SpawnBigText();
        }
    }
}
