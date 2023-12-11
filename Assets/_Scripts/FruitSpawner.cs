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
    private void Update() {
        if(currentFruit != null && !currentFruit.activeInHierarchy)
            currentFruit = null;
        //Spawn new fruit
        if(Input.GetMouseButtonDown(0))
        {
            var fruit = GetFruit();
            currentFruit = fruit;
            fruit.SetActive(true);
        }
        // if(Input.GetMouseButtonDown(1))
        // {
        //     if(currentFruit != null)
        //     {
        //         cutIt = true;
        //     }
        // }
        // GetPosses();
    }
    // private Vector2 startPos;
    // private Vector2 endPos;
    // private bool clicked;
    // private void GetPosses()
    // {
    //     Debug.Log("deneme");
    //     if(Input.GetMouseButtonDown(0))
    //     {
    //         clicked = false;
    //         startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     }
    //     if(Input.GetMouseButtonUp(0))
    //     {
    //         endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         clicked = true;
    //         Debug.Log($"startPos{startPos}, endPos: {endPos}");
    //     }
    // }
    // private void OnDrawGizmos() {
    //     if(clicked)
    //         Gizmos.DrawLine(startPos, endPos);
    // }
    private void FixedUpdate() {
        // if(cutIt)
        // {
        //     cutIt = false;

        //     currentFruit.GetComponent<Fruit>().isCompleted = false;

        //     var child0 = currentFruit.transform.GetChild(1);
        //     var child1 = currentFruit.transform.GetChild(2);

        //     if(child0.tag != "down")
        //     {
        //         var temp = child0;
        //         child0 = child1;
        //         child1 = temp;
        //     }

        //     child0.gameObject.SetActive(true);
        //     child1.gameObject.SetActive(true);

        //     child0.parent = null;
        //     child1.parent = null;

        //     child0.GetComponent<Rigidbody2D>().AddForce(-currentFruit.transform.up * splitSpeed, ForceMode2D.Impulse);
        //     child1.GetComponent<Rigidbody2D>().AddForce(currentFruit.transform.up * splitSpeed, ForceMode2D.Impulse);

        //     child0.GetComponent<Piece>().DidGetCut = true;
        //     child1.GetComponent<Piece>().DidGetCut = true;

        //     currentFruit.SetActive(false);
        // }
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
        int i = 0;
        while(i < 10)
        {
            var rnd = Random.Range(0, _fruitList.Count);
            if(!_fruitList[rnd].activeInHierarchy && _fruitList[rnd].GetComponent<Fruit>().isCompleted)
                return _fruitList[rnd];
            i++;
        }

        return CreateNewFruit();
    }
}
