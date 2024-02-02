namespace Runtime
{
    [System.Serializable]
    public class VPlayerData {
        public int highestScore;
        public int level;
        public int stars;

        public float experienceMultiplier;
        public int baseExperience;
        public int neededExperience;
        public int currentExperience;

        public int currentBladeIndex;
        public int currentDojoIndex;

        public bool[] UnlockedAchievements;
        public bool[] unlockedBlades;
        public bool[] boughtBlades;
        public bool[] unlockedDojos;
        public bool[] boughtDojos;

        public bool _areMenuTipsDone;
        public bool _arePreGameTipsDone;

        public bool areNewBladesUnlocked;
        public bool areNewDojosUnlocked;

        public float musicVolume;
        public float soundFXVolume;
        public float totalPlayTime;

        #region Achievements
        public bool achivement_sliceFirstFruit;
        public bool achivement_fruitSalad;
        public bool achivement_tastyQuadro;
        public bool achivement_comboBeginner;
        public bool achivement_berryFan;
        public bool achivement_pulpFiction;
        public bool achivement_sliceMaster;
        public bool achivement_fruitNinjaApprentice;
        public bool achivement_orangeBlitz;
        public bool achivement_comboProdigy;
        public bool achivement_fruitNinjaMaster;
        public bool achivement_comboVirtuoso;
        public bool achivement_fruitExtravaganza;
        public bool achivement_fruitNinjaLegend;
        #endregion

        public VPlayerData() {
            highestScore = 0;
            level = 1;
            stars = 500;

            experienceMultiplier = 1.2f;
            baseExperience = 80;
            neededExperience = (int)(baseExperience * (experienceMultiplier * level));
            currentExperience = 0;

            currentBladeIndex = 0;
            currentDojoIndex = 0;

            UnlockedAchievements = new bool[20];
            unlockedBlades = new bool[20];
            unlockedDojos = new bool[20];
            boughtBlades = new bool[20];
            boughtDojos = new bool[20];

            unlockedBlades[0] = true;
            boughtBlades[0] = true;
            unlockedDojos[0] = true;
            boughtDojos[0] = true;

            _areMenuTipsDone = false;
            _arePreGameTipsDone = false;

            areNewBladesUnlocked = false;
            areNewDojosUnlocked = false;

            musicVolume = 0f;
            soundFXVolume = 0f;

            totalPlayTime = 0f;
        }
    }

}
