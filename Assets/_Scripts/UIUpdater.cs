using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Runtime;

public class UIUpdater : MonoBehaviour
{
    [SerializeField] private float colorTime = 1f;
    [SerializeField] private GameObject _imageScore;
    [SerializeField] private TMP_Text _tScore;
    [SerializeField] private Transform healthHolder;

    private List<GameObject> healths = new List<GameObject>();

    public bool gameEnded;
    private int maxHealth;
    private int scoreCount;
    private Animator _imageAnimator;


    public static UIUpdater Instance;
    private void Awake() {
        if(Instance == null) Instance = this;

        scoreCount = 0;
        maxHealth = 3;

        foreach(Transform healthImage in healthHolder)
            healths.Add(healthImage.gameObject);
        _imageAnimator = _imageScore.GetComponent<Animator>();
    }
    public void IncreaseScore()
    {
        scoreCount++;
        _tScore.text = scoreCount.ToString();

        if(scoreCount % 50 == 0)
            FruitSpawner.Instance.IncreasePercentage();

        _imageAnimator.SetTrigger("scoreIncreaser");
    }
    public void DecreaseHealth()
    {
        if(maxHealth > 0)
        {
            maxHealth--;
            healths[maxHealth].GetComponent<Animator>().SetTrigger("popUp");
            healths[maxHealth].GetComponent<Image>().color = Color.black;
            if(maxHealth <= 0)
            {
                StartCoroutine(nameof(CountdownCor));
                Debug.Log("<color=red>Game is over</color>");
            }
        }
    }
    public void EndTheGame()
    {
        Debug.Log("End The Game");
        if(maxHealth > 0)
        {
            for (int i = 0; i < maxHealth; i++)
            {
                healths[i].GetComponent<Animator>().SetTrigger("popUp");
                healths[i].GetComponent<Image>().color = Color.black;
            }
            maxHealth = 0;
            Debug.Log("<color=red>Game is over</color>");
            StartCoroutine(nameof(CountdownCor));
        }
    }
    IEnumerator CountdownCor()
    {
        GameManager.Situation = GameSituation.Stop;
        yield return new WaitForSeconds(2f);
        EventManager.Broadcasting(GameEvents.OnFinishGame);
    }
    private void LoadSceneAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnEnable() {
        EventManager.AddHandler(GameEvents.OnFinishGame, LoadSceneAgain);
    }
    private void OnDisable() {
        EventManager.RemoveHandler(GameEvents.OnFinishGame, LoadSceneAgain);
    }
}