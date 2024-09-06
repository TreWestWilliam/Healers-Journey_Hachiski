using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    public void openMenu()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void closeMenu()
    {
        gameObject.SetActive(false);
        SettingsManager.SaveSettings();
        Time.timeScale = 1f;
    }

    public void quitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
