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
    
  
	public bool dodging = false;
    private Vector3 movement;
    private Rigidbody playerRigidBody;
	private string currentAnim = "";
	private bool targeting;
    private GameObject target;
    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

	void Update(){
		targeting = GetComponent<PlayerTargetScript> ().getWhetherTargeting ();
        target = GetComponent<PlayerTargetScript>().getCurrentTarget();
        animator.SetBool("isTargeting", targeting);
        dodging = animator.GetBool("isDodging");

    }

    void FixedUpdate()
    {
        float lh = Input.GetAxisRaw("Horizontal");
        float lv = Input.GetAxisRaw("Vertical");
        Move(lh, lv);
        animator.SetFloat("MovementH", lh, 1f, Time.deltaTime * 10f);
        animator.SetFloat("MovementV", lv, 1f, Time.deltaTime * 10f);
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
            if (!animator.GetBool("isDodging"))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetBool("isDodging", true);
                    movement.Set(5f, 0f, 0f);
                    
                }

                Vector3 rotation = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles;
                rotation = new Vector3(transform.eulerAngles.x, rotation.y, transform.eulerAngles.z);
                Quaternion rot = Quaternion.Euler(rotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * 3);
                movement.Set(lh, 0f, lv);



                movement = -(camera.transform.TransformDirection(-movement));
                movement.y = 0f;
            }
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

    public void setNoLongerDodging()
    {
        dodging = false;
    }
}