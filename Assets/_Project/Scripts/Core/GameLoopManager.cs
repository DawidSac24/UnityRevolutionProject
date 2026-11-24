using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private float loopDuration = 60f;

    private float timeRemaining;
    private bool _isTimerActive;
    private int _timerStep = 1;
    private bool isGameOver = false;

    private void Start()
    {
        timeRemaining = loopDuration;
        _isTimerActive = true;
    }

    private void Update()
    {
        if (isGameOver)
            return;

        TickTimer();
    }

    private void GameOver()
    {
        isGameOver = true;

        Cursor.lockState = CursorLockMode.None; // unlocking the Cursor
        Cursor.visible = true;
        SceneManager.LoadScene("DeathScreen");
    }

    private void TickTimer()
    {
        if (!_isTimerActive) return;

        timeRemaining -= _timerStep * Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            GameOver();
        }
    }

    public void ToggleTimer()
    {
        _isTimerActive = !_isTimerActive;
    }

    public void ReverseTimer()
    {
        _timerStep -= _timerStep * 2;
        Debug.Log(_timerStep);
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }
}