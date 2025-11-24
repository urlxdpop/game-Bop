namespace SaveData {

    [System.Serializable]
    public class GameData {
        public string sceneName;
        public bool[] levelOpened;
        public int[] minTimeForLevel;
        public int timeForAllLevels;

        public GameData() {
            sceneName = "1-1";
            levelOpened = new bool[100];
            minTimeForLevel = new int[100];
            timeForAllLevels = 0;

            levelOpened[0] = true;
        }
    }

    [System.Serializable]
    public class SecretsData {
        public string[] isSecretOpen;
        public int[] numSecretsInLevel;
        public bool[] savingData;

        public SecretsData() {
            numSecretsInLevel = new int[100];
            savingData = new bool[100];
            isSecretOpen = new string[100];
            for (int i = 0; i < 100; i++) {
                isSecretOpen[i] = "";
            }
        }
    }
}
