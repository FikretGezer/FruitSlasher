using UnityEngine;

namespace Runtime
{
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
        public void BuyBlade()
        {
            CompleteCheck(MissionType.BuyBlade);
            MissionController.Instance.SetMissions();
        }
        public void BuyDojo()
        {
            CompleteCheck(MissionType.BuyDojo);
            MissionController.Instance.SetMissions();
        }
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
                MenuUI.Instance.SetStars();
                NotificationController.Instance.EnqueueNotification(sprite, explanation, points);
            }
        }
        #endregion
    }
}
