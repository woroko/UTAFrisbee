//======================================================================================================
// Clean room implementation of Rigid Body tracking using OptitrackStreamingClient
//======================================================================================================

using System;
using UnityEngine;


public class CustomRigidBody : MonoBehaviour
{
    public OptitrackStreamingClient StreamingClient;
    public Int32 RigidBodyId;
    private OptitrackRigidBodyState rbState = null;

    void Start()
    {
        // null client, find default
        if (this.StreamingClient == null)
        {
            this.StreamingClient = OptitrackStreamingClient.FindDefaultClient();

            // disable if no client found
            if (this.StreamingClient == null)
            {
                Debug.LogError(GetType().FullName + ": No streaming clients found, disabling CustomRigidBody", this);
                this.enabled = false;
                return;
            }
        }
    }


    void Update()
    {
        rbState = StreamingClient.GetLatestRigidBodyState(RigidBodyId);
        if (rbState != null)
        {
            this.transform.localPosition = rbState.Pose.Position;
            this.transform.localRotation = rbState.Pose.Orientation;
        }
    }

    //direct access to OptiTrack data
    FrisbeeLocation getCurrentFrisbeeLocation()
    {
        if (rbState != null)
            return new FrisbeeLocation(rbState.Pose.Orientation, rbState.Pose.Position, Time.time, isSeen());
        else
            return null;
    }
    //TODO: UI Team: Implement isSeen indicator according to #8
    public bool isSeen()
    {
        if (rbState == null || OptitrackHiResTimer.Now().SecondsSince(rbState.DeliveryTimestamp) > 0.1)
            return false;
        else
            return true;
    }
}
