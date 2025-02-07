using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTrack : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Final Score: " + PlayerPrefs.GetInt("Scores");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void SetUI()
    //{
    //    // Set the livesText to the text in "" + the current lives variable value.
    //    scoreText.text = " " + PlayerPrefs.GetInt("Scores");
    //}
}
