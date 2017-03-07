using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Solider_Buff : MonoBehaviour {
	public bool isResuming;

	// Use this for initialization
	void Start () {
		isResuming = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.GetComponent<NavMeshAgent> ().speed == 0 && !isResuming) {
			isResuming = true;
			StartCoroutine ("ChangeMoveSpeedBack");
		}
	}

	IEnumerator ChangeMoveSpeedBack(){
		yield return new WaitForSeconds (2f);
		transform.GetComponent<RoleInfo> ().ResumMoveSpeed();
		yield return null;
	}
}
