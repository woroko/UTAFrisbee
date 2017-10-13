using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLoader : MonoBehaviour {

    public GameObject frisbeeModel;
    public TextAsset recording;
    public float SPEEDFACTOR = 1F;
    int FIRSTROW = 7;

    List<FrisbeeLocation> locationList = new List<FrisbeeLocation>(); //float[4] kvaternioni
    int animIndex = 0;
    Vector3 posScale = new Vector3(0.5F, 0.5F, 0.5F);
    float rateTimer = 0F;


	// init
	void Start () {
        string[] rows = recording.text.Split('\n');

        for(int i=FIRSTROW; i<rows.Length; i++)
        {
            string[] kentat = rows[i].Split(',');
            
            try
            {
                //Rotation X,Y,Z,W csv:stä
                Quaternion rot = new Quaternion(float.Parse(kentat[2]),
                float.Parse(kentat[3]), float.Parse(kentat[4]),
                float.Parse(kentat[5]));

                //Position X,Y,Z csv:stä
                Vector3 pos = new Vector3(float.Parse(kentat[6]),
                float.Parse(kentat[7]), float.Parse(kentat[8]));

                locationList.Add(new FrisbeeLocation(rot, pos));

            }
            catch
            {
                locationList.Add(null);
            }
        }

	}
	
	// called every frame
	void Update () {
        if (animIndex < locationList.Count) {
            if (locationList[animIndex] != null)
            {
                FrisbeeLocation location = locationList[animIndex];
                frisbeeModel.transform.localRotation = location.rot;
                frisbeeModel.transform.localPosition = Vector3.Scale(location.pos - new Vector3(0F, 1F, 0F), posScale);
            }
            animIndex = (int)(rateTimer / 0.01F);
            rateTimer += Time.deltaTime*SPEEDFACTOR;
        }
        else
        {
            animIndex = 0;
            rateTimer = 0F;
        }
	}
}
