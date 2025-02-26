using UnityEngine;
using TMPro; // Add this for Text Mesh Pro
using UnityEngine.SceneManagement;

public class TimeTrialManager : MonoBehaviour
{
    public float timeLeft = 60f; // Set this to whatever time limit you want
    public TextMeshProUGUI timerText; // Change this to TextMeshProUGUI for UI Text Mesh Pro
    public Transform player; // Assign the player transform
    public Transform goal; // Assign the goal transform
    public float requiredDistance = 5f; // Distance to goal to consider "reached"

    private void Update()
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

    private void UpdateTimerDisplay()
    {
        timerText.text = "Time Left: " + timeLeft.ToString("0");
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
        if (success)
        {
            timerText.text = "You reached the goal!";
            // Implement what happens if the player wins
        }
        else
        {
            timerText.text = "Time's up!";
            // Implement what happens if the player loses
        }
        // Optionally, restart the level or go to a different scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}