using UnityEngine;

public class Piece : MonoBehaviour
{
    private GameObject parentFruit;
    private Vector3 basePosition;
    private SpriteRenderer _renderer;
    private Vector3 rot;
    private float rotateSpeed = 50f;
    private float multiplier = 1f;
    public bool DidGetCut {get; set;}


    private Camera cam;
    private float minX, maxX;
    private float minY, maxY;

    private Color defaultColor;
    private void Awake() {
        if(transform.parent != null)
            parentFruit = transform.parent.gameObject;

        cam = Camera.main;
        _renderer = GetComponent<SpriteRenderer>();

        basePosition = transform.localPosition;
        defaultColor = new Color(1f, 1f, 1f);

        //Assign min and max values of the screen
        minX = cam.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
        maxX = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        minY = cam.ScreenToWorldPoint(new Vector2(0f, 0f)).y;
        maxY = cam.ScreenToWorldPoint(new Vector2(0f, Screen.height)).y;
    }

    private void OnEnable() {
        int d = Random.Range(-1, 2);
        multiplier = d >= 0 ? 1 : -1;
        rotateSpeed = Random.Range(30f, 70f);
        rot = transform.rotation.eulerAngles;
    }
    private void OnDisable() {
        gameObject.SetActive(false);
        _renderer.color = defaultColor;
    }
    private void Update() {
        if(DidGetCut)
        {
            //Rotate fruit randomly
            rot += Vector3.forward * rotateSpeed * multiplier * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rot);
        }

        if(IsOutsideOfTheCamera() && gameObject.activeInHierarchy)
        {
            if(parentFruit != null)
            {
                if(transform.parent == null)
                    transform.SetParent(parentFruit.transform);

                transform.rotation = Quaternion.Euler(Vector3.zero);
                transform.localPosition = basePosition;

                /*
                    • Check did both pieces return
                    • Also we can check, are there objects with tags up and down
                    • If there is we can make the fruit cutable again
                */
                if(parentFruit.transform.childCount >= 3)
                    parentFruit.GetComponent<Fruit>().cutable = true;
            }
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
}
