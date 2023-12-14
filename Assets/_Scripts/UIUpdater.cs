using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpdater : MonoBehaviour
{
    [SerializeField] private float colorTime = 1f;
    [SerializeField] private GameObject _imageScore;
    [SerializeField] private TMP_Text _tScore;
    [SerializeField] private Transform healthHolder;

    private List<GameObject> healths = new List<GameObject>();

    private int maxHealth = 2;
    private int scoreCount = 0;
    private Animator _imageAnimator;

    public static UIUpdater Instance;
    private void Awake() {
        if(Instance == null) Instance = this;

        scoreCount = 0;
        maxHealth = 2;

        foreach(Transform healthImage in healthHolder)
            healths.Add(healthImage.gameObject);
        _imageAnimator = _imageScore.GetComponent<Animator>();
    }
    public void IncreaseScore()
    {
        scoreCount++;
        _tScore.text = scoreCount.ToString();
        _imageAnimator.SetTrigger("scoreIncreaser");
    }
    public void DecreaseHealth()
    {
        if(maxHealth >= 0)
        {
            healths[maxHealth].GetComponent<Animator>().SetTrigger("popUp");
            healths[maxHealth].GetComponent<Image>().color = Color.black;
            if(maxHealth <= 0)
                Debug.Log("<color=red>Game is over</color>");
            maxHealth--;
        }
    }
}
