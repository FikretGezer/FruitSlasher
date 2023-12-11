using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fruit : MonoBehaviour
{
    //For movement
    [SerializeField] private float cutSpeed = 1f;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private float spawnOffset = 1f;
    [SerializeField] private float splitSpeed = 5f;
    [SerializeField] private Vector2 offset;

    private Rigidbody2D rigid;
    private Vector3 rot;
    private bool jump;
    private Camera cam;
    private float minX, maxX;
    private float minY, maxY;
    private float multiplier = 1f;
    private bool isItAppearedFirst;
    private bool didGetCut;
    public bool isCompleted; //Checks are there 3 alt object

    //For splitting
    private GameObject upPart;
    private GameObject downPart;
    private void Awake() {
        //Caching
        rigid = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        //Get parts for slicing
        upPart = transform.GetChild(1).gameObject;
        downPart = transform.GetChild(2).gameObject;

        isCompleted = true;

        //Assign min and max values of the screen
        minX = cam.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
        maxX = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x;
        Debug.Log($"{minX}, {maxX}");
        minY = cam.ScreenToWorldPoint(new Vector2(0f, 0f)).y;
        maxY = cam.ScreenToWorldPoint(new Vector2(0f, Screen.height)).y;
    }
    private void OnEnable() {
        //Set parameters to apply the fruit
        rot = Vector3.zero;
        float randomX = Random.Range(minX, maxX);
        transform.position = new Vector2(randomX, cam.ScreenToWorldPoint(new Vector2(0f, 0f)).y - spawnOffset);
        int d = Random.Range(-1, 2);
        multiplier = d >= 0 ? 1 : -1;
        cutSpeed = Random.Range(10f, 13f);
        rotateSpeed = Random.Range(50f, 90f);
    }
    private void Update() {
        //Rotate fruit randomly
        rot += Vector3.forward * rotateSpeed * multiplier * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rot);

        //Check is fruit outside of the screen before return it to the fruit pool
        if(IsOutsideOfTheCamera() && isItAppearedFirst)
            gameObject.SetActive(false);
        if(!IsOutsideOfTheCamera())
        {
            isItAppearedFirst = true;
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(transform.childCount >= 3)
                didGetCut = true;
        }
    }
    private void FixedUpdate() {
        //Applies force to make fruit fly
        if(!jump)
        {
            jump = true;
            //Apply force more to the right if it spawned on the left of the screen
            //Apply force more to the left if it spawned on the right of the screen
            //Basically change the x value
            var forceDir = new Vector2(-(transform.position.x / 10f), 1f);
            rigid.AddForce(forceDir * cutSpeed, ForceMode2D.Impulse);
        }

        if(didGetCut)
        {
            didGetCut = false;

            GetComponent<Fruit>().isCompleted = false;

            var child0 = transform.GetChild(1);
            var child1 = transform.GetChild(2);

            if(child0.tag != "down")
            {
                var temp = child0;
                child0 = child1;
                child1 = temp;
            }

            child0.gameObject.SetActive(true);
            child1.gameObject.SetActive(true);

            child0.parent = null;
            child1.parent = null;

            child0.GetComponent<Rigidbody2D>().AddForce(-transform.up * splitSpeed, ForceMode2D.Impulse);
            child1.GetComponent<Rigidbody2D>().AddForce(transform.up * splitSpeed, ForceMode2D.Impulse);

            child0.GetComponent<Piece>().DidGetCut = true;
            child1.GetComponent<Piece>().DidGetCut = true;

            gameObject.SetActive(false);
        }
    }
    //Checks if fruit out of the screen
    private bool IsOutsideOfTheCamera()
    {
        if(transform.position.x < minX - 1f
        || transform.position.x > maxX + 1f
        || transform.position.y < minY - 1f
        || transform.position.y > maxY + 1f)
            return true;

        return false;
    }

    //Set default parameters when fruit returns to the pool
    private void OnDisable() {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = 0f;
        rot = Vector3.zero;
        transform.rotation = Quaternion.Euler(rot);
        isItAppearedFirst = false;
        didGetCut = false;
        jump = false;
    }
}
