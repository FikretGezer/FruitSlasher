using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Runtime
{
    public class SavedGamesUI : MonoBehaviour
    {
        public string name;
        public int age;
        public TMP_Text logTxt;
        public TMP_Text outputTxt;
        public TMP_InputField nameInputField;
        public TMP_InputField ageInputField;
        public PlayerData _playerData;

        public void OnvalueChangeName(string _field)
        {
            name = nameInputField.text;
        }
        public void OnvalueChangeAge(string _field)
        {
            age = int.Parse(ageInputField.text);
            // _playerData.highestScore = int.Parse(ageInputField.text);
        }
        public void IncreaseScore()
        {
            var score = _playerData.highestScore;
            score += 10;
            _playerData.highestScore = score;
            GPGSManager.Instance.OpenSave(true);
        }
        private void OnDisable() {
            _playerData.level++;
            GPGSManager.Instance.OpenSave(true);
        }
	private void OnApplicationQuit() {
            _playerData.level++;
            GPGSManager.Instance.OpenSave(true);
        }
    }
}
