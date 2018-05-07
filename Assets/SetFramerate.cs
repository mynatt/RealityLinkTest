using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFramerate : MonoBehaviour {
	public int framerate = -1;
	// Use this for initialization
	void Start () {
		Application.targetFrameRate = framerate;
	}
	// Update is called once per frame
	void Update () {
	}
}
