using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreLabel;
    public int score;

    private void Awake()
    {
        score = 0;
        UpdateScore();
    }

    public void AddScore(int add)
    {
        score += add;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreLabel.text = "$" + score;
    }
}