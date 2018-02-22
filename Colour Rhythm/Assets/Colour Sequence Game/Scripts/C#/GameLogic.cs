using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour 
{
	public int[] sequence; //The sequence int array will hold our sequence order
	public int sequenceLength; //The sequenceLength is how long a sequence will be. Everytime the player completes a sequence it goes up one
	public int sequencesPlayedCom; //The sequencesPlayedCom is to measure how many times the computer has pressed the buttons in the current sequence
	public int sequencesPlayedPlayer; //The sequencesPlayedPlayer is to measure how many times the player has pressed the buttons in the current sequence

	public int score; //Each time the player completes a sequence they get +1 onto their score
	public int casualrhythmHighScore; //This variable holds the highest score that the player has ever achieved on the casual gamemode
	public int randomrhythmHighScore; //This variable holds the highest score that the player has ever achieved on the randomize gamemode
	public string gameMode; //This will tell us what game mode the player chose so we can modify the game (casual, randomize)

	//Buttons
	public GameObject greenButton; //This holds the GameObject of our green button, so that the script can send messages over to it
	public GameObject redButton; //This holds the GameObject of our red button, so that the script can send messages over to it
	public GameObject blueButton; //This holds the GameObject of our blue button, so that the script can send messages over to it
	public GameObject yellowButton; //This holds the GameObject of our yellow button, so that the script can send messages over to it

	//UI
	public Text scoreText; //This is the text that is displayed on screen, that tells us our score
	public Text highScoreText; //Just like the score text, but displaying the high score

	//Sounds
	public AudioSource audioSource; //This is the source of which we will play the sequence fail and complete sounds through
	//public AudioClip sequenceCompleteSound;
	//public AudioClip sequenceFailSound;
	public AudioClip buttonClickSound;


	void Start () //The Start function is called on the frame that the script becomes active
	{
		gameMode = PlayerPrefs.GetString("GameMode"); //We need to access our game mode that the player selected
        casualrhythmHighScore = PlayerPrefs.GetInt("CasualRhythmHighScore"); //We need to access our saved score
        randomrhythmHighScore = PlayerPrefs.GetInt("RandomRhythmHighScore"); 
		audioSource = GameObject.Find("Camera").GetComponent<AudioSource>();
		audioSource.volume = PlayerPrefs.GetFloat("Volume"); //We are setting our volume to the one that we set in the options menu
		sequence = new int[50]; //Here we are defining the sequence array's size
		StartCoroutine(GenerateNextSequence()); //We call the GenerateNextSequence function which randomly selects the next number for the sequence
	}

	void Update () //The Update function is called once per frame
	{
		scoreText.text = "SCORE\n" + score; //Here we are setting what our on screen text will say. The '\n' basically makes a new line
		if(gameMode == "casualrhythm")
        {
			highScoreText.text = "HIGH SCORE\n" + casualrhythmHighScore; //Just like the on screen score text, but displaying the high score for the casual gamemode
		}
		else if(gameMode == "randomrhythm")
        {
			highScoreText.text = "HIGH SCORE\n" + randomrhythmHighScore; //Just like the on screen score text, but displaying the high score for the randomize gamemode
		}
	}

	IEnumerator GenerateNextSequence ()
	{
		sequencesPlayedCom = 0; //Set the number of times that the computer has clicked a button to 0
		sequencesPlayedPlayer = 0; //Set the number of times that the player has clicked a button to 0

		if(gameMode == "casualrhythm")
        {
			sequence[sequenceLength - 1] = Random.Range(1, 5); //Since array index's start at 0, we have it randomly select a number between 1 and 5 (it rounds to the nearest int) and set it to the most current index number.
			yield return new WaitForSeconds(1); //We wait 1 second after picking our next colour, before going on to playing the sequence
			StartCoroutine(PlaySequence()); //Now that we have our next colour, we can go and play the array to the player
		}
		else if(gameMode == "randomrhythm")
        {
			for(int i = 0; i < sequenceLength; i ++){
				sequence[i] = Random.Range(1, 5);
			}
			yield return new WaitForSeconds(1);
			StartCoroutine(PlaySequence()); //Now that we have our random sequence, we can have the computer play it
		}
	}

	IEnumerator PlaySequence () //Plays the sequence
	{	
		CanClickButton(false); //We call a function that disables the buttons while the computer is playing the sequence

		while(sequencesPlayedCom < sequenceLength){ //A while loop. Here we have it continuously do what is inside the brackets as long as sequencesPlayedCom < sequenceLength
			GameObject buttonToPress = null; //We set this temporary GameObject variable to hold the button so its ButtonPress function can be called

			if(sequence[sequencesPlayedCom] == 1){ //If the next element in the int array is 1 then it will be a green button
				buttonToPress = greenButton;
			}
			else if(sequence[sequencesPlayedCom] == 2){
				buttonToPress = redButton;
			}
			else if(sequence[sequencesPlayedCom] == 3){
				buttonToPress = blueButton;
			}
			else if(sequence[sequencesPlayedCom] == 4){
				buttonToPress = yellowButton;
			}

			buttonToPress.SendMessage("ButtonPress"); //We call the ButtonPress function on the button so the player can see and hear it
			yield return new WaitForSeconds(0.7f); //Wait 0.7 seconds until we continue to the next colour in the sequence
			sequencesPlayedCom++; //Adding one to the sequencesPlayedCom so we know that computer just clicked a button
		}

		if(sequencesPlayedCom == sequenceLength){
			CanClickButton(true); //After the computer has pressed all the buttons in the sequence, the player can now click on the buttons
		}
	}

	IEnumerator CheckSequenceButton (int id)
	{	
		if(sequence[sequencesPlayedPlayer] == id){ //We check to see if the next color of the sequence is the one that the player pressed
			sequencesPlayedPlayer ++;
		}else{
			yield return new WaitForSeconds(0.3f); 
			//audioSource.PlayOneShot(sequenceFailSound); //Since the player failed the sequence, we need to play the appropriate sound

			SaveScore();

            
            PlayerPrefs.SetInt("randomrhythm", score);
            PlayerPrefs.SetInt("casualrhythm", score);

            yield return new WaitForSeconds(0.1f); //A delay after the player fails 
			Application.LoadLevel(3); //Since the player we load up the menu
            
                     
        }

		if(sequencesPlayedPlayer == sequenceLength){ //If the player has completed the sequence
			yield return new WaitForSeconds(0.3f); //We have a small delay after the player completes the sequence
			//audioSource.PlayOneShot(sequenceCompleteSound);
			score++; //Since the player has completed the sequence, we will add 1 to the score

            if (sequenceLength == 6)
            {
                // load the next level here...
                Application.LoadLevel(1);
            }
            else
            {
                sequenceLength++; //The sequence length is one added to it
			    StartCoroutine(GenerateNextSequence()); //We continue on with this loop, increasing the sequence one colour at a time
            }
		}
	}

	public void GoToMenu () //Function used to get back to the main menu	
	{
		audioSource.PlayOneShot(buttonClickSound); //Playing the button click sound
		SaveScore();
		Application.LoadLevel(1);
	}

	void CanClickButton (bool i) //Tells the buttons if they can be clicked or not
	{
		greenButton.SendMessage("DisableOrEnableButton", i);
		redButton.SendMessage("DisableOrEnableButton", i);
		blueButton.SendMessage("DisableOrEnableButton", i);
		yellowButton.SendMessage("DisableOrEnableButton", i);
	}

	void SaveScore () //Checks if score is higher that high score then saves it to Player Prefs
	{
		if(gameMode == "casualrhythm")
        { //If the game mode is Casual 
			if(score > casualrhythmHighScore)
            { //If the players current score is higher than their highscore then we will change it
				PlayerPrefs.SetInt("CasualRhythmHighScore", score);
			}
		}
		else if(gameMode == "randomrhythm")
        {
			if(score > randomrhythmHighScore)
            {
				PlayerPrefs.SetInt("RandomRhythmHighScore", score);
			}
		}
	}
}
