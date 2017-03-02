using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class YeguaiAI : MonoBehaviour {

	public float moveSpeed=3f;

	public Transform target;
	public Collider[] targets;
	public float minDistance=0.5f;
	private float atkRadius;
	float atkInterval=1.5f;
	public GameObject bullet;
	private LayerMask layerEnemy;
	float timeCount=0f;
	public int index=0;
	private NavMeshAgent  YeguaiAgent;
	private Animator ani; 
	private AnimatorStateInfo aniInfo;
	//	private AnimatorController ac = RuntimeAnimatorController as AnimatorController;  控制单个动画时使用
	//	private StateMachine sm=ac.GetLayer(0).stateMachine;
	public Transform bornPos;

	void Start(){
		//		aniInfo = GetComponent <AnimatorStateInfo> ();

		layerEnemy = LayerMask.GetMask("Red") | LayerMask.GetMask("Blue");
		YeguaiAgent = transform.GetComponent <NavMeshAgent> ();
		ani = GetComponent<Animator> ();
		target = bornPos;


//				bullet =Resources .Load <GameObject>("Prefeb_Role/bullet_S");
	}
	void Update(){

		AI_Move ();
		aniInfo = ani.GetCurrentAnimatorStateInfo (0);
	}
	void OnDrawGizmos(){
				Gizmos.color = new Color (0f,0f,1f,0.3f);
				atkRadius = 2f;
				Gizmos.DrawSphere (transform.position, atkRadius*2.5f);
	}
	public void AI_Move(){
		timeCount += Time.deltaTime;
		atkRadius = transform.GetComponent <YeguaiInfo> ().attack_Radius;
		Collider[] cols = Physics.OverlapSphere (transform.position, atkRadius,layerEnemy);
			//Debug.Log ("当前位置，目标点的位置"+transform.position+target .transform .name +target .position);

		if(cols.Length ==0) {   //没有目标         //？？？？？？？？？需要加一个判断，如果过了目标点就向下个目标前进
				Vector3 moveDir = target.position - transform.position;
				moveDir.y = 0f;
				if(Vector3 .Distance (transform .position ,target .position )>minDistance  ) {
				YeguaiAgent.SetDestination (bornPos.position); //丢失目标回到原点
				} 
			}
			if(cols .Length >0) {                         //有目标
				Debug.Log (cols [0].transform.name);
				float minDistance = Vector3.Distance (transform.position, cols [0].transform.position);
				int chioce=0;
				for (int i = 0; i < cols.Length ; i++) {
					float currentDis = Vector3.Distance (transform.position, cols [i].transform.position);
					if (currentDis <minDistance ) {
						chioce = i;
					}
				}
				YeguaiAgent.SetDestination (cols [chioce].transform.position);

				if (Vector3 .Distance (transform .position ,cols [chioce].transform.position )< (atkRadius-0.5f)) {
				YeguaiAgent.SetDestination(transform .position);
				}
				if (timeCount >atkInterval && Vector3 .Distance (transform .position ,cols [chioce].transform.position )<atkRadius) {
					ani.SetTrigger ("CanAtk");
					Vector3 lookTemp = cols [chioce].transform.position - transform.position;
					lookTemp.y = 0f;
					transform.rotation = Quaternion.LookRotation (lookTemp);
					GameObject bulletSolider=Instantiate(bullet ,transform.Find("Weapon").position,Quaternion .identity )as GameObject ;
				bulletSolider.GetComponent <BulletAction> ().GetNumber ( GetComponent<YeguaiInfo> () );
					bulletSolider.GetComponent <BulletAction> ().SetTarget (cols [chioce].transform);
					timeCount = 0;
				}


		}
	}//AI_move
		
}
