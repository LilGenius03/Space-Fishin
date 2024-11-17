using UnityEngine;
using UnityEngine.SceneManagement;

public class yum : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Leave()
    {
        Application.Quit();
    }
}
