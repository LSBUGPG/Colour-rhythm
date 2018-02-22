using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void changemenuscene(string Menu)
    {

        Application.LoadLevel(Menu);

    }
}
