using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles framerate-independant playback/looping with speed adjustments
 * periodically fetches ThrowBuffer from ThrowController, when ThrowController is in the playback state
 * Position is fetched according to timestamp, and the closest throwBuffer index is chosen
 * Slower ´playback speeds may need movement frame interpolation
 */
public class ModeHandler : MonoBehaviour {

    public GameObject frisbeeModel;
    public Renderer frisbeeMeshRenderer;
    public TrailHandler trail;
    public Material recMaterial;
    public Material playMaterial;
    public float speed = 1F;
    public FrisbeeLocation current = null;
	    
    List<FrisbeeLocation> throwBuffer;
    int animIndex = 0;
    float rateTimer = 0F;
    bool initPlayback = false;
    bool transitioningToThrow = true;

    public float currentRotSpeed = 0F;
    public float currentForwardSpeed = 0F;

	//TESTING VARIABLES
	public ThrowController throwController;
    private float pauseUntil = 0F;

	// init
	void Start () {
	}

    //Finds closest list index corresponding to (time). Be careful with startFrom!
    public int getListIndexFromTime(List<FrisbeeLocation> queue, float time, int startFrom=0)
    {
        int i;
        bool found = false;
        if (time >= queue[queue.Count - 1].time)
            return -1;
        else {
            for (i = startFrom; i < queue.Count; i++)
            {

                if (time <= queue[i].time)
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

    // get FrisbeeLocation object from list at (time)
    public FrisbeeLocation getFrisbeeLocationAtTime(List<FrisbeeLocation> list, float time, int startFrom=0)
    {
        int idx = getListIndexFromTime(list, time, startFrom);
        if (idx < 0)
        {
            Debug.Log("getFrisbeeLocationAtTime: Could not find location object at time: " + time);
            return null;
        }
        return list[idx];
    }

    // called every frame
    void Update () {
        if (Input.GetKeyDown("s"))
        {
            if (speed >= 1.0F)
            {
                speed = 0.0F;
            }
            if (speed < 1.0F)
            {
                speed = speed + 0.25F;
            }
        }

        // Copy throwbuffer to ModeHandler
        if ((throwController.InPlayback() || throwController.WaitingForThrow()) && !initPlayback) {
            throwBuffer = throwController.getThrowBuffer();
            if (throwBuffer != null)
            {
                initPlayback = true;
                transitioningToThrow = true;
                frisbeeMeshRenderer.sharedMaterial = playMaterial;

                // deprecated
                //trail.Deactivate();
				//Debug.Log("Detached trail!");

				// Create trail in the beginning of playback loop
				trail.Create(throwBuffer);
            }
            //Debug.Log("Init playback!");
            pauseUntil = Time.time + 1.5F; //Slight pause before starting playback
		}
        else if (throwController.Throwing())
        {
            if (transitioningToThrow)
            {
                transitioningToThrow = false;
                trail.Reset(); // clear trail for new throw

				// deprecated
                //trail.Activate();
                //Debug.Log("Attached trail!");
            }
            initPlayback = false;
            frisbeeMeshRenderer.sharedMaterial = recMaterial;
            frisbeeModel.transform.localRotation = throwController.getCurrentRot();
            frisbeeModel.transform.localPosition = throwController.getCurrentPos();
        }

        else if (initPlayback && (Time.time > pauseUntil))
        {
            UpdatePositionFromList();
        }
	}


	void UpdatePositionFromList () {
		if (animIndex < throwBuffer.Count-1 && animIndex >= 0) {
			if (throwBuffer[animIndex] != null) {
				FrisbeeLocation location = throwBuffer[animIndex];
                if (animIndex % 2 == 0)
                {
                    Debug.Log("Frisbee Rot Speed: " + location.rotSpeed);
                    Debug.Log("Frisbee Forward Speed: " + location.forwardSpeed);
                }
                frisbeeModel.transform.localRotation = location.rot;
				frisbeeModel.transform.localPosition = location.pos;
                current = location;
                //currentForwardSpeed = location.forwardSpeed;
                //currentRotSpeed = location.rotSpeed;
			}
            rateTimer += Time.deltaTime * speed;
            animIndex = getListIndexFromTime(throwBuffer, rateTimer);
		} else {
			animIndex = 0;
			rateTimer = 0F;
            pauseUntil = Time.time + 1.0F;
		}
        /*if (Time.frameCount % 100 == 0)
            Debug.Log("IDX: " + animIndex);*/
	}
}
