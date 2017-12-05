using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Threshold {
	public float throwing, holding, ending, glitch, throwStartZ;
    //recommend 0.2, 0.05, 1.2, 5
}


public class ThrowController : MonoBehaviour {

    //Suggested values: backtracking=1.5s, throwDetection=0.5s
    //NOTE: Update runs at variable Unity framerate, need timestamps

    public float throwBacktrackingTime = 1.5F;
    public float throwDetectionTime = 0.5F;

	public Threshold threshold;

	public Transform trackingTarget;

    public CustomRigidBody customRB;

	//UI
	public Text modeText;

	//delay between throws ( if the frisbee is laying in the net, don't start a new throw based off that stillness. )
	public float throwRate;
	private float nextThrow;


	private static int throwMode; //0 = playback , 1 = waiting for throw + playback , 2 = throw

	// position that throw started
    Vector3 throwStartPos;
    float throwStartTime;
    float scriptStartTime;

    private const int BUFFERCAPACITY = 2000;

    //Create a new double-ended queue with a capacity of BUFFERCAPACITY FrisbeeLocations
    //Double-ended queue implementation from 
    private Deque<FrisbeeLocation> captureBuffer = new Deque<FrisbeeLocation>(BUFFERCAPACITY);
    private List<FrisbeeLocation> throwBuffer = null;

    void Start ()
	{
		//start program off in playback mode
		throwMode = 0;

        //start waiting for throws after 2.5 seconds
        nextThrow = Time.time + 2.5F;

		//UI
		SetModeText ();

        //Buffer on program start
        scriptStartTime = Time.time;

        /*NOTE: RecordingFrisbee is Useful for debugging, because we can see the position
        in the editor while running. Also, it will be more portable if other
        MoCap systems than OptiTrack exist that provide a Unity plugin
        Direct access would also be useful though, since we could detect isSeen()
        NOTE: Direct access now implemented in CustomRigidBody*/

		//test for lab - could be used to make unity components less cluttered if works.
		//OptitrackRigidBody orb = gameObject.GetComponent<OptitrackRigidBody>();
		//Debug.Log ("orb==trackingtarget: "+(orb.transform == trackingTarget));
	}

	/*
	 * Buffers BUFFERCAPACITY number of previous FrisbeeLocations
     * 
	 */
	void Update() {

        if (Input.GetKeyDown("r"))
        {
            throwMode = 0;
        }

        Vector3 currentPosition = new Vector3 (trackingTarget.position.x, trackingTarget.position.y, trackingTarget.position.z);


        //add current FrisbeeLocation to back of queue
        //buffer has not yet reached full capacity
        if (captureBuffer.Count < BUFFERCAPACITY-1)
            captureBuffer.Add(new FrisbeeLocation(trackingTarget.localRotation, trackingTarget.localPosition, Time.time, customRB.isSeen()));
        else //buffer has reached BUFFERCAPACITY
        {
            captureBuffer.RemoveFront(); //remove oldest FrisbeeLocation
            captureBuffer.Add(new FrisbeeLocation(trackingTarget.localRotation, trackingTarget.localPosition, Time.time, customRB.isSeen()));
        }
        


		//0 = playback , 1 = waiting for throw + playback , 2 = throw in progress
        //Buffer first 2.5 seconds
        if (Time.time > scriptStartTime + 2.5F)
		HandleModeChanges(currentPosition);
        if(Throwing())
        {
            //deprecated:
            //throwBuffer.Add(new FrisbeeLocation(trackingTarget.localRotation, trackingTarget.localPosition, Time.time-throwStartTime));
            FrisbeeLocation temp = captureBuffer.Get(captureBuffer.Count - 1);
            FrisbeeLocation prev = captureBuffer.Get(captureBuffer.Count - 1 - 2);
            float currentThrowTime = temp.time - throwStartTime;
            float prevThrowTime = prev.time - throwStartTime;
            float fSpeed = Vector3.Distance(temp.pos, prev.pos) / (currentThrowTime - prevThrowTime);
            float a = (Quaternion.Inverse(prev.rot) * temp.rot).eulerAngles.y;
            float b = (Quaternion.Inverse(temp.rot) * prev.rot).eulerAngles.y;

            float rSpeed = Mathf.Abs((Mathf.Min(a,b) / 360F) / (currentThrowTime - prevThrowTime));
            rSpeed = rSpeed * 60F;
            throwBuffer.Add(new FrisbeeLocation(temp.rot, temp.pos, currentThrowTime, fSpeed, rSpeed, temp.wasSeen));
        }

		SetModeText();
	}


	void SetModeText ()
	{
		modeText.text = "Mode: " + GetMode().ToString();
	}

