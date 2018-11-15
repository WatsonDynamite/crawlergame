using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTargetScript : MonoBehaviour {


	private bool targeting = false;
	public Text targetText;
	private GameObject[] arrayOfTargetables;
	private GameObject currentTarget;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		arrayOfTargetables = GameObject.FindGameObjectsWithTag ("Targetable");

		//we're always chekcing which object is closest by
		float closestDistance = float.MaxValue;
		GameObject closestObject = null;
		foreach (GameObject target in arrayOfTargetables) {
			if (Vector3.Distance (transform.position, target.transform.position) < closestDistance) {
				closestObject = target;
				closestDistance = Vector3.Distance (transform.position, target.transform.position);
			}
		}

		if (Input.GetKey (KeyCode.Q)) {
			toggletargeting ();
			//IF WE'RE TARGETING:
			if (targeting) {
				targetText.text = "Targeting:" + closestObject.name; //remove after debug
			}
		}
			

	}

	public bool getWhetherTargeting(){
		return targeting;
	}

	public void toggletargeting(){
		targeting = !targeting;	
		if (!targeting) {
			targetText.text = "Targeting: ";
		}
	}

}