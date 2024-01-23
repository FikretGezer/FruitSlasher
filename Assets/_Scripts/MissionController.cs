using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace Runtime
{
    public class MissionController : MonoBehaviour
    {
        [SerializeField] private MissionContainer[] missionContainer = new MissionContainer[3];
        [SerializeField] private bool isThereABoard;
        public SelectedMissionsScriptable _selectedMissionsScriptable;
        public bool checkMissions;


        public static MissionController Instance;
        private void Awake() {
            if(Instance == null) Instance = this;

            if(isThereABoard)
                SetMissions();
        }
        private void Update() {
            if(checkMissions)
            {
                checkMissions = false;
                SetMissions();
            }
        }

        private void SetMissions()
        {
            var missions = new List<Mission>();
            int count = 0;

            if(CheckIsAllCompleted())
            {
                if(missionContainer.Length >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        // Select a random mission
                        var rnd = Random.Range(0, _selectedMissionsScriptable.missions.Count);
                        var currentMission = _selectedMissionsScriptable.missions[rnd];
                        count = 0;

                        if(missions.Count > 0)
                        {
                            while(CheckSimilarityInMissions(currentMission, missions) && count < 10)
                            {
                                rnd = Random.Range(0, _selectedMissionsScriptable.missions.Count);
                                currentMission = _selectedMissionsScriptable.missions[rnd];

                                count++;
                            }
                        }

                        // Show selected mission on the board
                        missionContainer[i].image.sprite = currentMission.sprite;
                        missionContainer[i].explanationT.text = currentMission.explanation;
                        missionContainer[i].pointsT.text = "+" + currentMission.points.ToString();
                        missionContainer[i].progressT.text = $"{currentMission.currentAmount} / {currentMission.requiredAmount}";
                        missionContainer[i].completedLine.SetActive(false);

                        missions.Add(currentMission);
                    }
                }

                if(_selectedMissionsScriptable.selectedMissions.Count > 0)
                {
                    foreach(var miss in _selectedMissionsScriptable.selectedMissions)
                    {
                        miss.completed = false;
                    }
                }
                _selectedMissionsScriptable.selectedMissions.Clear();
                _selectedMissionsScriptable.selectedMissions.AddRange(missions);
            }
            else // Set previous missions
            {
                if(missionContainer.Length >= 3)
                {
                    int containerIndex = 0;
                    foreach (var miss in _selectedMissionsScriptable.selectedMissions)
                    {
                        missionContainer[containerIndex].image.sprite = miss.sprite;
                        missionContainer[containerIndex].explanationT.text = miss.explanation;
                        missionContainer[containerIndex].pointsT.text = "+" + miss.points.ToString();
                        missionContainer[containerIndex].progressT.text = $"{miss.currentAmount} / {miss.requiredAmount}";

                        if(miss.completed)
                            missionContainer[containerIndex].completedLine.SetActive(true);
                        else
                            missionContainer[containerIndex].completedLine.SetActive(false);

                        containerIndex++;
                    }
                }
            }
        }
        private bool CheckSimilarityInMissions(Mission currentMission, List<Mission> missions)
        {
            foreach(var mission in missions)
            {
                if(mission.explanation == currentMission.explanation)
                    return true;
            }
            return false;
        }
        private bool CheckIsAllCompleted()
        {
            if(_selectedMissionsScriptable.selectedMissions.Count > 0)
            {
                foreach(var miss in _selectedMissionsScriptable.selectedMissions)
                {
                    if(!miss.completed)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // public void SetMissionsButton()
        // {
        //     SetMissions();
        // }
        // public void SetCompleted()
        // {
        //     if(selectedMissions.Count > 0)
        //     {
        //         foreach(var miss in selectedMissions)
        //         {
        //             if(miss.completed)
        //             {
        //                 miss.line.SetActive(true);
        //             }
        //         }
        //     }
        // }
    }

    [System.Serializable]
    public class MissionContainer
    {
        public Image image;
        public TMP_Text explanationT;
        public TMP_Text pointsT;
        public TMP_Text progressT;
        public GameObject completedLine;
    }

    [System.Serializable]
    public class Mission
    {
        public MissionType type;

        [Header("Board Parameters")]
        public Sprite sprite;
        public string explanation;
        public int points;

        [Header("Goal Progress")]
        public int requiredAmount;
        public int currentAmount;
        public bool completed;

        public bool IsReached()
        {
            return currentAmount >= requiredAmount;
        }

        #region Increment Functions
        public void CutFruit() => CompleteCheck(MissionType.CutFruit);
        public void CutBomb() => CompleteCheck(MissionType.CutBomb);
        public void PlayTheGame() => CompleteCheck(MissionType.Play);
        public void BuyBlade() => CompleteCheck(MissionType.BuyBlade);
        public void BuyDojo() => CompleteCheck(MissionType.BuyDojo);
        private void CompleteCheck(MissionType missionType)
        {
            if(!IsReached())
            {
                if(type == missionType && !IsReached()) currentAmount++;
            }
            if(IsReached() && !completed)
            {
                completed = true;
                VGPGSManager.Instance._playerData.stars += points;
            }
        }
        #endregion
    }
    public enum MissionType
    {
        CutFruit,
        CutBomb,
        Play,
        BuyBlade,
        BuyDojo
    }
}
