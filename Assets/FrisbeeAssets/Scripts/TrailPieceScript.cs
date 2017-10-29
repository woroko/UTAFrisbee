﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailPieceScript : MonoBehaviour {

	public GameObject trailTooltip;

	void Start()
	{
		trailTooltip = GameObject.Find ("Trail Tooltip");
	}

	// Mouse hovers over the object
	void OnMouseOver()
	{
		trailTooltip.GetComponent<Text>().text = ("Position: " + transform.localPosition + "\nRotation: " + transform.localRotation);
		trailTooltip.transform.position = new Vector3 (Input.mousePosition.x+80, Input.mousePosition.y+25, Input.mousePosition.z);
		GetComponent<Renderer> ().material.color = Color.green;
	}

	void OnMouseExit()
	{
		trailTooltip.GetComponent<Text>().text = ("");
		GetComponent<Renderer> ().material.color = Color.red;
	} 
}