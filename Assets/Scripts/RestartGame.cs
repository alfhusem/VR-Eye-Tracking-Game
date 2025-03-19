using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class RestartGame : MonoBehaviour
{
    public bool IsHovered { get; set; }
    
    [SerializeField] private TextMeshPro restartText; // Assign in inspector
    private float hoverTime = 0f; // Time the object has been hovered over
    private float hoverDuration = 3f; // Duration to look at the object before restarting

    void Start()
    {
        // Initialize the text and make sure it's not active until hovered over
        restartText.text = "Restart";
        restartText.gameObject.SetActive(false);
    }

    void Update() 
    {
        if (IsHovered)
        {
            // Start the countdown when hovered
            restartText.gameObject.SetActive(true);
            hoverTime += Time.deltaTime;
            int countdownTime = Mathf.CeilToInt(hoverDuration - hoverTime);
            restartText.text = $"Restarting... {countdownTime}";

            // Check if the hoverTime exceeds the threshold
            if (hoverTime >= hoverDuration)
            {
                // If so, reset the game
                PlayerHealthManager.Instance.ResetGame();
            }
        }
        else
        {
            // If not hovered, reset the hover time and text
            if (hoverTime > 0)
            {
                hoverTime = 0f;
                restartText.text = "Restart";
                restartText.gameObject.SetActive(false);
            }
        }
    }

    // Optionally, if you need the object to react when the gaze leaves, add this method
    public void OnHoverExit()
    {
        IsHovered = false;
    }
}
