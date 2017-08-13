using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPoolingScript : MonoBehaviour {
	public static BalloonPoolingScript main;
	public GameObject balloon;
	public List<GameObject> balloonList;
	public Transform imageTarget;
	public List<Color> colorList;

	// Use this for initialization
	void Awake () {
		main = this;
		SpawnBalloons();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnBalloons(){
		for(int i = 0; i < 4; i++){
			GameObject obj = Instantiate(balloon);
			Vector3 objPos = new Vector3(Random.Range(-1.4f, 1.4f), Random.Range(-0.5f, 0.6f), Random.Range(-0.9f, 1f));
			obj.transform.parent = imageTarget;
			obj.transform.localScale = balloon.transform.localScale;
			balloon.transform.localPosition = objPos;
			balloonList.Add(obj);
		}	
		
	}

	public GameObject GetPoppableBalloon(){
		foreach(GameObject balloon in balloonList){
			if(balloon.activeInHierarchy && balloon.GetComponent<BalloonScript>().isPoppable){
				return balloon;
			}
		}

		return null;

	}

	public void RemoveBalloon(GameObject obj){
		balloonList.Remove(obj);
	}

	public void AddBalloon(GameObject obj){
		balloonList.Add(obj);
	}

	

}
