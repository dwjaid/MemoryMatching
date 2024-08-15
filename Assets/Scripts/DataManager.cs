using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int gameCount;
    public int lastScore;
    public List<int> topScores;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private GameData gameData;
    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGameData(int score)
    {
        gameData.gameCount++;
        gameData.lastScore = score;

        gameData.topScores.Add(score);
        gameData.topScores.Sort((a, b) => b.CompareTo(a)); 

        SaveGameDataToDisk();
    }

    private void SaveGameDataToDisk()
    {
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
    }

    private void LoadGameData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            gameData = new GameData
            {
                gameCount = 0,
                lastScore = 0,
                topScores = new List<int>()
            };
        }
    }

    public List<int> GetTopScores()
    {
        return gameData.topScores.Count > 10
            ? gameData.topScores.GetRange(0, 10)
            : new List<int>(gameData.topScores);
    }

    public GameData GetGameData()
    {
        if (gameData == null)
        {
            gameData = new GameData
            {
                gameCount = 0,
                lastScore = 0,
                topScores = new List<int>()
            };
        }
        return gameData;
    }
}
