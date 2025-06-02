using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int currentScore = 0;
    public TMP_Text scoreText;
    private int checkpointScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // <- Manté l'objecte entre escenes
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    public void SaveCheckpointScore()
    {
        checkpointScore = currentScore;
    }

    public void ResetScore(bool hasCheckpoint)
    {
        currentScore = hasCheckpoint ? checkpointScore : 0;
        UpdateHUD();
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

void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Debug.Log("Escena carregada: " + scene.name);
    
    if (scoreText == null)
    {
scoreText = GameObject.Find("Score")?.GetComponent<TMP_Text>();
        Debug.Log("Assignat scoreText? " + (scoreText != null));
    }

    UpdateHUD();
}


}
