using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraChange : MonoBehaviour {

    public Dropdown dropdown;
    public GameObject mainCamera, topCamera, sideCamera;
    Vector3 mainCamDefPos;
    Vector3 topCamDefPos;
    Vector3 sideCamDefPos;

    void Start()
    {
        mainCamDefPos = mainCamera.transform.position;
        topCamDefPos = topCamera.transform.position;
        sideCamDefPos = sideCamera.transform.position;

    }
    public void Dropdown_IndexChanged(int index)
    {
        if (index == 0)
        {
            mainCamera.SetActive(true);
            mainCamera.transform.position = mainCamDefPos;
            topCamera.SetActive(false);
            sideCamera.SetActive(false);
        }
        else if (index == 1)
        {
            topCamera.SetActive(true);
            topCamera.transform.position = topCamDefPos;
            mainCamera.SetActive(false);
            sideCamera.SetActive(false);
        }
        else if (index == 2)
        {
            sideCamera.SetActive(true);
            sideCamera.transform.position = sideCamDefPos;
            mainCamera.SetActive(false);
            topCamera.SetActive(false);
        }
    }
}
