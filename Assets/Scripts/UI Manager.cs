using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    public int score = 0;
    public int highScore = 0;
    public TextMeshProUGUI mainScoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public GridManager gridManager;
    public GameObject inputBlocker;

    private int blockerCounter = 0;
    
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
        highScore = PlayerPrefs.GetInt("highScore", 0);
        gameOverPanel.SetActive(true);
        finalScoreText.text = score.ToString();
        if (score >= highScore)
        {
            highScore = score;
            highScoreText.text = highScore.ToString();
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        mainScoreText.text = score.ToString();
        mainScoreText.rectTransform.DOShakeAnchorPos(0.5f, strength: new Vector2(10, 10), vibrato: 20, randomness: 90, snapping: false, fadeOut: true);
        /*
        if (scoreToAdd < 0 && Camera.main != null)
                Camera.main.transform.DOShakePosition(0.5f, strength: new Vector3(0.5f, 0.5f, 0f), vibrato: 20, randomness: 90, fadeOut: true);
                */

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

    public void BlockInput()
    {
        blockerCounter++;
        inputBlocker.SetActive(true);
    }

    public void UnblockInput()
    {
        blockerCounter--;
        if (blockerCounter == 0) inputBlocker.SetActive(false);
    }
}
