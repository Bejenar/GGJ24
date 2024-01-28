using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreLabel;
    public int score;

    public TextMeshProUGUI animatedLabel;
    public ScoreAnimator _scoreAnimator;
    private void Awake()
    {
        score = 0;
        UpdateScore();
    }

    public void AddScore(int add)
    {
        score += add;
        animatedLabel.text = $"+{add}";
        _scoreAnimator.Pop();
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreLabel.text = "$" + score;
        
    }
}