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
    [SerializeField] private TMP_Text _tCurrentScore;
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

        if(FindObjectOfType<VGPGSManager>() != null)
            _highestScore.text = VGPGSManager.Instance._playerData.highestScore.ToString();
    }
    public void IncreaseScore(int count)
    {
        scoreCount += count;
        _tScore.text = scoreCount.ToString();

        if(scoreCount % 50 == 0)
            FruitSpawner.Instance.IncreasePercentage();

        _imageAnimator.SetTrigger("scoreIncreaser");

        if(scoreCount >= 1000)
            VAchievement.Instance.AchievementFruitNinjaLegend();
        else if(scoreCount >= 500)
            VAchievement.Instance.AchievementFruitNinjaMaster();
        else if(scoreCount >= 100)
            VAchievement.Instance.AchievementFruitNinjaApprentice();
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
            }
        }
    }
    public void EndTheGame()
    {
        if(maxHealth > 0)
        {
            for (int i = 0; i < maxHealth; i++)
            {
                healths[i].GetComponent<Animator>().SetTrigger("popUp");
                healths[i].GetComponent<Image>().color = Color.black;
            }
            maxHealth = 0;
            StartCoroutine(nameof(BombCountdownCor));
        }
    }
    IEnumerator BombCountdownCor()
    {
        GameManager.Situation = GameSituation.Stop;
        #region Play Game Mission
        // foreach(var mission in MissionController.Instance._selectedMissionsScriptable.selectedMissions)
        // {
        //     if(mission.type == MissionType.Play)
        //     {
        //         mission.PlayTheGame();
        //     }
        // }
        foreach(var mission in VGPGSManager.Instance._playerData.selectedMissions)
        {
            if(mission.type == MissionType.Play)
            {
                mission.PlayTheGame();
            }
        }
        #endregion

        MissionController.Instance.checkMissions = true;
        yield return new WaitForSeconds(2f);
        EventManager.Broadcasting(GameEvents.OnFinishGame);
        yield return new WaitForSeconds(1f);
        EventManager.Broadcasting(GameEvents.OnPlayerValuesChanges);
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
        StartCoroutine(IncreaseSessionScoreCor());
        #endregion
    }
    IEnumerator IncreaseSessionScoreCor()
    {
        int score = 0;
        while(score < scoreCount)
        {
            score += 1;
            _tCurrentScore.text = score.ToString();
            yield return new WaitForSeconds(0.01f);
        }
        VSavedGamesUI.Instance._scoreDone = true;
        VSavedGamesUI.Instance.CheckEverythingIsDone();
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
        EventManager.AddHandler(GameEvents.OnPlayerValuesChanges, EndGameUpdateCloud);
        EventManager.AddHandler(GameEvents.OnPlayerValuesChanges, UltimateEndGameScreenUpdate);

        EventManager.AddHandler(GameEvents.OnFinishGame, DisableUI);
        EventManager.AddHandler(GameEvents.OnFinishGame, ActivateEndScreenUI);
    }
    private void OnDisable() {
        EventManager.RemoveHandler(GameEvents.OnPlayerValuesChanges, EndGameUpdateCloud);
        EventManager.RemoveHandler(GameEvents.OnPlayerValuesChanges, UltimateEndGameScreenUpdate);

        EventManager.RemoveHandler(GameEvents.OnFinishGame, DisableUI);
        EventManager.RemoveHandler(GameEvents.OnFinishGame, ActivateEndScreenUI);
    }
}
