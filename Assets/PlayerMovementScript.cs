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
    
    
	private bool moving = false;
    private Vector3 movement;
    private Rigidbody playerRigidBody;
	private string currentAnim = "";
	private bool targeting;
    private GameObject target;
    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

	void Update(){
		targeting = GetComponent<PlayerTargetScript> ().getWhetherTargeting ();
        target = GetComponent<PlayerTargetScript>().getCurrentTarget();
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
        if (!targeting)
        {
            movement.Set(lh, 0f, lv);
            if (lh == 0 && lv == 0)
            {
                if (currentAnim != "Idle") 
                {
                    currentAnim = "Idle";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }
            else
            {
                if (currentAnim != "Run")
                {
                    currentAnim = "Run";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }

            movement = camera.transform.TransformDirection(movement);
            movement.y = 0f;

            if (lh != 0f || lv != 0f)
            {
                Rotating(lh, lv);
            }

        }
        //MOVEMENT WHEN TARGETING
        else
        {
			if (Input.GetKeyDown (KeyCode.Space)) {


				return;
			}
            Vector3 rotation = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles;
            rotation = new Vector3(transform.eulerAngles.x, rotation.y, transform.eulerAngles.z);
            Quaternion rot = Quaternion.Euler(rotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * 3);
            movement.Set(lh, 0f, lv);
            if (lh == 0 && lv == 0)
            {
                if (currentAnim != "Idle")//change this to targeting idle
                {
                    currentAnim = "Idle";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }
            else if (lh >= 0 && lv == 0) //strafe to the right
            {
                if (currentAnim != "Strafe Right")
                {
                    currentAnim = "Strafe Right";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }
            else if (lh >= 0 && lv >= 0) //strafe forward right
            {
                if (currentAnim != "Strafe Forward Right")
                {
                    currentAnim = "Strafe Forward Right";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }
            else if (lh == 0 && lv >= 0) //strafe forward
            {
                if (currentAnim != "Strafe Forward")
                {
                    currentAnim = "Strafe Forward";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }
            else if (lh <= 0 && lv >= 0) //strafe forward left
            {
                if (currentAnim != "Strafe Forward Left")
                {
                    currentAnim = "Strafe Forward Left";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }
            else if (lh <= 0 && lv == 0) //strafe left
            {
                if (currentAnim != "Strafe Left")
                {
                    currentAnim = "Strafe Left";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }
            else if (lh <= 0 && lv <= 0) //strafe backward left
            {
                if (currentAnim != "Strafe Backward Left")
                {
                    currentAnim = "Strafe Backward Left";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }
            else if (lh == 0 && lv <= 0) //strafe backward
            {
                if (currentAnim != "Strafe Backward")
                {
                    currentAnim = "Strafe Backward";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }
            else if (lh >= 0 && lv <= 0) //strafe backward right
            {
                if (currentAnim != "Strafe Backward Right")
                {
                    currentAnim = "Strafe Backward Right";
                    animator.CrossFade(currentAnim, 0.2f);
                }
            }


            movement = -(camera.transform.TransformDirection(-movement));
            movement.y = 0f;

        }
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidBody.MovePosition(transform.position + movement);
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