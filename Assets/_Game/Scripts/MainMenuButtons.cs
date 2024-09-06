using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    private float timer = 0;
    [SerializeField] private float titleFadeTime;
    [SerializeField] private float titleFadeLength;
    [SerializeField] private float buttonFadeTime;
    [SerializeField] private float buttonFadeLength;

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject buttons;
    private void Update()
    {
        Color titleColor = title.GetComponent<TMP_Text>().color;
        titleColor.a = Mathf.Min(Mathf.Max(timer - titleFadeTime, 0) / titleFadeLength, 1);
        title.GetComponent<TMP_Text>().color = titleColor;

        foreach (Transform child in buttons.transform)
        {
            Color buttonColor = child.GetComponent<Button>().colors.normalColor;
            buttonColor.a = Mathf.Min(Mathf.Max(timer - buttonFadeTime, 0) / buttonFadeLength, 1);
        }

        timer += Time.deltaTime;
    }

    public void NewGame()
    {
        SceneManager.LoadScene("GameArea_1");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
