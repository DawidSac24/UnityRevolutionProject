using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgainController : MonoBehaviour
{
       public void ReStartGame()
    {
        SceneManager.LoadScene("Mitica_Test");
    }
}
