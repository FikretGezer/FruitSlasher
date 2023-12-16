using System.Collections;
using System.Collections.Generic;
using Runtime;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _fruitPrefabs;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private GameObject _fruitParent;
    [SerializeField] private float splitSpeed = 10f;

    //Fruit pool
    private List<GameObject> _fruitList = new List<GameObject>();
    private List<GameObject> _bombList = new List<GameObject>();

    public float maxPercentage = 0.1f;

    public static FruitSpawner Instance;
    private void Awake() {
        if(Instance == null) Instance = this;
        maxPercentage = 0.1f;
        CreateFruits();
    }
    private void Start() {
        SpawnTimer();
    }
    private void Update() {
        //Spawn new fruit
        if(Input.GetKeyDown(KeyCode.A))
        {
            var fruit = GetFruit();
            fruit.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            var bomb = GetBomb();
            bomb.SetActive(true);
        }
    }
    private void SpawnTimer()
    {
        // Random delay time
        float rndDelayTime = Random.Range(2f, 3f);
        // Call cor
        StartCoroutine(SpawnTimerCor(rndDelayTime));
    }
    IEnumerator SpawnTimerCor(float delayTime)
    {
        // Delay
        yield return new WaitForSeconds(delayTime);

        // Is Game Over?
        if(GameManager.Situation == GameSituation.Play)
        {
            // Random fruit spawn amount
            int rndSpawnCount = Random.Range(1, 11);

            for (int i = 0; i < rndSpawnCount; i++)
            {
                var fruit = GetFruit();
                fruit.SetActive(true);
            }
            SpawnBomb(0.1f);
            StartCoroutine(SpawnTimerCor(delayTime));
        }
    }
    public void IncreasePercentage()
    {
        if(maxPercentage < 0.2f)
        {
            maxPercentage += 0.02f;
            Debug.Log(maxPercentage);
        }
    }
    private void SpawnBomb(float min)
    {
        float percentage = Mathf.Pow(Random.Range(0f, 1f), 2f);
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
    public GameObject GetBomb()
    {
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
}
