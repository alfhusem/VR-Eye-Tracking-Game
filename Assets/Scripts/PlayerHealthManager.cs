using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance { get; private set; }
    public int lives = 5;
    public Image damageImage; // Assign a UI Image with a red sprite in the inspector
    public TextMeshProUGUI gameOverText; // Assign in inspector
    public TextMeshProUGUI scoreText; // Assign in inspector
    private bool isDead = false;
    private float lastDamageTime = -1; // Initialize to a negative value
    private bool isRegenerating = false;
    public float healthRegenerationCooldown = 3f; // Time in seconds before health starts regenerating
    public float healthRegenerationRate = 1f; // Time in seconds between each health point regeneration
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        gameOverText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    void Start()
    {

        // Start with full lives
        lives = 5;
        StartCoroutine(HealthRegeneration());
    }

    public void AddScore(int amount)
    {
        if (!isDead) // Only add score if not dead
        {
            score += amount;
        }
    }

    public void TakeDamage()
    {
        if (isDead)
            return;

        lives--;
        lastDamageTime = Time.time;
        isRegenerating = false; // Stop regeneration when taking damage
        StopAllCoroutines(); // Stop all coroutines, including any ongoing regeneration
        StartCoroutine(ShowDamageEffect());

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            // Start the regeneration coroutine if it's not already running
            StartCoroutine(HealthRegeneration());
        }
    }

    private IEnumerator HealthRegeneration()
    {
        // Wait for the cooldown before starting to regenerate health
        yield return new WaitForSeconds(healthRegenerationCooldown);
        isRegenerating = true;

        // Start regenerating health every second
        while (lives < 5 && isRegenerating)
        {
            // Wait for the specified rate before adding a life
            yield return new WaitForSeconds(healthRegenerationRate);
            
            // Only add a life if enough time has passed since the last damage was taken
            if (Time.time - lastDamageTime >= healthRegenerationCooldown)
            {
                lives++;
                // Update UI here if necessary
            }
        }
    }

    private IEnumerator ShowDamageEffect()
    {
        // Set the alpha to full to show the damage effect
        damageImage.color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.5f); // Duration of the damage effect
        // Fade out the damage effect
        damageImage.color = new Color(1, 0, 0, 0);
    }

    private void GameOver()
    {
        isDead = true;
        gameOverText.gameObject.SetActive(true);
        scoreText.text = "Score: " + score; // Set the score text
        scoreText.color = Color.white;
        scoreText.gameObject.SetActive(true); // Show the score text
        // Optionally, stop the game
        Time.timeScale = 0;
    }

    // Call this method to reset the game
    public void ResetGame()
    {
        // Reset game state
        isDead = false;
        lives = 5;
        score = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Start regeneration again
        StartCoroutine(HealthRegeneration());
    }
}
