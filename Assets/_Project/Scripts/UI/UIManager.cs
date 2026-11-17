
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameLoopManager gameLoopManager;
    [SerializeField]
    private TMP_Text timerText;

    void Update()
    {
        if (gameLoopManager == null || timerText == null)
            return;

        float timeLeft = gameLoopManager.GetTimeRemaining();

        timerText.text = Mathf.CeilToInt(timeLeft).ToString(); // arround the time to the upper int and into a string 
    }
}
