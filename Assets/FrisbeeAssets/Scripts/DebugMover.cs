using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Used to apply debugging movement to RecordingFrisbee from csv
 */
public class DebugMover : MonoBehaviour {

    public GameObject frisbeeModel;
    public TextAsset recording;
    public float speed = 1F;

    const int FIRSTROW = 7;

    List<FrisbeeLocation> locationList = new List<FrisbeeLocation>(); //float[4] kvaternioni
    int animIndex = 0;
    Vector3 posScale = new Vector3(1.0F, 1.0F, 1.0F);
    float rateTimer = 0F;
    float beginTime = 0F;

    bool moving = false;
    //bool beginning = true;



    // Use this for initialization
    void Start () {
        ParseCSVFile();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            moving = true;
            animIndex = 0;
            rateTimer = 0F;
            beginTime = Time.time;
        }
        //Begin steady
        else if (Time.time < beginTime + 2.5F) {
            FrisbeeLocation location = locationList[0];
            frisbeeModel.transform.localRotation = location.rot;
            frisbeeModel.transform.localPosition = Vector3.Scale(location.pos - new Vector3(0F, 1F, 0F), posScale);
        }
        else if (moving)
            UpdatePosition();

    }

    void UpdatePosition()
    {
        if (animIndex < locationList.Count)
        {
            if (locationList[animIndex] != null)
            {
                FrisbeeLocation location = locationList[animIndex];
                frisbeeModel.transform.localRotation = location.rot;
                frisbeeModel.transform.localPosition = Vector3.Scale(location.pos - new Vector3(0F, 1F, 0F), posScale);
            }
            animIndex = (int)(rateTimer / 0.01F);
            rateTimer += Time.deltaTime * speed;
        }
        else
        {
            animIndex = 0;
            rateTimer = 0F;
            moving = false;
        }
    }

    void ParseCSVFile()
    {
        string[] rows = recording.text.Split('\n');

        for (int i = FIRSTROW; i < rows.Length; i++)
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
