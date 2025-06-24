using UnityEngine;
using TMPro;

public class AssignScoreText : MonoBehaviour
{
    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.scoreText = GetComponent<TMP_Text>();
            ScoreManager.Instance.SendMessage("UpdateHUD");
            Debug.Log("ScoreText assignat manualment via script extern.");
        }
        else
        {
            Debug.LogWarning("ScoreManager.Instance és null.");
        }
    }
}
