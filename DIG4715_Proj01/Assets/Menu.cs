using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen: MonoBehaviour
{
    public Text winnerText;

    public void Setup(GameWon, Object Object)
    {
        GameObject.SetActive(true);
        winnerText.Text = object.ToString() + "Winner";
    }
    public void MenuButton() {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton() {
        SceneManager.LoadScene("QuitGame");
    }
}