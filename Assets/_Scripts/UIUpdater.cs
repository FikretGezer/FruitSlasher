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
    [SerializeField] private Transform pauseButton;
    [SerializeField] private Transform endScreenUI;
    [Header("Upper Board Variables")]
    [SerializeField] private TMP_Text _highestScore;
    [Header("Left Board Variables")]
    [SerializeField] private TMP_Text _currentScore;
    [SerializeField] private EndGameMenu _endGameMenu;

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
        pauseButton.GetComponent<Animator>().SetTrigger("disableUI");
    }
    private void ActivateEndScreenUI()
    {
        endScreenUI.gameObject.SetActive(true);
        _endGameMenu.upperBoard.SetActive(true);
        _endGameMenu.leftBoard.SetActive(true);
        _endGameMenu.rightBoard.SetActive(true);
        _endGameMenu.bottomBoard.SetActive(true);
        _endGameMenu.playAgainButton.SetActive(true);

        _endGameMenu.upperBoard.GetComponent<Animator>().SetBool("slideDown", true);
        _endGameMenu.leftBoard.GetComponent<Animator>().SetTrigger("slideRight");
        _endGameMenu.rightBoard.GetComponent<Animator>().SetTrigger("slideLeft");
        _endGameMenu.bottomBoard.GetComponent<Animator>().SetTrigger("slideUp");
        _endGameMenu.playAgainButton.GetComponent<Animator>().SetTrigger("pop");

        _endGameMenu.playAgainButton.transform.GetChild(0).GetComponent<Animator>().SetTrigger("rotateF");
        _endGameMenu.playAgainButton.transform.GetChild(1).GetComponent<Animator>().SetTrigger("rotateC");
    }
    private void UltimateEndGameScreenUpdate()
    {
        #region Left Board
        _currentScore.text = scoreCount.ToString();
        #endregion
    }
    private void EndGameUpdateCloud()
    {
        #region Score Update
        if(FindObjectOfType<VSavedGamesUI>() != null)
        {
            VSavedGamesUI.Instance.UpdateHighestScore(scoreCount);
        }

        #endregion
    }
    private void OnEnable() {
        EventManager.AddHandler(GameEvents.OnFinishGame, EndGameUpdateCloud);
        EventManager.AddHandler(GameEvents.OnFinishGame, DisableUI);
        EventManager.AddHandler(GameEvents.OnFinishGame, ActivateEndScreenUI);
        EventManager.AddHandler(GameEvents.OnFinishGame, UltimateEndGameScreenUpdate);
    }
    private void OnDisable() {
        EventManager.RemoveHandler(GameEvents.OnFinishGame, EndGameUpdateCloud);
        EventManager.RemoveHandler(GameEvents.OnFinishGame, DisableUI);
        EventManager.RemoveHandler(GameEvents.OnFinishGame, ActivateEndScreenUI);
        EventManager.RemoveHandler(GameEvents.OnFinishGame, UltimateEndGameScreenUpdate);
    }
}
[System.Serializable]
public class EndGameMenu {
    public GameObject upperBoard;
    public GameObject leftBoard;
    public GameObject rightBoard;
    public GameObject bottomBoard;
    public GameObject playAgainButton;
}
