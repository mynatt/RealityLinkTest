using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawFPSText : MonoBehaviour {

	protected int ticks = 0;
	protected float accumulator = 0.0f;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		accumulator += Time.deltaTime;
		ticks++;
		if (accumulator >= 1) {
			this.GetComponent<Text>().text = Mathf.RoundToInt(ticks / accumulator).ToString() + " FPS";
		}
	}
}
