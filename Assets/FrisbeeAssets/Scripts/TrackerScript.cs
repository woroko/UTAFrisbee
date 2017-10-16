using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerScript : MonoBehaviour {


    public GameObject RecordingFrisbee;
    private List<FrisbeeLocation> LocationList = new List<FrisbeeLocation>();

    // Use this for initialization
    void Start () {
        if(RecordingFrisbee == null)
        {
            Debug.Log("not working");
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        LocationList.Add(new FrisbeeLocation(RecordingFrisbee.transform.localRotation, RecordingFrisbee.transform.localPosition));
        Debug.Log(RecordingFrisbee.transform.localPosition);
        Debug.Log(RecordingFrisbee.transform.localRotation);
    }
}
