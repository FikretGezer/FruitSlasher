using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blade : MonoBehaviour
{
    private readonly string[] tags = {"greenApple","lemon","lime","orange","peach","pear","redApple","starFruit","strawberry","watermelon"};
    private readonly string[] colors = {"#ABC837","#FFDC53","#94B800","#FF8A00","#FFCD08","#A0BB33","#C90000","#F9D700","#D40000","#225500"};
    [SerializeField] private GameObject trailEffect;
    [SerializeField] private Sprite[] splashes;

    private Camera cam;
    private Vector2 startPos;
    private Vector2 endPos;
    private void Awake() {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Confined;

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

                        // cut effect
                        var hitEf = EffectSpawner.Instance.GetEffect();
                        hitEf.transform.position = fruit.transform.position;
                        hitEf.SetActive(true);

                        // splash effect
                        SpawnSplash(fruit.tag, fruit.transform.position);

                        UIUpdater.Instance.IncreaseScore();

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
    private void SpawnSplash(string fruitTag, Vector3 pos)
    {
        // Get splash from the pool
        var splashEf = EffectSpawner.Instance.GetEffectSplash();
        splashEf.transform.position = pos;

        // Check tags for coloring splashes
        foreach(var currentTag in tags)
        {
            if(fruitTag == currentTag)
            {
                int idx = Array.IndexOf(tags, currentTag);

                Color newCol;
                if(ColorUtility.TryParseHtmlString(colors[idx], out newCol))
                {
                    splashEf.GetComponent<SpriteRenderer>().color = newCol;
                }
            }
        }
        // Assign random size to make it look random
        var rndSize = Random.Range(0.3f, 0.5f);
        splashEf.transform.localScale = new Vector3(rndSize, rndSize, rndSize);

        // Assign random splash sprite to make it look random
        var rndSplash = Random.Range(0, splashes.Length);
        splashEf.GetComponent<SpriteRenderer>().sprite = splashes[rndSplash];

        // Start animation
        splashEf.SetActive(true);
        splashEf.GetComponent<Animator>().SetTrigger("reduceAlpha");
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
