using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int rows = 4;
    public int columns = 4;
    public float tileSpacing = 1.1f;
    public Transform canvas;
    public GameObject[] tileList;
    public GameObject gameOverPanel;
    public GameObject tilesPanel;
    public GameObject backToMenuBtn;
    public Text gameOverText;
    public Text scoreText;
    public Text timeText;
    private Tile firstTile;
    private Tile secondTile;
    private List<GameObject> tiles = new List<GameObject>();
    private float gameTime = 0f;
    [SerializeField]
    private float totalTime = 60f;
    private float remaingTime = 0f;
    private int matchedPairs = 0;
    private int score = 0;
    [SerializeField]
    private int getScore = 5;
    [SerializeField]
    private int loseScore = 1;
    private int totalPairs;
    private bool gameOver = false;
    private bool isChecking = false;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        timeText.text = $"剩余时间: {totalTime:F2} 秒";
        scoreText.text = $"当前分数: {0:D2}";
    }
    
    public bool GetCheckStatus()
    {
        return isChecking;
    }

    public void TileClicked(Tile clickedTile)
    {
        
        if (firstTile == null)
        {
            firstTile = clickedTile;
        }
        else if (secondTile == null)
        {
            secondTile = clickedTile;
            isChecking = true;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (firstTile.icon == secondTile.icon)
        {
            score += getScore;
            matchedPairs++;
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            score -= loseScore;
            firstTile.Flip();
            secondTile.Flip();
        }
        scoreText.text = $"当前分数: {score:D2}";
        firstTile = null;
        secondTile = null;
        isChecking = false;
        if (matchedPairs == totalPairs)
        {
            GameOver();
        }
    }

    void Start()
    {
        CreateGameBoard();
        totalPairs = (rows * columns) / 2; 
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (gameOver == true) return;
        gameTime += Time.deltaTime;
        remaingTime = totalTime - gameTime;
        if (remaingTime <= 0)
        {
            remaingTime = 0;
            gameTime = totalTime;
            GameOver();
        }
        timeText.text = $"剩余时间: {remaingTime:F2} 秒";
    }


    private void CreateGameBoard()
    {
        float startX = -(columns / 2f) * tileSpacing / (columns / 2f) + tileSpacing / 2f;
        float startY = -(rows / 2f) * tileSpacing / (rows / 2f) + tileSpacing / 2f;
        float scaleX = 2f / columns;
        float scaleY = 2f / rows;

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; j += 2)
            {
                GameObject temp = tileList[Random.Range(0, tileList.Length)];
                tiles.Add(temp);
                tiles.Add(temp);
            }
        }
        for (int i = 0; i < rows * columns; ++i)
        {
            int x1 = Random.Range(0, columns);
            int y1 = Random.Range(0, rows);
            int x2 = Random.Range(0, columns);
            int y2 = Random.Range(0, rows);
            if (x1 != x2 && y1 != y2)
            {
                GameObject temp = tiles[columns * y1 + x1];
                tiles[columns * y1 + x1] = tiles[columns * y2 + x2];
                tiles[columns * y2 + x2] = temp;
            }
        }
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                GameObject temp = Instantiate(tiles[columns * i + j], canvas);
                temp.transform.localPosition = new Vector3(startX + j * tileSpacing / (columns / 2f), startY + i * tileSpacing / (rows / 2f), 0);
                temp.transform.localScale = new Vector3(scaleX, scaleY, 0);
            }
        }
    }
    private void GameOver()
    {
        if (gameOver == true) return;
        gameOver = true;
        timeText.text = $"剩余时间: {remaingTime:F2} 秒";


        tilesPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        backToMenuBtn.SetActive(true);
        gameOverText.text = $"游戏结束!\n用时: {gameTime:F2} 秒\n成功匹配: {matchedPairs} 对";
        gameOverText.text = gameOverText.text.Replace("\\n", "\n");

        DataManager.Instance.SaveGameData(score);


    }

}
