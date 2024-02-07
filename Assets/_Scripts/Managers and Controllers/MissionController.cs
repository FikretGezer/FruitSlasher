using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        public void SetMissions()
        {
            if(CheckIsAllCompleted())
                SetNewMissions();
            else
                SetPreviousMissions();
        }
        private void SetNewMissions()
        {
            var missions = new List<Mission>();
            int count = 0;

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
                    // Reset values
                    currentMission.currentAmount = 0;
                    currentMission.completed = false;

                    // Show selected mission on the board
                    missionContainer[i].image.sprite = currentMission.sprite;
                    missionContainer[i].explanationT.text = currentMission.explanation;
                    missionContainer[i].pointsT.text = "+" + currentMission.points.ToString();
                    missionContainer[i].progressT.text = $"{currentMission.currentAmount} / {currentMission.requiredAmount}";
                    missionContainer[i].completedLine.SetActive(false);

                    missions.Add(currentMission);
                }
            }

            // if(_selectedMissionsScriptable.selectedMissions.Count > 0)
            // {
            //     foreach(var miss in _selectedMissionsScriptable.selectedMissions)
            //     {
            //         miss.completed = false;
            //     }
            // }
            if(VGPGSManager.Instance._playerData.selectedMissions.Count > 0)
            {
                foreach(var miss in VGPGSManager.Instance._playerData.selectedMissions)
                {
                    miss.completed = false;
                }
            }
            // _selectedMissionsScriptable.selectedMissions.Clear();
            // _selectedMissionsScriptable.selectedMissions.AddRange(missions);

            VGPGSManager.Instance._playerData.selectedMissions.Clear();
            VGPGSManager.Instance._playerData.selectedMissions.AddRange(missions);
        }
        private void SetPreviousMissions()
        {
            if(missionContainer.Length >= 3)
            {
                int containerIndex = 0;
                foreach (var miss in VGPGSManager.Instance._playerData.selectedMissions)
                {
                    missionContainer[containerIndex].image.sprite = _selectedMissionsScriptable.missions[miss.missionIndex].sprite;
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
            // if(_selectedMissionsScriptable.selectedMissions.Count > 0)
            // {
            //     foreach(var miss in _selectedMissionsScriptable.selectedMissions)
            //     {
            //         if(!miss.completed)
            //         {
            //             return false;
            //         }
            //     }
            // }
            if(VGPGSManager.Instance._playerData.selectedMissions.Count > 0)
            {
                foreach(var miss in VGPGSManager.Instance._playerData.selectedMissions)
                {
                    if(!miss.completed)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
