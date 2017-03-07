using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YangJianSkillControl : MonoBehaviour {
	public Role_Camp camp;
	public float radius;
	public Collider[] col;
	public Role_Main owner;
	// Use this for initialization
	void Start () {
		StartCoroutine ("DestoryIt");
		radius = 4f;
	}
	
	// Update is called once per frame
	void Update () {
		col = Physics.OverlapSphere (transform.position, radius,LayerMask.GetMask(GetOtherCamp(camp)));
		if (col != null) {
			for (int i = 0; i < col.Length; i++) {
				if (col [i].gameObject.GetComponent<RoleInfo> ()!= null)
//					col [i].gameObject.GetComponent<RoleInfo> ().Hp -= 10;
					col[i].gameObject.GetComponent<RoleInfo>().GetHurt(5,5,owner);
			}	
		}
	}

	IEnumerator DestoryIt(){
		yield return new WaitForSeconds (5f);
		Destroy (this.gameObject);
	}

	string GetOtherCamp(Role_Camp cam){
		if (cam == Role_Camp.Blue)
			return "Red";
		else
			return "Blue";
	}

	void OnDrawGizmos(){
		Gizmos.color = new Color(1, 0, 0, 1);
		Gizmos.DrawSphere (transform.position, radius);
	}
}