    //finds the closest index to the timestamp
    public int getBufferIndexFromTime(Deque<FrisbeeLocation> queue, float time)
    {
        int i = 0;
        bool found = false;
        if (time >= queue[queue.Count - 1].time)
        {
            return -1;
        }
        else
        {
            for (i = 0; i < queue.Count; i++)
            {
                if (time <= queue.Get(i).time)
                {
                    found = true;
                    break;
                }
            }
        }
        if (found)
            return i;
        else return -1; // did not find a FrisbeeLocation for time
    }

    //finds the position frisbee position at (time) in the buffer
    Vector3 getPosAtTime(Deque<FrisbeeLocation> queue, float time)
    {
        int idx = getBufferIndexFromTime(queue, time);
        if (idx < 0)
        {
            Debug.Log("getPosAtTime: Could not find position at time: " + time);
            return new Vector3(0F, 0F, 0F);
        }
        return queue.Get(idx).pos;
    }


    void HandleModeChanges (Vector3 position)
	{
        //calc difference between current position and position throwDetectionTime seconds before present in buffer
        //float difference = Vector3.Distance(position, getPosAtTime(captureBuffer, Time.time - throwDetectionTime));//(shortAndLongDif - nowAndShortDif);
        float difference = position.z - getPosAtTime(captureBuffer, Time.time - throwDetectionTime).z;

        //threshold.glitch hack to ignore DebugMover abrupt position jump
        //detect throw start if position difference in throwDetectionTime is above throwing threshold
        //also includes rudimentary check for z-safezone
        if (WaitingForThrow() && difference > threshold.throwing && difference < threshold.glitch && position.z < 1F) {
			HandleThrow(position);
		}
		else if (InPlayback () && Time.time > nextThrow && position.z < threshold.throwStartZ && difference < threshold.holding) {
            throwMode = 1;
            Debug.Log("Waiting for throw...");
		} 
		else if (Throwing ()) {

            if (Time.frameCount % 50 == 0)
                Debug.Log("airborne OR mid-throw, zpos: " + (position.z - throwStartPos.z));

			HandleEnd (position);

			//maybe time limit too
		}


	}

	void HandleThrow (Vector3 position)
	{
        throwStartTime = Time.time - throwBacktrackingTime;
        int firstThrowIndex = getBufferIndexFromTime(captureBuffer, throwStartTime);
        throwMode = 2;
		Debug.Log("Throw just began!");

        //get first FrisbeeLocation in throw
        FrisbeeLocation temp = captureBuffer.Get(firstThrowIndex);
        FrisbeeLocation prev = null;

        throwStartPos = temp.pos;
        throwStartTime = temp.time;

        //reinitialize throwBuffer to empty list
        throwBuffer = new List<FrisbeeLocation>();

        //Transfer previous throwBacktrackingTime seconds of data to throwBuffer
        //to make sure that the beginning of the throw is not lost
        //TODO: maybe add a more sophisticated backtracking algorithm that is based on velocity
        for (int i=firstThrowIndex; i<captureBuffer.Count; i++)
        {
            temp = captureBuffer.Get(i);
            //manually initialize first timestamp to zero
            if (i == firstThrowIndex)
                throwBuffer.Add(new FrisbeeLocation(temp.rot, temp.pos, 0F, 0F, 0F, temp.wasSeen));
            else
            { //rest of the timestamps will increment from zero
                if (i-firstThrowIndex>=2)
                    prev = throwBuffer[throwBuffer.Count - 2];
                else
                    prev = throwBuffer[throwBuffer.Count - 1];
                float currentThrowTime = temp.time - throwStartTime;
                float fSpeed = Vector3.Distance(temp.pos, prev.pos) / (currentThrowTime - prev.time);
                float a = (Quaternion.Inverse(prev.rot) * temp.rot).eulerAngles.y;
                float b = (Quaternion.Inverse(temp.rot) * prev.rot).eulerAngles.y;

                float rSpeed = Mathf.Abs((Mathf.Min(a,b)/360F) / (currentThrowTime - prev.time));
                rSpeed = rSpeed * 60F;
                throwBuffer.Add(new FrisbeeLocation(temp.rot, temp.pos, currentThrowTime, fSpeed, rSpeed, temp.wasSeen));
            }
            Debug.Log("ThrowBuffer: " + throwBuffer[throwBuffer.Count - 1].rotSpeed.ToString());
        }
        
        


	}

    //Detect if z-distance from beginning of throw is greater than ending threshold (virtual wall)
	void HandleEnd (Vector3 position)
	{
		if (position.z > threshold.ending) {
			Debug.Log ("hit max throw z-distance at: " + position);
			throwMode = 0;
            nextThrow = Time.time + throwRate;
        }
        
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
	public static int GetMode () 
	{
		return throwMode;
	}

    public List<FrisbeeLocation> getThrowBuffer()
    {
        if (InPlayback() || WaitingForThrow())
        {
            return throwBuffer;
        }
        else return null;
    }

    public Vector3 getCurrentPos()
    {
        return trackingTarget.localPosition;
    }
    public Quaternion getCurrentRot()
    {
        return trackingTarget.localRotation;
    }
}
