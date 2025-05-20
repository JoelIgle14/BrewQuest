using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int currentScore = 0;
    public TMP_Text scoreText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddPoints(int amount)
    {
        currentScore += amount;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + currentScore;
        }
    }
}
