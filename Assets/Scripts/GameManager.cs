using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;  // Required for IEnumerator

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject startMenu;          // UI for the start menu
    public GameObject gameOverPanel;      // UI panel shown when the game is over
    public TextMeshProUGUI timerText;     // TextMeshPro element to display the timer
    public TextMeshProUGUI outcomeText;   // TextMeshPro element to display the outcome message
    public TextMeshProUGUI timeTakenText; // TextMeshPro element to display the time taken to reach the goal

    [Header("Gameplay Components")]
    public Transform player;              // Assign the player transform
    public GameObject character;
    public Transform goal;                // Assign the goal transform
    public PlayerMovement playerMovement;
    public PlayerInput playerInput;
    public MonoBehaviour cameraFollow;    // Reference to camera follow script

    [Header("Game Settings")]
    public float timeLeft = 60f;          // Duration of the game in seconds
    public float requiredDistance = 5f;   // Distance to goal to consider "reached"
    private bool gameActive = false;      // Flag to control the game state
    private float startTime;              // Start time to calculate time taken

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        gameOverPanel.SetActive(false);
        DisableGameplayComponents();
    }

    void Update()
    {
        if (gameActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimerDisplay();
                CheckIfGoalReached();
            }
            else
            {
                EndTimeTrial(false);
            }
        }
    }

    public void PlayerReachedGoal()
    {
        EndTimeTrial(true);
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        gameActive = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        timeLeft = 90f;
        startTime = Time.time;

        EnableGameplayComponents();
    }

    private void CheckIfGoalReached()
    {
        if (Vector3.Distance(player.position, goal.position) <= requiredDistance)
        {
            EndTimeTrial(true);
        }
    }

    private void EndTimeTrial(bool success)
    {
        gameActive = false;
        gameOverPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        character.SetActive(false);  // Deactivate player GameObject


        if (success)
        {
            float timeTaken = Time.time - startTime;
            outcomeText.text = "Congratulations! You reached the goal!";
            timeTakenText.text = "Time Taken: " + timeTaken.ToString("0.00") + " seconds";
            timeTakenText.gameObject.SetActive(true);  // Show the time taken
        }
        else
        {
            outcomeText.text = "Time's up! Game Over!";
            timeTakenText.gameObject.SetActive(false);  // Hide the time taken
        }

        DisableGameplayComponents();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the current scene
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EnableGameplayComponents()
    {
        playerMovement.enabled = true;
        playerInput.enabled = true;
        cameraFollow.enabled = true;
    }

    private void DisableGameplayComponents()
    {
        playerMovement.enabled = false;
        playerInput.enabled = false;
        cameraFollow.enabled = false;
    }
}