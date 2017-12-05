using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	// Variables to access the frisbee and it's script
	private GameObject frisbee;
	private CustomRigidBody frisbeeScript;

    public ModeHandler modeHandler;

	public Text recordingIndicator;
	public Text frisbeePosition;
	public Text frisbeeRotation;
    public Text isSeenText;

    private bool firstThrow;

    // Use this for initialization
    void Start () {
		frisbee = GameObject.Find ("RecordingFrisbee");
		frisbeeScript = frisbee.GetComponent<CustomRigidBody> ();
        firstThrow = true;
	}
	
	// Update is called once per frame
	void Update () {

        // Clear the trail visualization when pressing space
        // UPDATE: Don't need to clear trail here, handled in ModeHandler
        /*if (Input.GetKey ("space")) {
			frisbee.GetComponent<TrailRenderer> ().Clear();
		}*/

        if (frisbeeScript.isSeen())
            isSeenText.text = "Can see frisbee: true";
        else
            isSeenText.text = "Can see frisbee: false";

        // Changing the indicator text based on the throwmode
        int throwmode = ThrowController.GetMode();
        // Get fresbee back to throwing position & playing your throw
        if (throwmode == 0) {
            recordingIndicator.color = Color.red;
            if (firstThrow == true) 
                recordingIndicator.text = "Get frisbee to starting position";
            else
                recordingIndicator.text = "Get frisbee back to starting position (playing your last throw)";
        }

        // Ready to throw
        else if (throwmode == 1) {
            if (frisbeeScript.isSeen()) {
                recordingIndicator.color = Color.green;
                if (firstThrow == true)
                    recordingIndicator.text = "Ready for a new throw";
                else
                    recordingIndicator.text = "Ready for a new throw (Playing your last throw)";
            }
            else {
                recordingIndicator.color = Color.red;
                if (firstThrow == true)
                    recordingIndicator.text = "Can't see frisbee, don't throw yet";
                else
                    recordingIndicator.text = "Can't see frisbee, don't throw yet (Playing your last throw)";
            }
        }

        // Recording frisbee throw
        else if (throwmode == 2) {
            firstThrow = false;
            recordingIndicator.color = Color.red;
            recordingIndicator.text = "Analyzing your throw";
        }

		// Testing out showing the frisbee position and rotation on screen
		frisbeePosition.text = "Position: " + frisbee.transform.localPosition;
		frisbeeRotation.text = "Playback Speed: " + modeHandler.speed + "x";
	}
}