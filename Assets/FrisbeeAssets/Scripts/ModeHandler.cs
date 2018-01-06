using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles framerate-independant playback/looping with speed adjustments
 * periodically fetches ThrowBuffer from ThrowController, when ThrowController is in the playback state
 * The closest throwBuffer index is chosen for playback every frame
 * Slower playback speeds may benefit visually from frame interpolation
 * We have decided not to implement this, since interpolation hides
 * possible capture errors from the user.
 * If the OptiTrack system is working properly, there should be enough data
 * to make interpolation unnecessary.
 */
public class ModeHandler : MonoBehaviour {

    public GameObject frisbeeModel;
    public Renderer frisbeeMeshRenderer;
    public TrailHandler trail;
    public TrailHandler simulationTrail;

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

    Prediction pred = new Prediction();

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
        // Detects when we need to initialize
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

				// Create actual trail of the throw in the beginning of playback loop
				trail.Create(throwBuffer);

                //Simulate the frisbee and create simulated trail
                simulateFrisbee();
                
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
                simulationTrail.Reset(); // clear simulation trail

				// deprecated previous implementation using Unity builtin Trail
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

    // Simulates the frisbee with empirically based model in the z and y-axis, and simple constant velocity on the x-axis
    void simulateFrisbee()
    {
        FrisbeeLocation cutoff = null;
        int cutoffIndex = 0;

        //Find a cutoff point where the frisbee is likely to still be in mid-flight
        foreach (FrisbeeLocation loc in throwBuffer)
        {
            //cutoff is at 0.7 times the travel distance from throw start to throw end
            if (loc.pos.z - throwBuffer[0].pos.z > 0.7 * (throwBuffer[throwBuffer.Count - 1].pos.z - throwBuffer[0].pos.z))
            {
                cutoff = loc;
                break;
            }
            cutoffIndex++;
        }

        if (cutoff != null)
        {
            //Calculate the velocities and angle to ground plane
            FrisbeeLocation prevLoc = throwBuffer[cutoffIndex - 4];
            double vz0 = (cutoff.pos.z - prevLoc.pos.z) / (cutoff.time - prevLoc.time);
            double vy0 = (cutoff.pos.y - prevLoc.pos.y) / (cutoff.time - prevLoc.time);
            double vx0 = (cutoff.pos.x - prevLoc.pos.x) / (cutoff.time - prevLoc.time);
            Vector3 forwardDirection = cutoff.pos - prevLoc.pos;
            forwardDirection.Normalize();
            float pitch = Vector3.Angle(forwardDirection, Vector3.ProjectOnPlane(forwardDirection, Vector3.up));

            //integration window is set to 0.001s
            List<FrisbeeLocation> simulated = pred.simulate3D(cutoff.pos.x, cutoff.pos.y, cutoff.pos.z, vx0, vy0, vz0, pitch, 0.001F);
            //debug
            /*Debug.Log("Simulation len: " + simulated.Count);
            Debug.Log("ThrowBuffer len: " + throwBuffer.Count);
            Debug.Log("Cutoff and prev y-pos: " + cutoff.pos.y + " " + prevLoc.pos.y);
            Debug.Log("Cutoff and prevloc timestamps" + cutoff.time + " " + prevLoc.time);
            Debug.Log("Simulation parameters: " + cutoff.pos.y + " " + vz0 + " " + vy0 + " " + pitch);*/
            simulationTrail.Create(simulated);
        }
    }

    // Called every frame while in playback to update the PlaybackFrisbee position
	void UpdatePositionFromList () {
		if (animIndex < throwBuffer.Count-1 && animIndex >= 0) {
			if (throwBuffer[animIndex] != null) {
				FrisbeeLocation location = throwBuffer[animIndex];

                frisbeeModel.transform.localRotation = location.rot;
				frisbeeModel.transform.localPosition = location.pos;
                current = location;
			}
            rateTimer += Time.deltaTime * speed;
            animIndex = getListIndexFromTime(throwBuffer, rateTimer, animIndex);
		} else {
			animIndex = 1;
			rateTimer = 0F;
            pauseUntil = Time.time + 1.0F;
		}
        /*if (Time.frameCount % 100 == 0)
            Debug.Log("IDX: " + animIndex);*/
	}
}
