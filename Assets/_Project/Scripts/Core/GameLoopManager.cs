
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private float loopDuration = 60f;

    private float timeRemaining;
    private bool isGameOver = false;

    private void Start()
    {
        timeRemaining = loopDuration;
    }

    private void Update()
    {
        if (isGameOver)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;

        Cursor.lockState = CursorLockMode.None; // unlocking the Cursor
        Cursor.visible = true;
        SceneManager.LoadScene("DeathScreen");
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }
}
