using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Used to apply debugging movement to RecordingFrisbee from csv
 */
public class DebugMover : MonoBehaviour {

    public GameObject frisbeeModel;
    public TextAsset recording;
    public TextAsset recording2;
    public float speed = 1F;

    // We start from row 7, since there is some junk in the csv
    const int FIRSTROW = 7;

    List<FrisbeeLocation> locationList = new List<FrisbeeLocation>();
    List<FrisbeeLocation> locationList1 = new List<FrisbeeLocation>();
    List<FrisbeeLocation> locationList2 = new List<FrisbeeLocation>();

    int animIndex = 0;
    Vector3 posScale = new Vector3(1.0F, 1.0F, 1.0F);
    float rateTimer = 0F;
    float beginTime = 0F;

    bool moving = false;
    bool switchState = false;

    // Csv parsed to locationList at script init
    void Start () {
        locationList1 = ParseCSVFile(recording);
        locationList2 = ParseCSVFile(recording2);
        locationList = locationList1;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            moving = true;
            animIndex = 0;
            rateTimer = 0F;
            beginTime = Time.time;
            if (switchState)
            {
                locationList = locationList1;
                switchState = !switchState;
            }
            else
            {
                locationList = locationList2;
                switchState = !switchState;
            }
        }
        //Begin steady
        else if (Time.time < beginTime + 2.5F) {
            FrisbeeLocation location = locationList[0];
            frisbeeModel.transform.localRotation = location.rot;
            frisbeeModel.transform.localPosition = Vector3.Scale(location.pos - new Vector3(0F, 0F, 0F), posScale);
        }
        //Update the position from locationList
        else if (moving)
            UpdatePosition();

    }

    // Updates the RecordingFrisbee location from csv file. Scaling and offset left for manual adjustment
    void UpdatePosition()
    {
        if (animIndex < locationList.Count)
        {
            if (locationList[animIndex] != null)
            {
                FrisbeeLocation location = locationList[animIndex];
                frisbeeModel.transform.localRotation = location.rot;
                frisbeeModel.transform.localPosition = Vector3.Scale(location.pos - new Vector3(0F, 0F, 0F), posScale);
            }
            animIndex = (int)(rateTimer / (1/100F)); //presumes 100fps
            rateTimer += Time.deltaTime * speed;
        }
        else
        {
            animIndex = 0;
            rateTimer = 0F;
            moving = false;
        }
    }

    List<FrisbeeLocation> ParseCSVFile(TextAsset rec)
    {
        string[] rows = rec.text.Split('\n');
        List<FrisbeeLocation> list = new List<FrisbeeLocation>();

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

                list.Add(new FrisbeeLocation(rot, pos));

            }
            catch
            {
                list.Add(null);
            }
        }
        return list;
    }
}
