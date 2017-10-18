using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraChange : MonoBehaviour {

    public Dropdown dropdown;
    public GameObject mainCamera, topCamera, sideCamera;

    public void Dropdown_IndexChanged(int index)
    {
        if (index == 0)
        {
            mainCamera.SetActive(true);
            topCamera.SetActive(false);
            sideCamera.SetActive(false);
        }
        else if (index == 1)
        {
            topCamera.SetActive(true);
            mainCamera.SetActive(false);
            sideCamera.SetActive(false);
        }
        else if (index == 2)
        {
            sideCamera.SetActive(true);
            mainCamera.SetActive(false);
            topCamera.SetActive(false);
        }
    }
}
