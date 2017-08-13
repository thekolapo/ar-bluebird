using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControllerScript : MonoBehaviour {
	public static BirdControllerScript main;
	public Transform target;
	private float rotationSpeed = 0.8f;
	[HideInInspector]
	public Quaternion targetRotation;
	private float movementSpeed = 2f;

	void Awake(){
		main = this;
	}

	void FixedUpdate() {
	   transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }
	
	// Update is called once per frame
	void Update () {
		 targetRotation = Quaternion.LookRotation (target.position - transform.position);
   		 transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	}

	public void ChangeTarget(){
		if(target == null){
			return;
		}

		target = BalloonPoolingScript.main.GetPoppableBalloon().transform;
	}

}
