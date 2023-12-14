using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _fruitPrefabs;
    [SerializeField] private GameObject _fruitParent;
    [SerializeField] private float splitSpeed = 10f;
    private GameObject currentFruit;
    private bool cutIt;

    //Fruit pool
    private List<GameObject> _fruitList = new List<GameObject>();
    private void Awake() {
        CreateFruits();
    }
    private void Start() {
        SpawnTimer();
    }
    private void Update() {
        // if(currentFruit != null && !currentFruit.activeInHierarchy)
        //     currentFruit = null;
        //Spawn new fruit
        if(Input.GetKeyDown(KeyCode.A))
        {
            var fruit = GetFruit();
            // currentFruit = fruit;
            fruit.SetActive(true);
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
        //Delay
        yield return new WaitForSeconds(delayTime);

        // Random fruit spawn amount
        int rndSpawnCount = Random.Range(1, 11);

        for (int i = 0; i < rndSpawnCount; i++)
        {
            var fruit = GetFruit();
            currentFruit = fruit;
            fruit.SetActive(true);
        }
        StartCoroutine(SpawnTimerCor(delayTime));
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
}
