using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLoader : MonoBehaviour {

    public GameObject frisbeeMalli;
    public TextAsset csvTiedosto;
    int EKARIVI = 7;

    List<FrisbeeSijainti> sijaintiLista = new List<FrisbeeSijainti>(); //float[4] kvaternioni
    int animaatioIndeksi = 0;
    Vector3 posSkaala = new Vector3(0.5F, 0.5F, 0.5F);

	// init
	void Start () {
        string[] rivit = csvTiedosto.text.Split('\n');

        for(int i=EKARIVI; i<rivit.Length; i++)
        {
            string[] kentat = rivit[i].Split(',');
            float testi;
            
            try
            {
                //Rotation X,Y,Z,W csv:stä
                Quaternion rot = new Quaternion(float.Parse(kentat[2]),
                float.Parse(kentat[3]), float.Parse(kentat[4]),
                float.Parse(kentat[5]));

                //Position X,Y,Z csv:stä
                Vector3 pos = new Vector3(float.Parse(kentat[6]),
                float.Parse(kentat[7]), float.Parse(kentat[8]));

                sijaintiLista.Add(new FrisbeeSijainti(rot, pos));

            }
            catch
            {
                sijaintiLista.Add(null);
            }
        }

	}
	
	// kutsutaan joka frame
	void Update () {
        if (animaatioIndeksi < sijaintiLista.Count) {
            if (sijaintiLista[animaatioIndeksi] != null)
            {
                FrisbeeSijainti sijainti = sijaintiLista[animaatioIndeksi];
                frisbeeMalli.transform.localRotation = sijainti.rot;
                frisbeeMalli.transform.localPosition = Vector3.Scale(sijainti.pos - new Vector3(0F, 1F, 0F), posSkaala);
            }
            animaatioIndeksi++;
        }
        else
        { animaatioIndeksi = 0;}
	}
}
