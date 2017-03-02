using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAction : MonoBehaviour {
	public BulletType type;
	public int attack_Physical;
	public int attack_Magic;
	public float moveSpeed;
	public Transform target;
	public Role_Main roleMain;

	public void GetNumber(RoleInfo role){
		this.attack_Physical = role.attack_Physical;
		this.attack_Magic = role.attack_Magic;    
		moveSpeed = 2.5f;
	}
	public void GetNumber(Role_Main role){
		this.roleMain = role;
		this.attack_Physical = role.attack_Physical;
		this.attack_Magic = role.attack_Magic;  
		moveSpeed = 2.6f;
	}
	public void GetNumber(SoliderInfo solidInfo){
		this.attack_Magic = solidInfo.attack_Magic;
		this.attack_Physical = solidInfo.attack_Physical;
		moveSpeed = 1.8f;
	}

	public void GetNumber(YeguaiInfo yeguaiInfo)
	{
		this.attack_Magic = yeguaiInfo.attack_Magic;
		this.attack_Physical = yeguaiInfo.attack_Physical;
		moveSpeed = 1.9f;
	}

	public void SetTarget(Transform target){
		this.target = target;
		StartCoroutine ("Akt");
	}
	public IEnumerator Akt(){
		Vector3 startPos = transform.position;
		float time = 0;
		while (target !=null && Vector3 .Distance (transform .position ,target .position )>0.1f) {
			time += Time.deltaTime;
			transform.position = Vector3.Lerp (startPos, target.position, time * moveSpeed);
			transform.rotation = Quaternion.LookRotation (target.position, transform.position);
			yield return null;
		}
		if (target ==null ) {
			Debug.Log ("目标为空");
			Destroy (this.gameObject);
			yield return null;
		}
		if (target!=null ) {
			if (roleMain != null) {
				target.GetComponent <RoleInfo> ().GetHurt (attack_Physical, attack_Magic, roleMain);
			} else {
				target.GetComponent <RoleInfo> ().GetHurt (attack_Physical, attack_Magic);	
			}
			Destroy (this.gameObject);
			yield return null;
		}
	}

}
public enum BulletType{
	bullet_Role,
	bullet_solider
}