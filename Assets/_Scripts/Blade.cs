using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private GameObject trailEffect;

    private Camera cam;
    private Vector2 startPos;
    private Vector2 endPos;
    private void Awake() {
        cam = Camera.main;

        if(trailEffect != null)
            trailEffect.SetActive(false);
    }
    private void Update() {
        RenderTrailEffect();
        CutTheFruit();



    }
    private bool started;
    private void CutTheFruit()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPos = cam.ScreenPointToRay(Input.mousePosition).origin;
        }
        if(Input.GetMouseButton(0))
        {
            endPos = cam.ScreenPointToRay(Input.mousePosition).origin;

            if(endPos == startPos)
                return;

            started = true;

            var dir = endPos - startPos;
            var length = dir.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(startPos, dir, length);
            if(hit.collider != null)
            {
                var fruit = hit.collider.GetComponent<Fruit>();
                if(fruit != null)
                {
                    if(fruit.cutable)//Assign a bool variable rather than checking childcount, it's much more safer
                    {
                        //In the original game when fruit got cut, to give the player more accurate cutting filling
                        //Fruit was being rotated
                        //Do rotating
                        fruit.cutable = false;
                        var up = Vector2.up;
                        var right = Vector2.right;
                        var rightR = (endPos - startPos).normalized;
                        Vector2 upR = (Vector2)Vector3.Cross(rightR, -Vector3.forward);
                        if(startPos.x > endPos.x)
                        {
                            upR = (Vector2)Vector3.Cross(rightR, Vector3.forward);
                        }

                        var targetRot = Quaternion.FromToRotation(up, upR);
                        fruit.transform.rotation = Quaternion.Euler(targetRot.eulerAngles);

                        fruit.cutIt = true;
                        /*
                            This is open for some minor adjustments
                                • When players swipe down to up, if start x > end x, fruit rotates other side
                        */
                    }
                }
            }
        }
    }
    private void RenderTrailEffect()
    {
        if(trailEffect != null)
        {
            trailEffect.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
            if(Input.GetMouseButton(0))
            {
                trailEffect.SetActive(true);
            }
            if(Input.GetMouseButtonUp(0))
                trailEffect.SetActive(false);
        }
    }
    /*
        1->
            • Add colliders to the fruits
            • Get start and end positions for mouse
            • Create a vector between start and end pos
            • Limit length in physics.raycast func
            • Try detecting fruits
            • If one of them is detected, cut it
        2->
            • Check startpos and endpos are not same
            • For that use getmousebutton rather than getmousebuttunup
    */
    private void OnDrawGizmos() {
        //Basic
        var zero = Vector2.zero;
        var up = Vector2.up;
        var right = Vector2.right;

        // Gizmos.color = Color.green;
        // Gizmos.DrawLine(zero, up * 2);
        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(zero, right * 2);

        //Rotated
        if(started)
        {
            var rightR = (endPos - startPos).normalized;
            var upR = (Vector2)Vector3.Cross(rightR, -Vector3.forward);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPos, startPos + rightR * 2);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPos, startPos + upR * 2);

            var rot = Quaternion.FromToRotation(up, upR);
            Debug.Log("Rotation: " + rot.eulerAngles);
        }
    }
}
