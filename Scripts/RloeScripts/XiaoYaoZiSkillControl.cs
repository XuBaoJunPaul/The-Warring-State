using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaoYaoZiSkillControl : MonoBehaviour {
	public Role_Main owner;
	public Role_Camp camp;
	public float moveSpeed;
	// Use this for initialization
	void Start () {
		moveSpeed = 0.3f;
		StartCoroutine ("DestoryIt");
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (transform.forward * moveSpeed, Space.World);
	}

	IEnumerator DestoryIt(){
		yield return new WaitForSeconds (2f);
		Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider col){
		if (col != null && col.gameObject.GetComponent<RoleInfo> () != null) {
			if (camp != col.gameObject.GetComponent<RoleInfo> ().roleCamp) {
				//				col.gameObject.GetComponent<RoleInfo> ().Hp -= 199;
				col.gameObject.GetComponent<RoleInfo>().GetHurt(100,90,owner);
			} 
		}
	}
}
