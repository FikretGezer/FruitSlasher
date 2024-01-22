using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class MissionController : MonoBehaviour
    {
        [SerializeField] private MissionContainer[] missionContainer = new MissionContainer[3];
        [SerializeField] private SelectedMissionsScriptable _selectedMissionsScriptable;

        private void Awake() {
            SetMissions();
        }

        private void SetMissions()
        {
            var missions = new List<Mission>();

            if(CheckIsAllCompleted())
            {
                if(missionContainer.Length >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        // Select a random mission
                        var rnd = Random.Range(0, _selectedMissionsScriptable.missions.Count);
                        var currentMission = _selectedMissionsScriptable.missions[rnd];

                        // Show selected mission on the board
                        missionContainer[i].image.sprite = currentMission.sprite;
                        missionContainer[i].explanationT.text = currentMission.explanation;
                        missionContainer[i].pointsT.text = "+" + currentMission.points.ToString();
                        missionContainer[i].completedLine.SetActive(false);

                        // Make sure it is selected and not be selectable over and over again
                        currentMission.line = missionContainer[i].completedLine;

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

                        if(miss.completed)
                            missionContainer[containerIndex].completedLine.SetActive(true);
                        else
                            missionContainer[containerIndex].completedLine.SetActive(false);

                        containerIndex++;
                    }
                }
            }
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
        public GameObject completedLine;
    }

    [System.Serializable]
    public class Mission
    {
        public Sprite sprite;
        public string explanation;
        public int points;
        public bool completed;
        [HideInInspector] public GameObject line;
    }
}
