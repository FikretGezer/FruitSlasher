using GooglePlayGames;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace Runtime
{
    public class VSavedGamesUI : MonoBehaviour
    {
        // public TMP_Text outputTxt;
        // public void IncreaseScore()
        // {
        //     var score = VGPGSManager.Instance._playerData.highestScore;
        //     score += 10;
        //     VGPGSManager.Instance._playerData.highestScore = score;
        //     VGPGSManager.Instance.OpenSave(true);
        // }
        // public void LoadScore()
        // {
        //     if(outputTxt != null)
        //     {
        //         VGPGSManager.Instance.OpenSave(false);
        //         VGPGSManager.Instance.output = outputTxt;
        //     }
        // }
        // public void IncreaseLevel() {
        //     VGPGSManager.Instance._playerData.level++;
        //     VGPGSManager.Instance.OpenSave(true);
        // }
        [SerializeField] private TMP_Text _tHighestScore;
        [SerializeField] private TMP_Text _tLevel;
        [SerializeField] private TMP_Text _tExpNeeded;
        [SerializeField] private TMP_Text _tExpCurrent;
        [SerializeField] private TMP_Text _tTime;
        [SerializeField] private TMP_Text _tTotalStars;
        [SerializeField] private TMP_Text _tStars;
        [SerializeField] private Image _levelSlider;
        [SerializeField] private Transform _totalStarsImageTransform;
        [SerializeField] private Transform _sessionStarsImageTransform;
        [SerializeField] private float speedController = 1f;
        [SerializeField] private GameObject _starsTrailEffect;
        [SerializeField] private GameObject _starsPopEffect;
        [SerializeField] private GameObject _starsPopEffectSecond;


        #region Time Params
        private float seconds = 0;
        [SerializeField] private bool _calculateGameTime;
        #endregion


        private Animator _tLevelAnimator;
        private bool _expDone;
        private bool _starsDone;
        public bool _scoreDone;

        // Star Trail
        private bool _sendTrail;
        private float current = 0f, target = 1f;
        [SerializeField] private float _trailSpeed = 1f;


        public static VSavedGamesUI Instance;
        private void Awake() {
            if(Instance == null) Instance = this;


            // neededExperience = (int)(baseExperience * (experienceMultiplier * level));

            // var fillAmount = Mathf.Clamp01((float)150f / neededExperience);
            // _levelSlider.fillAmount = fillAmount;
            // _tExpCurrent.text = "Current: " + currentExperience.ToString();
            // _tExpNeeded.text = "Needed: " + neededExperience.ToString();
        }
        private void Start() {
            LoadedDatasOfThePlayer();
        }
        private void Update() {
            if(_calculateGameTime && GameManager.Situation == GameSituation.Play)
            {
                CalculateGameTime();
            }
            if(_sendTrail)
            {
                current = Mathf.MoveTowards(current, target, _trailSpeed * Time.unscaledDeltaTime);

                var trailPos = _starsTrailEffect.transform.position;
                trailPos = Vector3.Lerp(trailPos, _totalStarsImageTransform.position, current);
                _starsTrailEffect.transform.position = trailPos;

                if((trailPos - _totalStarsImageTransform.position).magnitude < 0.1f)
                {
                    _starsPopEffectSecond.transform.position = _totalStarsImageTransform.position;
                    _starsPopEffectSecond.SetActive(true);
                    _tTotalStars.text = VGPGSManager.Instance._playerData.stars.ToString();
                    _sendTrail = false;
                }
            }
        }
        private void LoadedDatasOfThePlayer()
        {
            var _playerData = VGPGSManager.Instance._playerData;

            if(_tLevel != null)
            {
                _tLevel.text = _playerData.level.ToString();
                _tLevelAnimator = _tLevel.GetComponent<Animator>();
            }
            if(_tTotalStars != null)
            _tTotalStars.text = _playerData.stars.ToString();

            // Level XP Bar
            if(_levelSlider != null)
            {
                var fillAmount = Mathf.Clamp01((float)_playerData.currentExperience / _playerData.neededExperience);
                _levelSlider.fillAmount = fillAmount;
            }

            if(_tExpCurrent != null)
                _tExpCurrent.text = "Current: " + _playerData.currentExperience.ToString();
            if(_tExpNeeded != null)
                _tExpNeeded.text = "Needed: " + _playerData.neededExperience.ToString();

            // _starsTrailEffect.transform.position = _starsPopEffect.transform.position = _sessionStarsImageTransform.position;
        }
        private void CalculateGameTime()
        {
            seconds += Time.deltaTime;
            _tTime.text = $"{seconds:000000}";
        }

        /*
        #region Random Params
        private bool _startCor;
        private int baseExperience = 500;
        private int neededExperience = 0;
        private int currentExperience = 0;
        private int level = 1;
        private float experienceMultiplier = 1.2f;

        #endregion

        private void Update() {
            if(!_startCor && Input.GetKeyDown(KeyCode.V))
            {
                _startCor = true;
                currentExperience = 10000;
                StartCoroutine(FillAnimationCorSecond(150, currentExperience, neededExperience));
            }
        }
        */

        #region In Game Updates

        #region Score
        public void UpdateHighestScore(int currentScore)
        {
            var playerData = VGPGSManager.Instance._playerData;

            if(currentScore > playerData.highestScore)
            {
                playerData.highestScore = currentScore;
                _tHighestScore.text = currentScore.ToString();

                VGPGSManager.Instance.OpenSave(true);
                PostScoreToLeaderboard(currentScore);
            }
            else
            {
                _tHighestScore.text = playerData.highestScore.ToString();
                PostScoreToLeaderboard(currentScore);
            }
        }
        private void PostScoreToLeaderboard(int scoreToPost)
        {
            if(FindObjectOfType<VLeaderboard>() != null)
                VLeaderboard.Instance.PostScoreLeaderboard(scoreToPost);
        }
        #endregion

        #region Experience
        public void CalculateExperience(int fruitCutAmount)
        {
            int experience = fruitCutAmount + (int)seconds;

            IncreaseXP(experience);
        }
        private void IncreaseXP(int experience)
        {
            var _playerData = VGPGSManager.Instance._playerData;
            var pastExperience = _playerData.currentExperience;
            _playerData.currentExperience += experience;

            StartCoroutine(FillAnimationCor(pastExperience, _playerData.currentExperience, _playerData.neededExperience));
        }
        IEnumerator FillAnimationCor(int pastExperience, int increasedXP, int neededExperience)
        {
            float currentXP = (float)pastExperience;
            //      100          150             100           100 => elde var 50
            while(currentXP < increasedXP && currentXP < neededExperience)
            {
                currentXP += Time.unscaledDeltaTime * speedController;
                _levelSlider.fillAmount = Mathf.Clamp01((float)currentXP / neededExperience);
                _tExpCurrent.text = "Current: " + ((int)currentXP).ToString();
                yield return null;
            }
            currentXP = increasedXP;

            var _playerData = VGPGSManager.Instance._playerData;

            _tExpNeeded.text = "Needed: " + _playerData.neededExperience.ToString();

            if(currentXP < neededExperience)// XP loaded max
            {
                VGPGSManager.Instance.OpenSave(true);
                _expDone = true;
                CheckEverythingIsDone();
            }
            else
            {
                _playerData.currentExperience = _playerData.currentExperience - _playerData.neededExperience;
                _playerData.level++;
                _tLevel.text = _playerData.level.ToString();
                _tLevelAnimator.SetTrigger("levelPop");
                speedController *= _playerData.experienceMultiplier;

                _playerData.neededExperience = (int)(_playerData.baseExperience * (_playerData.experienceMultiplier * _playerData.level));
                _tExpCurrent.text = "Current: " + _playerData.currentExperience.ToString();
                _tExpNeeded.text = "Needed: " + _playerData.neededExperience.ToString();

                StartCoroutine(FillAnimationCor(0, _playerData.currentExperience, _playerData.neededExperience));
            }
        }
        /*IEnumerator FillAnimationCorSecond(int pastExperience, int increasedXP, int neededExperience)
        {
            float currentXP = (float)pastExperience;
            //      100          150             100           100 => elde var 50
            while(currentXP < increasedXP && currentXP < neededExperience)
            {
                currentXP += Time.unscaledDeltaTime * speedController;
                _levelSlider.fillAmount = Mathf.Clamp01((float)currentXP / neededExperience);
                yield return null;
            }
            currentXP = increasedXP;

            _tExpCurrent.text = "Current: " + currentExperience.ToString();
            _tExpNeeded.text = "Needed: " + neededExperience.ToString();

            if(currentXP < neededExperience)// XP loaded max
            {
                _startCor = false;
            }
            else
            {
                currentExperience = currentExperience - neededExperience;
                level++;
                _tLevel.text = level.ToString();
                _tLevelAnimator.SetTrigger("levelPop");

                neededExperience = (int)(baseExperience * (experienceMultiplier * level));
                speedController *= experienceMultiplier;
                _tExpCurrent.text = "Current: " + currentExperience.ToString();
                _tExpNeeded.text = "Needed: " + neededExperience.ToString();

                StartCoroutine(FillAnimationCorSecond(0, currentExperience, neededExperience));
            }
        }
        */
        #endregion

        #region Stars
        public void CalculateStars(int uniqueFruitAmount/*this could be max 10(there is 10 diff. fruits)*/, int specialFruitAmount, int comboCount)
        {
            int stars = uniqueFruitAmount + specialFruitAmount + comboCount + (int)(seconds * 0.1f);

            StartCoroutine(IncreaseStarsCor(stars));
        }
        IEnumerator IncreaseStarsCor(int sessionStars)
        {
            var _playerData = VGPGSManager.Instance._playerData;

            int stars = 0;

            while(stars < sessionStars)
            {
                stars += 1;
                _tStars.text = "+" + stars.ToString();

                yield return new WaitForSeconds(0.01f);
            }

            _playerData.stars += stars;
            VGPGSManager.Instance.OpenSave(true);

            if(stars > 0)
            {
                _starsPopEffect.SetActive(true);
                _starsTrailEffect.SetActive(true);
                _sendTrail = true;
            }

            _starsDone = true;
            CheckEverythingIsDone();
        }
        #endregion

        #endregion
        public void CheckEverythingIsDone()
        {
            if(_expDone && _starsDone && _scoreDone)
                GameManager.Situation = GameSituation.EverythingDone;
        }
    }
}
