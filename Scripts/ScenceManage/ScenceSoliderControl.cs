using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceSoliderControl : MonoBehaviour {
	private static ScenceSoliderControl _instance;
	public static ScenceSoliderControl Instance{
		get { return _instance;}
	}
	// Use this for initialization
	void Start () {
		_instance = this;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
