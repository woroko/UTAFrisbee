using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour {


	public int elemsBehind;
	public float throwThreshold;
	public float holdThreshold;

	public Transform trackingTarget;

	public float throwRate;
	private float nextThrow;


	private int throwMode; //0 = playback , 1 = waiting for throw , 2 = throw



	private Queue<Vector3> oldPosition = new Queue<Vector3>(); //create queue 

	void Start ()
	{
		//start program off by waiting for throw?
		throwMode = 1;
		//populate oldPosition with zeroes. this will probably screw up the first few seconds of program start
		//but alternative is having it crash when it Dequeues a null?
		// TODO: set "calibrating" mode.
		for (int i=0; i<elemsBehind; i++){
			oldPosition.Enqueue (new Vector3(0, 0, 0));
		}
		//oldPosition.TrimToSize (); // not working -> not worth the effort
	}

	/*
	 * the dream:
	 * check distance between current position and position elemsBehind updates ago to see relative change
	 * and use that to distinguish between modes
	 */
	void Update() { //Update vs LateUpdate?
		Vector3 position = new Vector3 (trackingTarget.position.x, trackingTarget.position.y, trackingTarget.position.z);


		float difference = Vector3.Distance(position, oldPosition.Dequeue ());


		//0 = playback , 1 = waiting for throw , 2 = throw
		//TODO: forgot what it was but something.......
		if (WaitingForThrow() && difference > throwThreshold) {
			throwMode = 2;
		} 

		else if (InPlayback () && Time.time > nextThrow && difference < holdThreshold) {
			nextThrow = Time.time + throwRate;
			throwMode = 1;
		} 

		else if (Throwing ()) { 
			// TODO: check if "wall" has been breached
			//to_do_wallfunc();
		}

		oldPosition.Enqueue (position);
	}


	/*
	 * returns true if game object is in playback mode, otherwise false. 
	 */
	public bool InPlayback ()
	{
		return throwMode == 0;
	}

	public bool WaitingForThrow()
	{
		return throwMode == 1;
	}

	public bool Throwing ()
	{
		return throwMode == 2;
	}

	//shorthand
	public int GetMode () 
	{
		return throwMode;
	}
}
