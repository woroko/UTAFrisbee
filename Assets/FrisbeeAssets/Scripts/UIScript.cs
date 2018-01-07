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

    public GameObject mainCamera, topCamera, sideCamera;

    private bool firstThrow;

    public float moveSpeed = 2.5F;

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
        
        //Camera keyboard controls
        if (mainCamera.activeSelf)
        {
            GameObject tempCam = mainCamera;
            if (Input.GetKey("q"))
            {
                tempCam.transform.Translate(tempCam.transform.forward * -1F * moveSpeed*Time.deltaTime);
            }
            if (Input.GetKey("e"))
            {
                tempCam.transform.Translate(tempCam.transform.forward * moveSpeed*Time.deltaTime);
            }
            if (Input.GetKey("w"))
            {
                tempCam.transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime), Space.World);
            }
            if (Input.GetKey("s"))
            {
                tempCam.transform.Translate(new Vector3(0, 0, -moveSpeed * Time.deltaTime), Space.World);
            }
            if (Input.GetKey("a"))
            {
                tempCam.transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0), Space.World);
            }
            if (Input.GetKey("d"))
            {
                tempCam.transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0), Space.World);
            }
        }
        else if (topCamera.activeSelf)
        {
            GameObject tempCam = topCamera;
            if (Input.GetKey("q"))
            {
                tempCam.transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0), Space.World);
            }
            if (Input.GetKey("e"))
            {
                tempCam.transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0), Space.World);
            }
            if (Input.GetKey("w"))
            {
                tempCam.transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime), Space.World);
            }
            if (Input.GetKey("s"))
            {
                tempCam.transform.Translate(new Vector3(0, 0, -moveSpeed * Time.deltaTime), Space.World);
            }
            if (Input.GetKey("a"))
            {
                tempCam.transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0), Space.World);
            }
            if (Input.GetKey("d"))
            {
                tempCam.transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0), Space.World);
            }
        }
        else if (sideCamera.activeSelf)
        {
            GameObject tempCam = sideCamera;
            if (Input.GetKey("q"))
            {
                tempCam.transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0), Space.World);
            }
            if (Input.GetKey("e"))
            {
                tempCam.transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0), Space.World);
            }
            if (Input.GetKey("w"))
            {
                tempCam.transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0), Space.World);
            }
            if (Input.GetKey("s"))
            {
                tempCam.transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0), Space.World);
            }
            if (Input.GetKey("a"))
            {
                tempCam.transform.Translate(new Vector3(0, 0, -moveSpeed * Time.deltaTime), Space.World);
            }
            if (Input.GetKey("d"))
            {
                tempCam.transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime), Space.World);
            }
        }

        if (frisbeeScript.isSeen())
            isSeenText.text = "";
        else
            isSeenText.text = "Can't see fisbee!";

        // Changing the indicator text based on the throwmode
        int throwmode = ThrowController.GetMode();
        // Get fresbee back to throwing position & playing your throw
        if (throwmode == 0) {
            recordingIndicator.color = Color.white;
            if (firstThrow == true) 
                recordingIndicator.text = "Get frisbee to starting position";
            else
                recordingIndicator.text = "Get frisbee back to starting position (playing last throw)";
        }

        // Ready to throw
        else if (throwmode == 1) {
            if (frisbeeScript.isSeen()) {
                recordingIndicator.color = Color.white;
                if (firstThrow == true)
                    recordingIndicator.text = "Ready for a new throw";
                else
                    recordingIndicator.text = "Ready for a new throw (playing last throw)";
            }
            else {
                recordingIndicator.color = Color.white;
                if (firstThrow == true)
                    recordingIndicator.text = "Don't throw yet";
                else
                    recordingIndicator.text = "Don't throw yet (playing last throw)";
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