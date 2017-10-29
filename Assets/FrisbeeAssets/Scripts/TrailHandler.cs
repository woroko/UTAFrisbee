using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour {

    //public TrailRenderer trailRenderer;
    public Transform follow;
    bool trailActive = false;

	public float trailSize;
	List<GameObject> trailPieces;

    void Start()
    {
		trailPieces = new List<GameObject>();
    }

    void Update()
    {

        if (trailActive == true)
        {
			GameObject trailPiece = GameObject.CreatePrimitive (PrimitiveType.Sphere);

			trailPiece.transform.localScale = new Vector3(trailSize, trailSize, trailSize);
			trailPiece.transform.position = follow.localPosition;

			trailPiece.GetComponent<Renderer> ().material.color = Color.red;
			trailPiece.AddComponent<TrailPieceScript> ();

			trailPieces.Add(trailPiece);

			//trailRenderer.transform.position = follow.localPosition;
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
		trailPieces.ForEach (delegate(GameObject piece) {
			Destroy (piece);
		});

		trailPieces.Clear ();

        //trailRenderer.Clear();
    }
}
