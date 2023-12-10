using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _fruitPrefabs;
    [SerializeField] private GameObject _fruitParent;

    //Fruit pool
    private List<GameObject> _fruitList = new List<GameObject>();
    private void Awake() {
        CreateFruits();
    }
    private void Update() {
        //Spawn new fruit
        if(Input.GetMouseButtonDown(0))
        {
            var fruit = GetFruit();
            fruit.SetActive(true);
        }
    }
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
        // foreach(var fruit in _fruitList)
        // {
        //     if(!fruit.activeInHierarchy)
        //     {
        //         return fruit;
        //     }
        // }
        int i = 0;
        while(i < 10)
        {
            var rnd = Random.Range(0, _fruitList.Count);
            if(!_fruitList[rnd].activeInHierarchy)
                return _fruitList[rnd];
            i++;
        }

        return CreateNewFruit();
    }
}
