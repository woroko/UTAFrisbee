using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {

	// An empty object that is parent to all the components in the start menu.
	// This makes it so the objects can be hidden/shown all at once
	public GameObject startMenu;

	// Variable for the title text
	public Text startMenuTitle;

	// An empty object that is parent to all the main buttons
	public GameObject mainButtons;

	// Main buttons
	public Button howToButton;
	public Button startButton;
	public Button settingsButton;
	public Button quitButton;

	// An empty object that is parent to the how-to view objects
	public GameObject howTo;

	// Button to return from the how-to view
	public Button returnButton;

	// An empty object that is parent to all the setting controls
	public GameObject settingSliders;

	// Setting sliders
	public Slider backtrackSlider;
	public Slider detectionSlider;
	public Slider rateSlider;
	public Slider trailSlider;

	// Setting Toggles 
	public Toggle terrainToggle;
	public Toggle lineToggle;

	// Buttons to reset settings to default or save them
	public Button defaultButton;
	public Button saveButton;

	// Materials to use in the terrain toggle setting
	public Material grassyTerrain;
	public Material simpleTerrain;

	// Use this for initialization
	void Start () {

		// Stop time while in the menu
		Time.timeScale = 0F;

		// Set click actions for all the buttons and sliders (see functions below)
		howToButton.GetComponent<Button> ().onClick.AddListener(OnHowToButtonClick);
		startButton.GetComponent<Button> ().onClick.AddListener(OnStartButtonClick);
		settingsButton.GetComponent<Button> ().onClick.AddListener(OnSettingsButtonClick);
		quitButton.GetComponent<Button> ().onClick.AddListener(OnQuitButtonClick);

		returnButton.GetComponent<Button> ().onClick.AddListener(OnReturnButtonClick);

		backtrackSlider.GetComponent<Slider> ().onValueChanged.AddListener(OnBacktrackSliderChange);
		detectionSlider.GetComponent<Slider> ().onValueChanged.AddListener(OnDetectionSliderChange);
		rateSlider.GetComponent<Slider> ().onValueChanged.AddListener(OnRateSliderChange);
		trailSlider.GetComponent<Slider> ().onValueChanged.AddListener(OnTrailSliderChange);

		terrainToggle.GetComponent<Toggle> ().onValueChanged.AddListener (OnTerrainToggleChange);
		lineToggle.GetComponent<Toggle> ().onValueChanged.AddListener (OnLineToggleChange);

		defaultButton.GetComponent<Button> ().onClick.AddListener(OnDefaultButtonClick);
		saveButton.GetComponent<Button> ().onClick.AddListener(OnSaveButtonClick);
	}

	// Update is called once per frame
	void Update() {

		// The start menu can be closed with space key
		if (Input.GetKey ("space")) {
			OnStartButtonClick ();
		}

		// The start menu can be opened again with esc key
		if (Input.GetKey ("escape")) {
			
			// Stop time while in the menu
			Time.timeScale = 0F;

			// Bring the menu to the screen
			startMenu.SetActive (true);
		}
	}

	//
	// Click actions for main buttons
	//

	// Button to open the how-to instructions
	void OnHowToButtonClick() {

		// Hide the main menu buttons
		mainButtons.SetActive(false);

		// Change the title text to how-to
		startMenuTitle.text = "How to throw";

		// Show the how-to view
		howTo.SetActive(true);
	}

	// Button to start the application
	void OnStartButtonClick() {

		// Resume time
		Time.timeScale = 1F;

		// Hide the menu
		startMenu.SetActive(false);
	}

	// Button to open the settings submenu
	void OnSettingsButtonClick() {
		
		// Hide the main menu buttons
		mainButtons.SetActive(false);

		// Change the title text to settings
		startMenuTitle.text = "Settings";

		// Bring forth the setting buttons
		settingSliders.SetActive(true);
	}

	// Button to quit the application
	void OnQuitButtonClick() {

		// Close the application (does nothing in the editor)
		Application.Quit ();

		// Debug for testing the button in the editor
		Debug.Log ("Quit button pressed");
	}

	//
	// Click actions for the how-to view buttons
	//

	void OnReturnButtonClick() {

		// Hide the how-to view
		howTo.SetActive(false);

		// Change the title text back to the actual title text
		startMenuTitle.text = "Frisbee Visualizer 2018";

		// Show the main menu buttons
		mainButtons.SetActive(true);
	}

	//
	// Click actions for setting sliders + save button
	//

	void OnBacktrackSliderChange(float value) {

		// Change the backtracking time based on the slider value
		GameObject.Find ("RecordingFrisbee").GetComponent<ThrowController> ().throwBacktrackingTime = value;

		// Update the value text next to the slider. The F1 parameter is used to trim the decimals to 1.
		backtrackSlider.GetComponentsInChildren<Text> ()[1].text = value.ToString("F1") + " s";
	}

	void OnDetectionSliderChange(float value) {

		// Change the detection time based on the slider value
		GameObject.Find ("RecordingFrisbee").GetComponent<ThrowController> ().throwDetectionTime = value;

		// Update the value text next to the slider
		detectionSlider.GetComponentsInChildren<Text> ()[1].text = value.ToString("F1") + " s";
	}

	void OnRateSliderChange(float value) {

		// Change the throw rate based on the slider value
		GameObject.Find ("RecordingFrisbee").GetComponent<ThrowController> ().throwRate = value;

		// Update the value text next to the slider
		rateSlider.GetComponentsInChildren<Text> ()[1].text = value.ToString() + " s";
	}

	void OnTrailSliderChange(float value) {

		// Change the percentage of trail dots drawn based on the slider value
		GameObject.Find ("Trail").GetComponent<TrailHandler> ().trailFrequency = value;

		// Update the value text next to the slider
		trailSlider.GetComponentsInChildren<Text> ()[1].text = value.ToString() + " %";
	}

	void OnTerrainToggleChange(bool value) {

		// Get the terrain game object
		GameObject terrain = GameObject.Find ("Plane");

		// User wants to see the grassy terrain
		if (value == true) {

			// Make the terrain wider
			terrain.transform.localScale = new Vector3 (10, 1, 10);

			// Set the terrain to grass
			terrain.GetComponent<Renderer> ().material = grassyTerrain;
		} 

		// User hates the grassy terrain and wants it gone
		else {

			// Make the terrain smaller
			terrain.transform.localScale = new Vector3 (1, 1, 1);

			// Set the terrain to simple white glow
			terrain.GetComponent<Renderer> ().material = simpleTerrain;
		}
	}

	void OnLineToggleChange(bool value) {

		// User wants to see the green line
		if (value == true) {

			// Set the line to active
			GameObject.Find ("Line").GetComponent<LineRenderer> ().enabled = true;
		}

		// User doesn't want to see the green line
		else {

			// Set the line to inactive
			GameObject.Find ("Line").GetComponent<LineRenderer> ().enabled = false;
		}
	}

	// Resets the settings back to default values
	void OnDefaultButtonClick() {

		OnBacktrackSliderChange (1.0F);
		backtrackSlider.value = 1.0F;

		OnDetectionSliderChange (0.5F);
		detectionSlider.value = 0.5F;

		OnRateSliderChange (4.0F);
		rateSlider.value = 4.0F;

		OnTrailSliderChange (100.0F);
		trailSlider.value = 100.0F;

		OnTerrainToggleChange (true);
		terrainToggle.isOn = true;

		OnLineToggleChange (true);
		lineToggle.isOn = true;
	}

	// Note: the settings are saved when using the sliders, not when pressing the save button.
	// All this button does is close the settings menu and make the user feel in control
	void OnSaveButtonClick() {

		// Hide the setting buttons
		settingSliders.SetActive(false);

		// Change the title text back to the actual title text
		startMenuTitle.text = "Frisbee Visualizer 2018";

		// Show the main menu buttons
		mainButtons.SetActive(true);
	}
}
