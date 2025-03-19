using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }

    void Awake()
    {
        // Ensure there's only one instance of the ScoreManager in the scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
        // You can also call your UI update method here if you prefer
        UIManager.Instance.UpdateScoreUI(Score);
    }
}

