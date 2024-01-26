using System.Collections;
using System.Collections.Generic;
using Runtime;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _fruitPrefabs;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private GameObject _specialFruitPrefab;
    [SerializeField] private GameObject _fruitParent;
    [SerializeField] private float splitSpeed = 10f;

    //Fruit pool
    private List<GameObject> _fruitList = new List<GameObject>();
    private List<GameObject> _bombList = new List<GameObject>();
    private List<GameObject> _specialFruitList = new List<GameObject>();
    public List<Fruit> _activeFruits = new List<Fruit>();

    public float bombMaxPercentage = 0.25f;
    private float randomDelayForEachFruit;
    private bool isFirstWayOfSpawningFruits;

    public static FruitSpawner Instance;
    private void Awake() {
        if(Instance == null) Instance = this;

        bombMaxPercentage = 0.2f;

        // Create Pools
        CreateFruits();
        CreateBombs();
        CreateSpecialFruit();
    }
    private void Start() {
        SpawnTimer();
    }
    private void Update() {
        #region Manual Spawning
        //Spawn new fruit
        if(Input.GetKeyDown(KeyCode.A))
        {
            var fruit = GetFruit();
            fruit.SetActive(true);
        }
        //Spawn new bomb
        if(Input.GetKeyDown(KeyCode.S))
        {
            var bomb = GetBomb();
            bomb.SetActive(true);
        }
        //Spawn new fruit
        if(Input.GetKeyDown(KeyCode.D))
        {
            var sFruit = GetSpecialFruit();
            sFruit.SetActive(true);
        }
        #endregion
    }
    private void SpawnTimer()
    {
        // Random delay time
        float rndDelayTime = Random.Range(2f, 3f);
        // Start spawning fruits, bombs and special fruits
        StartCoroutine(SpawnTimerCor(rndDelayTime));
    }
    IEnumerator SpawnTimerCor(float delayTime)
    {
        // Delay
        yield return new WaitForSeconds(delayTime);

        // Is Game Over? If it's not spawn new fruits
        if(GameManager.Situation == GameSituation.Play)
        {
            // Random fruit spawn amount
            int rndSpawnCount = Random.Range(1, 11);
            // isFirstWayOfSpawningFruits = Random.Range(0, 2) == 0 ? true : false;

            #region First way of spawning fruits
            if(isFirstWayOfSpawningFruits)
            {
                for (int i = 0; i < rndSpawnCount; i++)
                {
                    var fruit = GetFruit();
                    _activeFruits.Add(fruit.GetComponent<Fruit>());
                    fruit.SetActive(true);
                }
            }
            #endregion

            #region Second way of spawning fruits
            else
            {
                for (int i = 0; i < rndSpawnCount; i++)
                {
                    var fruit = GetFruit();
                    _activeFruits.Add(fruit.GetComponent<Fruit>());
                    fruit.SetActive(true);
                    SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.fruitPopSFX);

                    randomDelayForEachFruit = Random.Range(0.1f, 0.5f);
                    yield return new WaitForSeconds(randomDelayForEachFruit);
                }
            }
            #endregion
            SpawnBomb(bombMaxPercentage);
            SpawnSpecialFruit(0.1f);
            StartCoroutine(SpawnTimerCor(delayTime));
        }
    }
    public void IncreasePercentage()
    {
        if(bombMaxPercentage < 0.2f)
        {
            bombMaxPercentage += 0.02f;
            Debug.Log(bombMaxPercentage);
        }
    }
    private void SpawnBomb(float min)
    {
        float percentage = Random.Range(0f, 1f);
        Debug.Log("Bomb, per: " + percentage);
        if(percentage < min)
        {
            int spawnAmount = Random.Range(1, 3);
            for (int i = 0; i < spawnAmount; i++)
            {
                var _bomb = GetBomb();
                _bomb.SetActive(true);
            }
        }
    }
    private void SpawnSpecialFruit(float min)
    {
        float percentage = Random.Range(0f, 1f);
        Debug.Log("Special, per: " + percentage);
        if(percentage < min)
        {
            var _sFruit = GetSpecialFruit();
            _sFruit.SetActive(true);
        }
    }


    #region Fruit Pool
    //Spawns objects for pool
    private void CreateFruits()
    {
        for (int i = 0; i < _fruitPrefabs.Length; i++)
        {
            var fruit = Instantiate(_fruitPrefabs[i]);
            fruit.transform.SetParent(_fruitParent.transform);
            fruit.SetActive(false);
            _fruitList.Add(fruit);
        }
    }
    //Creates new fruit, if there is no object on the queue
    private GameObject CreateNewFruit()
    {
        var rnd = Random.Range(0, _fruitPrefabs.Length);
        var fruit = Instantiate(_fruitPrefabs[rnd]);
        fruit.transform.SetParent(_fruitParent.transform);
        fruit.SetActive(false);
        _fruitList.Add(fruit);

        return fruit;
    }
    //Get fruit from the pool
    private GameObject GetFruit()
    {
        int i = 0;
        while(i < 10)
        {
            var rnd = Random.Range(0, _fruitList.Count);
            if(!_fruitList[rnd].activeInHierarchy && _fruitList[rnd].GetComponent<Fruit>().cutable)
                return _fruitList[rnd];
            i++;
        }

        return CreateNewFruit();
    }
    #endregion
    #region Bomb Pool
    private void CreateBombs()
    {
        for(int i = 0; i < 10; i++)
        {
            var hit = Instantiate(_bombPrefab);
            hit.SetActive(false);
            hit.transform.SetParent(_fruitParent.transform);
            _bombList.Add(hit);
        }
    }
    private GameObject GetBomb()
    {
        SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.bombPop);
        foreach(var bomb in _bombList)
        {
            if(!bomb.activeSelf)
                return bomb;
        }
        var newBomb = Instantiate(_bombPrefab);
        newBomb.SetActive(false);
        newBomb.transform.SetParent(_fruitParent.transform);
        _bombList.Add(newBomb);
        return newBomb;
    }
    #endregion
    #region Special Fruit Pool
    //Spawns objects for pool
    private void CreateSpecialFruit()
    {
        for (int i = 0; i < _fruitPrefabs.Length; i++)
        {
            var fruit = Instantiate(_fruitPrefabs[i]);
            fruit.transform.SetParent(_fruitParent.transform);
            fruit.SetActive(false);
            _fruitList.Add(fruit);
        }
    }
    //Get fruit from the pool
    private GameObject GetSpecialFruit()
    {
        foreach (var sFruit in _specialFruitList)
        {
            if(!sFruit.activeInHierarchy)
            {
                return sFruit;
            }
        }
        var _sFruit = Instantiate(_specialFruitPrefab);
        _sFruit.transform.SetParent(_fruitParent.transform);
        _sFruit.SetActive(false);
        _specialFruitList.Add(_sFruit);
        return _sFruit;
    }
    #endregion
}
