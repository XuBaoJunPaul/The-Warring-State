using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .AI ;
using UnityEditorInternal;
//[ExecuteInEditMode]
public class SoliderAI_3Vs3_z : MonoBehaviour {
	private Role_Camp camp;
	public Transform target;
	public Collider[] targets;
	public float minDistance=0.5f;
	private float atkRadius;
	float atkInterval=1.5f;
	public GameObject bullet;
	private LayerMask layerEnemy;
	float timeCount=0f;
	public int index=0;
	public Transform[] paths;
	private NavMeshAgent  soliderAgent;
	private Animator ani; 
//	private AnimatorController ac = RuntimeAnimatorController as AnimatorController;  控制单个动画时使用
//	private StateMachine sm=ac.GetLayer(0).stateMachine;

	void Start(){
		camp = GetComponent <RoleInfo> ().roleCamp;
		paths = ChoicePaths (camp);
		layerEnemy = GetComponent <RoleInfo> ().EnemyLayer;
		soliderAgent = transform.GetComponent <NavMeshAgent> ();
		ani = GetComponent<Animator> ();


		if (paths != null  && paths .Length >0) {
			target = paths[index];
		}
//		bullet =Resources .Load <GameObject>("Prefeb_Role/bullet_S");
	}
	void Update(){
		AI_Move ();
	}
	void OnDrawGizmos(){
//		Gizmos.color = new Color (0f,0f,1f,0.3f);
//		atkRadius = info.attack_Radius;
//		Gizmos.DrawSphere (transform.position, atkRadius*2.5f);
	}
	public void AI_Move(){
		timeCount += Time.deltaTime;
		atkRadius = transform.GetComponent <SoliderInfo> ().attack_Radius;
		Collider[] cols = Physics.OverlapSphere (transform.position, atkRadius*2.5f, layerEnemy);
		targets = cols;
		if(index <paths.Length -1) {
			//Debug.Log ("当前位置，目标点的位置"+transform.position+target .transform .name +target .position);
			if (Vector3 .Distance (transform .position ,target .position )<1.5f) {

				index++;
				target =paths [index].transform;
			}
		}
		if(cols.Length ==0 ) {   //没有目标         //？？？？？？？？？需要加一个判断，如果过了目标点就向下个目标前进
			Vector3 moveDir = target.position - transform.position;
			moveDir.y = 0f;
			if(Vector3 .Distance (transform .position ,target .position )>minDistance  ) {
				soliderAgent.SetDestination (target.position);
			} 
		}
		if(cols .Length >0) {                         //有目标
			float minDistance = Vector3.Distance (transform.position, cols [0].transform.position);
			int chioce=0;
			for (int i = 0; i < cols.Length ; i++) {
				float currentDis = Vector3.Distance (transform.position, cols [i].transform.position);
				if (currentDis <minDistance ) {
					chioce = i;
				}
			}
			soliderAgent.SetDestination (cols [chioce].transform.position);
			if (Vector3 .Distance (transform .position ,cols [chioce].transform.position )< (atkRadius-0.5f)) {
				soliderAgent.SetDestination(transform .position);
			}
			if (timeCount >atkInterval && Vector3 .Distance (transform .position ,cols [chioce].transform.position )<atkRadius) {
				ani.SetTrigger ("CanAtk");
				Vector3 lookTemp = cols [chioce].transform.position - transform.position;
				lookTemp.y = 0f;
				transform.rotation = Quaternion.LookRotation (lookTemp);
				GameObject bulletSolider=Instantiate(bullet ,transform.Find("Weapon").position,Quaternion .identity )as GameObject ;
				bulletSolider.GetComponent <BulletAction> ().GetNumber ( GetComponent<SoliderInfo> () );
				bulletSolider.GetComponent <BulletAction> ().SetTarget (cols [chioce].transform);
				timeCount = 0;
			}
			
		}
	}//AI_move

	public Transform[] ChoicePaths(Role_Camp camp1){
		Transform path=GameObject .Find ("PathSolider_z").transform ;
		Transform[] path2 = new Transform[path.childCount];
		if (camp1 == Role_Camp.Red) {
			for (int i = 0; i < path.childCount ; i++) {
				path2 [i] = path.GetChild (i);
			}
		} 
		else if (camp1 ==Role_Camp.Blue) {
			for (int i = 0; i < path.childCount ; i++) {
				path2 [i] = path.GetChild(path.childCount-i-1);
			}
		} 
		return path2;
	}
}
