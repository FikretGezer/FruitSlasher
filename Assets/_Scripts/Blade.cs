using System;
using System.Collections;
using System.Collections.Generic;
using Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blade : MonoBehaviour
{
    private readonly string[] tags = {"greenApple","lemon","lime","orange","peach","pear","redApple","starFruit","strawberry","watermelon"};
    private readonly string[] colors = {"#ABC837","#FFDC53","#94B800","#FF8A00","#FFCD08","#A0BB33","#C90000","#F9D700","#D40000","#C90000"};
    // private readonly string[] colors = {"#ABC837","#FFDC53","#94B800","#FF8A00","#FFCD08","#A0BB33","#C90000","#F9D700","#D40000","#225500"};
    [SerializeField] private GameObject trailEffect;
    [SerializeField] private Sprite[] splashes;

    private Camera cam;
    private Vector2 startPos;
    private Vector2 endPos;

    #region Player Data Params
    private int _fruitAmountThatGotCut;
    private int _comboAmount;
    private int _specialFruitAmount;
    private List<string> _uniqueList = new List<string>();

    private void CalculateUniques(string tag)
    {
        if(!_uniqueList.Contains(tag) && Array.IndexOf(tags, tag) != -1)
        {
            _uniqueList.Add(tag);
        }
    }
    #endregion

    private void Awake() {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Confined;
        _fruitAmountThatGotCut = 0;

        if(trailEffect != null)
            trailEffect.SetActive(false);
    }
    private void Update() {
        RenderTrailEffect();
        if(GameManager.Situation == GameSituation.Play)
        {
            Slash();
        }
    }
    private bool started;
    private void ChangeJuiceColor(int idx, Vector3 pos)
    {
        var juice = EffectSpawner.Instance.GetEffect(EffectType.JuiceEffect);
        var main = juice.GetComponent<ParticleSystem>().main;
        var clr = CodeToColor(idx);
        main.startColor = new ParticleSystem.MinMaxGradient(clr, clr);

        foreach(Transform pEf in juice.transform)
        {
            main = pEf.gameObject.GetComponent<ParticleSystem>().main;
            main.startColor = new ParticleSystem.MinMaxGradient(clr, clr);
        }
        juice.transform.position = pos;
        juice.SetActive(true);
    }
    private Color CodeToColor(int colorIndex)
    {
        Color newCol;
        if(ColorUtility.TryParseHtmlString(colors[colorIndex], out newCol))
        {
            return newCol;
        }
        return default;
    }
    private bool isSpecialFruit;
    private void Slash()
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

            // RaycastHit2D hit = Physics2D.Raycast(startPos, dir, length);
            RaycastHit2D hit = Physics2D.Raycast(endPos, Vector3.forward);
            if(hit.collider != null)
            {
                var fruit = hit.collider.GetComponent<Fruit>();
                var specialFruit = hit.collider.GetComponent<SpecialFruit>();
                if(fruit != null)
                {
                    CutTheFruit(fruit);
                }
                else if(specialFruit != null)
                {
                    CameraController.Instance.SFruit = specialFruit.transform;
                    CameraController.Instance.IsActive = true;

                    if(!isSpecialFruit)
                    {
                        isSpecialFruit = true;

                        // Cut Effect
                        SpawnCutEffect(specialFruit.transform.position);

                        // Splash Effect
                        SpawnSplash("redApple", specialFruit.transform.position);

                        EffectSpawner.Instance.GetTextEffect(specialFruit.transform.position);
                    }
                }
                else//This is bomb
                {
                    var _bomb = hit.collider.GetComponent<Bomb>();
                    if(_bomb != null)
                        ExploadTheBomb(_bomb);
                    else
                    {
                        isSpecialFruit = false;
                    }
                }
                startPos = hit.collider.transform.position;
            }
            else
            {
                isSpecialFruit = false;
            }
        }
    }
    private void CutTheFruit(Fruit fruit)
    {
        if(fruit.cutable)//Assign a bool variable rather than checking childcount, it's much more safer
        {
            //In the original game when fruit got cut, to give the player more accurate cutting filling
            //Fruit was being rotated
            //Do rotating
            fruit.cutable = false;

            // Player Data Stars And XP Params
            _fruitAmountThatGotCut++;
            CalculateUniques(fruit.tag);

            // Rotate Fruit
            RotateFruitInCuttingAxis(fruit.gameObject);

            // Cut Effect
            SpawnCutEffect(fruit.transform.position);

            // Splash Effect
            SpawnSplash(fruit.tag, fruit.transform.position);

            UIUpdater.Instance.IncreaseScore(1);

            fruit.cutIt = true;
            if(!comboStart)
                StartCoroutine(ComboCor(0.1f));
            else
            {
                comboCount++;
                lastHitFruitPos = cam.WorldToScreenPoint(fruit.transform.position);
            }
        }
    }
    private int comboCount = 1;
    private Vector3 lastHitFruitPos;
    private bool comboStart = false;
    private void RotateFruitInCuttingAxis(GameObject fruit)
    {
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
    }
    private void SpawnCutEffect(Vector2 pos)
    {
        // cut effect
        var hitEf = EffectSpawner.Instance.GetEffect(EffectType.HitEffect);
        hitEf.transform.position = pos;
        hitEf.SetActive(true);
    }
    private void ExploadTheBomb(Bomb _bomb)
    {
        _bomb.cutable = false;

        var bombEf = EffectSpawner.Instance.GetEffect(EffectType.BombEffect);
        bombEf.transform.position = _bomb.transform.position;
        bombEf.SetActive(true);

        _bomb.cutIt = true;
        UIUpdater.Instance.EndTheGame();
    }
    private void RenderTrailEffect()
    {
        if(trailEffect != null && Time.timeScale > 0.1f)
        {
            trailEffect.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
            if(Input.GetMouseButton(0))
            {
                trailEffect.SetActive(true);
            }
            if(Input.GetMouseButtonUp(0))
                trailEffect.SetActive(false);
        }
        else
            trailEffect.SetActive(false);
    }
    private void SpawnSplash(string fruitTag, Vector3 pos)
    {
        // Get splash from the pool
        var splashEf = EffectSpawner.Instance.GetEffect(EffectType.SplashEffect);
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
                ChangeJuiceColor(idx, pos);
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
    private IEnumerator ComboCor(float time)
    {
        comboStart = true;
        comboCount = 1;
        yield return new WaitForSeconds(time);
        Debug.Log("ComboCount: " + comboCount);
        if(comboCount > 1)
        {
            EffectSpawner.Instance.GetComboTextEffect(lastHitFruitPos, comboCount);
            _comboAmount++;
        }
        comboStart = false;
    }
    private void IncreaseXP()
    {
        VSavedGamesUI.Instance.CalculateExperience(_fruitAmountThatGotCut);
    }
    private void IncreaseStars()
    {
        VSavedGamesUI.Instance.CalculateStars(_uniqueList.Count, _specialFruitAmount, _comboAmount);
    }
    private void OnEnable() {
        EventManager.AddHandler(GameEvents.OnPlayerValuesChanges, IncreaseXP);
        EventManager.AddHandler(GameEvents.OnPlayerValuesChanges, IncreaseStars);
    }
    private void OnDisable() {
        EventManager.RemoveHandler(GameEvents.OnPlayerValuesChanges, IncreaseXP);
        EventManager.RemoveHandler(GameEvents.OnPlayerValuesChanges, IncreaseStars);
    }
}
