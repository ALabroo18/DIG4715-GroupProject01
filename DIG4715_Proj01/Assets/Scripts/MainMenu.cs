using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        // Limit frame rate to the refresh rate of the monitor.
        QualitySettings.vSyncCount = 1;
    }

    private void Update()
    {
       
    }

    // Load the game scene with a snow biome.
    public void PlayControls()
    {
        // Set chosen level, number of o
        LoadScene("control page");
        
    }

    // Load the game scene with a grassland biome.
    public void LoadCredits()
    {
        LoadScene("End Credits 1");
    }

    public void LoadControls()
    {
        LoadScene("control page");
    }
    // This function will make it so if you press the quit button, the application will close.
    public void QuitButton()
    {
        Application.Quit();
    }

    public void LoadMenu(){
        LoadScene("title");
    }

    // Function that loads the scene.
    void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}
