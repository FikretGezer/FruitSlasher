using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace Runtime
{
    public class VSavedGamesUI : MonoBehaviour
    {
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


        public bool _scoreDone;
        private Animator _tLevelAnimator;
        private bool _expDone;
        private bool _starsDone;


        // Star Trail
        private bool _sendTrail;
        private float current = 0f, target = 1f;
        [SerializeField] private float _trailSpeed = 1f;

        [Header("Reward Animation")]
        [SerializeField] private GameObject _rewardsBoard;
        [SerializeField] private TMP_Text _tCurrentLevel;
        [SerializeField] private TMP_Text _tStarsReward;
        [SerializeField] private AnimationClip closingRewardClip;
        [SerializeField] private AnimationClip closingRewardLongClip;
        [SerializeField] private AnimationClip rewardItemsClip;

        [SerializeField] private Transform rewardItemContainer;
        [SerializeField] private GameObject rewardItemPrefab;

        [SerializeField] private BladesHolder _bladesHolder;
        [SerializeField] private DojosHolder _dojosHolder;

        [SerializeField] private UIRaycaster _mainSceneCaster;

        private Animator _rewardsContainerAnim;
        private bool areItemsAdded;
        private bool _continueClicked;
        private bool _rewardItemsExist;
        private bool isTrailSent;

        public static VSavedGamesUI Instance;
        private void Awake() {
            if(Instance == null) Instance = this;

            if(_rewardsBoard != null)
                _rewardsContainerAnim = _rewardsBoard.transform.GetChild(0).GetComponent<Animator>();
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
                    isTrailSent = true;
                }
            }
        }
        private void LoadedDatasOfThePlayer()
        {
            var _playerData = VGPGSManager.Instance._playerData;

            if(_tHighestScore != null)
                _tHighestScore.text = _playerData.highestScore.ToString();

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

        }
        private void CalculateGameTime()
        {
            seconds += Time.deltaTime;
            _tTime.text = $"{seconds:000000}";
        }

        #region In Game Updates

        #region Score
        public void UpdateHighestScore(int currentScore)
        {
            var playerData = VGPGSManager.Instance._playerData;

            if(currentScore > playerData.highestScore)
            {
                playerData.highestScore = currentScore;
                _tHighestScore.text = currentScore.ToString();


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

                _expDone = true;
                _mainSceneCaster.enabled = true;
                CheckEverythingIsDone();
            }
            else
            {
                _mainSceneCaster.enabled = false;
                _playerData.currentExperience = _playerData.currentExperience - _playerData.neededExperience;
                _playerData.level++;
                _tLevel.text = _playerData.level.ToString();
                _tLevelAnimator.SetTrigger("levelPop");
                speedController *= _playerData.experienceMultiplier;

                _playerData.neededExperience = (int)(_playerData.baseExperience * (_playerData.experienceMultiplier * _playerData.level));
                _levelSlider.fillAmount = 0f;
                _tExpCurrent.text = "Current: " + _playerData.currentExperience.ToString();
                _tExpNeeded.text = "Needed: " + _playerData.neededExperience.ToString();

                if(_rewardsBoard != null)
                {
                    _rewardsBoard.SetActive(true);
                    _tCurrentLevel.text = _playerData.level.ToString();
                    _tStarsReward.text = GenerateLevelUpCoinReward(_playerData);
                    yield return new WaitUntil(() => isTrailSent);

                    _rewardsContainerAnim.SetTrigger("triggerRewards");

                    AddItemsToRewards(_playerData);
                    yield return new WaitUntil(() => areItemsAdded);

                    if(_rewardItemsExist)
                    {
                        yield return new WaitForSeconds(1f);
                        _rewardsContainerAnim.SetBool("showItems", true);
                        yield return new WaitForSeconds(rewardItemsClip.length);
                    }

                    yield return new WaitUntil(() => _continueClicked);

                    if(_rewardItemsExist)
                    {
                        _rewardsContainerAnim.SetBool("showItems", false);
                        yield return new WaitForSeconds(closingRewardLongClip.length);
                    }
                    else
                    {
                        _rewardsContainerAnim.SetTrigger("closeRewards");
                        yield return new WaitForSeconds(closingRewardClip.length);
                    }


                    _rewardsBoard.SetActive(false);
                    _continueClicked = false;
                }

                StartCoroutine(FillAnimationCor(0, _playerData.currentExperience, _playerData.neededExperience));
            }
        }

        private string GenerateLevelUpCoinReward(VPlayerData _pData)
        {
            int reward = Random.Range(15, 21) * 10;
            _pData.stars += reward;
            return reward.ToString();
        }
        private void AddItemsToRewards(VPlayerData _pData)// This is gonna work on each level up
        {
            // Add Unlocked Items' Sprite to A List
            var spriteList = new List<Sprite>();
            foreach(var blade in _bladesHolder.blades)
            {
                if(blade.bladeLevel == _pData.level)
                {
                    spriteList.Add(blade.bladeSprite);
                }
            }
            foreach(var dojo in _dojosHolder.dojos)
            {
                if(dojo.dojoLevel == _pData.level)
                {
                    spriteList.Add(dojo.dojoSprite);
                }
            }
            // Instantiate Items for Rewards Container
            if(spriteList.Count > 0)
            {
                _rewardItemsExist = true;
                foreach(var sprite in spriteList)
                {
                    var item = Instantiate(rewardItemPrefab);
                    item.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
                    item.transform.SetParent(rewardItemContainer);
                    item.transform.localScale = Vector3.one;
                }
            }
            else
                _rewardItemsExist = false;

            areItemsAdded = true;
        }
        public void ClickContinue()
        {
            _continueClicked = true;
        }
        #endregion

        #region Stars
        private int sessionStars = 0;
        public void CalculateStars(int uniqueFruitAmount/*this could be max 10(there is 10 diff. fruits)*/, int specialFruitAmount, int comboCount)
        {
            int stars = uniqueFruitAmount + specialFruitAmount + comboCount + (int)(seconds * 0.1f);
            sessionStars = stars;

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
            {
                GameManager.Situation = GameSituation.EverythingDone;
                EventManager.Broadcasting(GameEvents.OnEndGameUIUpdateFinish);
            }
        }
    }
}
