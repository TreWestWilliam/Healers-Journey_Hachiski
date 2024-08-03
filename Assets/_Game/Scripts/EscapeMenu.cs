using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    public void openMenu()
    {
        gameObject.SetActive(true);
    }

    public void closeMenu()
    {
        gameObject.SetActive(false);
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
