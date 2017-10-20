using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	// Variables to access the frisbee and it's script
	private GameObject frisbee;
	private CustomRigidBody frisbeeScript;

	public Text recordingIndicator;
	public Text frisbeePosition;
	public Text frisbeeRotation;

	// Use this for initialization
	void Start () {
		frisbee = GameObject.Find ("RecordingFrisbee");
		frisbeeScript = frisbee.GetComponent<CustomRigidBody> ();
	}
	
	// Update is called once per frame
	void Update () {

		// Changing the indicator text based on the isSeen() method
		if (frisbeeScript.isSeen ()) {
			recordingIndicator.text = "Recording...";
		} else {
			recordingIndicator.text = "Cannot see frisbee";
		}

		// Testing out showing the frisbee position and rotation on screen
		frisbeePosition.text = "Position: " + frisbee.transform.localPosition;
		frisbeeRotation.text = "Rotation: " + frisbee.transform.localRotation;
	}
}
