using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showReticle : MonoBehaviour {


	public GameObject reticuleModel;
	private GameObject reticuleInstance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void spawnReticule(){
		reticuleInstance = Instantiate (reticuleModel, new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
	}
	void deleteReticule(){
		Destroy (reticuleInstance);
	}
}
