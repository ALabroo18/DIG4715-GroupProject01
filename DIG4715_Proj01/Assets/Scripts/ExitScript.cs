using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ExitScript : MonoBehaviour
{
    public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){

        PlayerBehavior pB = other.GetComponent<PlayerBehavior>();

        pB.PlaySound(audioClip);

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Level2")
        {
            PlayerPrefs.SetInt("Scores", pB.Score);
            SceneManager.LoadScene("Project1_WinScreen");
        }
        else
        {
            PlayerPrefs.SetInt("Scores", pB.Score);
            //PlayerPrefs.SetInt("LivesText", pB.Lives);
            SceneManager.LoadScene("Level2");
        }
        
    }
}
