using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode ]
public class PathDo : MonoBehaviour {
	private static PathDo _instance;
	public static PathDo Instance{
		get { return _instance;}
	}
	public Transform[] paths;
	void Awake(){
		_instance = this;
		paths = new Transform[transform.childCount];
		for (int i = 0; i < transform .childCount ; i++) {
			paths [i] = transform.GetChild (i);
		}
	}
	void OnDrawGizmos(){
		for (int i = 0; i < paths .Length-1 ; i++) {
			Gizmos.color = Color.red;
			Gizmos.DrawLine (paths [i].position, paths [i + 1].position);
		}
	}
}
