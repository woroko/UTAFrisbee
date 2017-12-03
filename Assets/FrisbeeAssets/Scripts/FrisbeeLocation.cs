using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeLocation{
    public Quaternion rot;
    public Vector3 pos;
    public float time = 0F;
    public float forwardSpeed = 0F;
    public float rotSpeed = 0F;
    public bool wasSeen = true;

    public FrisbeeLocation()
    {
        this.rot = new Quaternion(0F, 0F, 0F, 0F);
        this.pos = new Vector3(0F, 0F, 0F);
        this.time = 0F;
    }

    public FrisbeeLocation(Quaternion rot, Vector3 pos)
    {
        this.rot = rot;
        this.pos = pos;
    }
    public FrisbeeLocation(Quaternion rot, Vector3 pos, float time)
    {
        this.rot = rot;
        this.pos = pos;
        this.time = time;
    }
    public FrisbeeLocation(Quaternion rot, Vector3 pos, float time, bool wasSeen)
    {
        this.rot = rot;
        this.pos = pos;
        this.time = time;
        this.wasSeen = wasSeen;
    }
    public FrisbeeLocation(Quaternion rot, Vector3 pos, float time, float forwardSpeed, float rotSpeed, bool wasSeen)
    {
        this.rot = rot;
        this.pos = pos;
        this.time = time;
        this.forwardSpeed = forwardSpeed;
        this.rotSpeed = rotSpeed;
        this.wasSeen = wasSeen;
    }
}
