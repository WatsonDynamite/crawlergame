using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 2f;
    public float runSpeed = 5f;
    public float turnSmoothing = 15f;

    public Camera camera;

    public Animator animator;

    public CharacterController characterController;
    
	private bool moving = false;
    private Vector3 movement;
    private Rigidbody playerRigidBody;
	private string currentAnim = "";
	private bool targeting;
    void Awake()
    {
		
		animator.Play ("Idle");
        playerRigidBody = GetComponent<Rigidbody>();
    }

	void Update(){
		targeting = GetComponent<PlayerTargetScript> ().getWhetherTargeting ();
	}

    void FixedUpdate()
    {
        float lh = Input.GetAxisRaw("Horizontal");
        float lv = Input.GetAxisRaw("Vertical");
        Move(lh, lv);
    }


    void Move(float lh, float lv)
    {
		//MOVEMENT WHEN NOT TARGETING
		if (!targeting) {
			movement.Set (lh, 0f, lv);
			if (lh == 0 && lv == 0) {
				if (currentAnim != "Idle") {
					currentAnim = "Idle";
					animator.CrossFade (currentAnim, 0.2f);
				} 
			} else {
			
				if (currentAnim != "Run") {
					currentAnim = "Run";
					animator.CrossFade (currentAnim, 0.2f);
				}
			}
			movement = camera.transform.TransformDirection (movement);
			movement.y = 0f;

			/*
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement = movement.normalized * runSpeed * Time.deltaTime;
        }
        else */
			{
				movement = movement.normalized * speed * Time.deltaTime;
			}

			playerRigidBody.MovePosition (transform.position + movement);


			if (lh != 0f || lv != 0f) {
				Rotating (lh, lv);
			}

		}
		//MOVEMENT WHEN TARGETING
		else {

		}
			
    }


    void Rotating(float lh, float lv)
    {
        Vector3 targetDirection = new Vector3(lh, 0f, lv);

        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);

        Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);

        GetComponent<Rigidbody>().MoveRotation(newRotation);

		//TODO: obtain rotation and blend the correct turn animation

    }

}