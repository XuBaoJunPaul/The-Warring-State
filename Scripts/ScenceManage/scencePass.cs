using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scencePass : MonoBehaviour {
	public static bool IsGameOver;
	// Use this for initialization
	void Start () {
		IsGameOver = false;
		GameObject.DontDestroyOnLoad (this .gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
