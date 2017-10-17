using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {

	public GameObject startMenu;

	public Button startButton;
	public Button quitButton;

	// Use this for initialization
	void Start () {

		// Stop time (so the frisbee doesn't fly while in start menu)
		Time.timeScale = 0F;

		// Set a click action for the start button
		startButton.GetComponent<Button> ().onClick.AddListener(OnStartButtonClick);

		// Set a click action for the start button
		quitButton.GetComponent<Button> ().onClick.AddListener(OnQuitButtonClick);
	}

	// Update is called once per frame
	void Update() {

		// The start menu can be opened again with esc key
		if (Input.GetKey ("escape")) {
			
			// Stop time
			Time.timeScale = 0F;

			// Bring the menu to the screen
			startMenu.SetActive (true);
		}
	}

	// Start button does this
	void OnStartButtonClick() {

		// Resume time
		Time.timeScale = 1F;

		// Hide the menu
		startMenu.SetActive(false);
	}

	// Quit button does this
	void OnQuitButtonClick() {

		// Close the application (does nothing in the editor)
		Application.Quit ();

		// Debug for testing the button in the editor
		Debug.Log ("Quit button pressed");
	}
}
