using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeSijainti{
    public Quaternion rot;
    public Vector3 pos;

    public FrisbeeSijainti(Quaternion rot, Vector3 pos)
    {
        this.rot = rot;
        this.pos = pos;
    }
}
