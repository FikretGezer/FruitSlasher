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
    [SerializeField] private Transform scoreHolder;
    [SerializeField] private Transform endScreenUI;

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
    public void IncreaseScore(int count)
    {
        scoreCount += count;
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
                StartCoroutine(nameof(BombCountdownCor));
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
            StartCoroutine(nameof(BombCountdownCor));
        }
    }
    IEnumerator BombCountdownCor()
    {
        GameManager.Situation = GameSituation.Stop;
        yield return new WaitForSeconds(2f);
        EventManager.Broadcasting(GameEvents.OnFinishGame);
    }
    public void LoadSceneAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void DisableUI()
    {
        healthHolder.GetComponent<Animator>().SetTrigger("disableUI");
        scoreHolder.GetComponent<Animator>().SetTrigger("disableUI");
    }
    private void ActivateEndScreenUI()
    {
        endScreenUI.gameObject.SetActive(true);
        endScreenUI.GetChild(0).GetComponent<Animator>().SetTrigger("slideDown");
        endScreenUI.GetChild(1).GetComponent<Animator>().SetTrigger("slideRight");
        endScreenUI.GetChild(2).GetComponent<Animator>().SetTrigger("slideLeft");
        endScreenUI.GetChild(3).GetComponent<Animator>().SetTrigger("slideUp");
        endScreenUI.GetChild(4).GetComponent<Animator>().SetTrigger("pop");

        endScreenUI.GetChild(4).GetChild(0).GetComponent<Animator>().SetTrigger("rotateF");
        endScreenUI.GetChild(4).GetChild(1).GetComponent<Animator>().SetTrigger("rotateC");

    }
    private void OnEnable() {
        EventManager.AddHandler(GameEvents.OnFinishGame, DisableUI);
        EventManager.AddHandler(GameEvents.OnFinishGame, ActivateEndScreenUI);
    }
    private void OnDisable() {
        EventManager.RemoveHandler(GameEvents.OnFinishGame, DisableUI);
        EventManager.RemoveHandler(GameEvents.OnFinishGame, ActivateEndScreenUI);
    }
}
