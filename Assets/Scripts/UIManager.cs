using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public UIManager Instance { get; private set; }

    public Text gameCountText;
    public Text lastScoreText;
    public Text topScoresText;


    private void UpdateUI()
    {
        if (gameCountText == null || lastScoreText == null || topScoresText == null)
        {
            return;
        }

        GameData gameData = DataManager.Instance?.GetGameData();

        if (gameData == null)
        {
            return;
        }
        gameCountText.text = $"�������: {gameData.gameCount}";
        lastScoreText.text = $"�ϴη���: {gameData.lastScore}";

        topScoresText.text = "��߷���:\n";
        for (int i = 0; i < gameData.topScores.Count; i++)
        {
            Debug.Log(gameData.topScores[i]);
            topScoresText.text += $"{i + 1}. {gameData.topScores[i]}\n";
        }
        topScoresText.text = topScoresText.text.Replace("\\n", "\n");
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        UpdateUI();
    }


    public void ShowGamePage()
    {
        SceneManager.LoadScene(1);
    }
    public void ShowMainPage()
    {
        SceneManager.LoadScene(0);
    }
}
