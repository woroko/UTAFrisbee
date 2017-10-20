using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLoader : MonoBehaviour {

    public GameObject frisbeeModel;
    public TextAsset recording;
    public float speed = 1F;

	//FOR TESTING
	public ThrowController throwController;
    
	const int FIRSTROW = 7; //csv gimmick

    List<FrisbeeLocation> locationList = new List<FrisbeeLocation>(); //float[4] kvaternioni
    int animIndex = 0;
    Vector3 posScale = new Vector3(0.5F, 0.5F, 0.5F);
    float rateTimer = 0F;

	//TESTING VARIABLE
	private int temp;

	// init
	void Start () {
		ParseCSVFile ();
		temp = locationList.Count;
	}
	
	// called every frame
	void Update () {
		// FOR TESTING
		if (!throwController.InPlayback ()) {
			
			UpdatePosition ();
			temp = locationList.Count;

		} else {
			Debug.Log ("In Playback now");
			animIndex = temp;

			if (temp++>=locationList.Count)
				animIndex = temp = 0;
			
			UpdatePosition ();
		}
	}


	void UpdatePosition () {
		if (animIndex < locationList.Count) {
			if (locationList [animIndex] != null) {
				FrisbeeLocation location = locationList [animIndex];
				frisbeeModel.transform.localRotation = location.rot;
				frisbeeModel.transform.localPosition = Vector3.Scale (location.pos - new Vector3 (0F, 1F, 0F), posScale);
			}
			animIndex = (int)(rateTimer / 0.01F);
			rateTimer += Time.deltaTime * speed;
		} else {
			animIndex = 0;
			rateTimer = 0F;
		}
	}



	void ParseCSVFile () {
		string[] rows = recording.text.Split('\n');

		for(int i=FIRSTROW; i<rows.Length; i++)
		{
			string[] cols = rows[i].Split(',');

			try
			{
				//Rotation X,Y,Z,W from csv
				Quaternion rot = new Quaternion(float.Parse(cols[2]),
					float.Parse(cols[3]), float.Parse(cols[4]),
					float.Parse(cols[5]));

				//Position X,Y,Z from csv
				Vector3 pos = new Vector3(float.Parse(cols[6]),
					float.Parse(cols[7]), float.Parse(cols[8]));

				locationList.Add(new FrisbeeLocation(rot, pos));

			}
			catch
			{
				locationList.Add(null);
			}
		}
	}
}
