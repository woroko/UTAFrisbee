using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeLocation{
    public Quaternion rot;
    public Vector3 pos;
    public float time = 0F;

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
}
