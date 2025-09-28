using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    public int score = 0;

    public TextMeshProUGUI mainScoreText;
    public TextMeshProUGUI finalScoreText;
    public GridManager gridManager;
    
    private void OnEnable()
    {
        GameManager.Instance.onCompleteTurn.AddListener(AddScore);
    }

    private void OnDisable()
    {
        GameManager.Instance.onCompleteTurn.RemoveListener(AddScore);
    }
    
    private void Awake()
    {
        Instance = this;
    }
    
    public GameObject gameOverPanel;
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = score.ToString();
    }

    public void AddScore()
    {
        score++;
        mainScoreText.text = score.ToString();
        
        // if score is divisible by 10, reduce enemy spawn CD (for progression)
        if (gridManager.enemySpawnCD > 1 && score % 10 == 0)
        {
            gridManager.enemySpawnCD--;
        }
    }

    public void RestartGame()
    {
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
