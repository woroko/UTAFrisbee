using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour {

    public TrailRenderer trailRenderer;
    public Transform follow;
    bool trailActive = false;

    void Start()
    {
    }

    void Update()
    {

        if (trailActive == true)
        {
            trailRenderer.transform.position = follow.localPosition;
        }

    }

    public void Activate()
    {
        trailActive = true;
    }

    public void Deactivate()
    {
        trailActive = false;
    }

    public void Reset()
    {
        trailRenderer.Clear();
    }
}
