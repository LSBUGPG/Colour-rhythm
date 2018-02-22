using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    public float volume; //This will hold our volume 0.0 to 1.0
    
    //Menu Pages
    public GameObject mainMenuPage; //These are the pages that hold the main menu, play page and options page
    public GameObject playGamePage;
    public GameObject optionsPage;
    public GameObject creditsPage;

    //UI
    public Text casualrhythmHighScoreText; //This is the text that displays the highscore for the Casual game mode
    public Text randomrhythmHighScoreText; //This is the text that displays the highscore for the Randomize game mode

    public Slider volumeSlider; //This is the slider that controls the audio volume
    

    //Sounds
    public AudioSource audioSource; //This is the source of which we will play the button clicks through
    public AudioClip buttonClickSound; //This is the button click sound effect\

    void Start()
    {
        mainMenuPage.active = true; //Activating the menu and deactivating the other pages, because we want the player to start at the menu when they load up the game
        playGamePage.active = false;
        optionsPage.active = false;
        creditsPage.active = false;

        casualrhythmHighScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("CasualRhythmHighScore"); //Setting the highscore text to our saved playerpref variable.
        randomrhythmHighScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("RandomRhythmHighScore");

        audioSource = GameObject.Find("Camera").GetComponent<AudioSource>(); //Getting the cameras AudioSource component
        volumeSlider.value = PlayerPrefs.GetFloat("Volume"); //We need to set the volume to the one that we saved
        
    }

    public void PlayGameMode(string gameMode) //This function is called once the player clicks on a gamemode button
    {
        PlayerPrefs.SetString("GameMode", gameMode); //Here we are saving what our selected gamemode is. PlayerPrefs can store variables so you can use them across scenes and even after you close down and re open the game
        PlayerPrefs.SetFloat("Volume", volumeSlider.value); //When we start the game we want to save our volume that we could of set in the options menu
        Application.LoadLevel(2); //Now that we have our gamemode saved, we are ready to start up the game. Since in the Build Settings the menu scene's id is '0' and the game's scene is '1' we will load up that
    }

    public void SetPageActive(string page)
    {
        if (page == "mainmenu")
        {
            mainMenuPage.active = true;
            playGamePage.active = false;
            optionsPage.active = false;
            creditsPage.active = false;
        }
        if (page == "playgame")
        {
            mainMenuPage.active = false;
            playGamePage.active = true;
            optionsPage.active = false;
            creditsPage.active = false;
        }
        if (page == "options")
        {
            mainMenuPage.active = false;
            playGamePage.active = false;
            optionsPage.active = true;
            creditsPage.active = false;
        }
        if (page == "credits")
        {
            mainMenuPage.active = false;
            playGamePage.active = false;
            optionsPage.active = false;
            creditsPage.active = true;

        }

    }
    public void ResetScore() //When this function is called, it resets the highscores for all game modes
    {
        PlayerPrefs.SetInt("CasualRhythmHighScore", 0);
        PlayerPrefs.SetInt("RandomRhythmHighScore", 0);
        Application.LoadLevel(1);
    }

    public void QuitGame() //This function is called when you click on the 'Quit Game' button
    {
        Application.Quit(); //Closes the application
    }

    public void ButtonSound() //When called, a button click sound will be played through the audioSource variable component
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
    public void GoToMenu() //Function used to get back to the main menu	
    {
        Application.LoadLevel(0);
    }
}