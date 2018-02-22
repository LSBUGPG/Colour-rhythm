using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class carryscore : MonoBehaviour
{
    public Text scoreText;

    private void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("casualrhythm").ToString();
        scoreText.text = PlayerPrefs.GetInt("randomrhythm").ToString();
    }
}


