using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeLocation{
    public Quaternion rot;
    public Vector3 pos;

    public FrisbeeLocation(Quaternion rot, Vector3 pos)
    {
        this.rot = rot;
        this.pos = pos;
    }
}
