using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool IsPaused;
    [SerializeField] AudioSource audioSource;


    public void PauseGame()
    {
        Time.timeScale = 0f;
        audioSource.Stop();
        IsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }
}
