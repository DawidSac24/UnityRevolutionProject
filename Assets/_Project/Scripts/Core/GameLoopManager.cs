
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField]
    public float loopDuration = 60f;

    private float timeRemaining;
    private bool isGameOver = false;

    void Start()
    {
        timeRemaining = loopDuration;
    }

    void Update()
    {
        if (isGameOver)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        isGameOver = true;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }
}
