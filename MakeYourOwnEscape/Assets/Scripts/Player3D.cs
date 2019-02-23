using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Controller3D))]
public class Player3D : MonoBehaviour {

	private Controller3D controller;

	private Vector3 velocity;

	private Animator animator;

/*
	[SerializeField]
	private CrossPlatformInputManager inputManager;*/

	[SerializeField]
	private float maxSpeed = 18f;
	[SerializeField]
	private float VelocityAcceleration = 8f, rotationAcceleration = 3f, visionDistance = 1f;

	[SerializeField]
	private float rotateSpeed = 0.1f, step = 90f, removeStep = 180f, maxStep = 40f;

	

	[SerializeField]
    private float gravity, maxJumpVelocity, minJumpVelocity;

	[SerializeField]
	private bool useKeyBoard = false, _break;

    private bool mouseDown = false;

	private Vector3 targetOrientation;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller3D>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            mouseDown = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            mouseDown = false;
        }

        bool jumpPressed, jumpUnPressed;
		if(!useKeyBoard){
			Vector3 direction_joystick = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"),0f, CrossPlatformInputManager.GetAxis("Vertical"));
            jumpPressed = CrossPlatformInputManager.GetButtonDown("Jump");
            jumpUnPressed = CrossPlatformInputManager.GetButtonUp("Jump");
            //Debug.Log("direction_joystick : " + direction_joystick);
            if (direction_joystick.magnitude > 0.1f && mouseDown){
				Vector3 Z_prime = Camera.main.cameraToWorldMatrix.MultiplyVector(Vector3.zero);
				//Vector3 axis = Vector3.Cross(transform.up, Z_prime);
				//float angle = Vector3.Angle(transform.up, Z_prime);
                //transform.RotateAround(transform.position, axis, angle);
                
                //Vector3 xy_p = Camera.main.cameraToWorldMatrix.MultiplyVector(direction_joystick);
                Vector3 xy_p = direction_joystick;

                float angle2 = Vector3.Angle(transform.forward, xy_p - Z_prime);

                Debug.DrawRay(Z_prime, xy_p - Z_prime, Color.gray);
                Debug.DrawRay(Z_prime, transform.forward, Color.cyan);

                Debug.DrawLine(Camera.main.transform.position, Z_prime, Color.yellow);
                Debug.DrawLine(Camera.main.transform.position, xy_p, Color.blue);

                //Vector3 actuallyLookingPoint = Vector3.zero - transform.forward + Z_prime;
                //Vector3 direction = xy_p - transform.forward;
                //Debug.Log("actually : " + transform.forward + " dir : " + xy_p);
                bool _isLeft = isLeft(-transform.forward, transform.forward, xy_p);
                if (_isLeft)
                {
                    angle2 = -2 * angle2 / 5;
                }
                else
                {
                    angle2 = 2 * angle2 / 5;
                }

                //Debug.Log("isLeft : " + _isLeft + " | angle 2 : " + angle2 + " | xy_p : " + xy_p);
                if(_break)
                    Debug.Break();

                transform.RotateAround(transform.position, transform.up, angle2);

				float speed = VelocityAcceleration * direction_joystick.magnitude;
				velocity = new Vector3(0f,0f, speed);
				speed = 0f;
			}
			//else
				//Debug.Log("No input : " + direction_joystick.magnitude);
		}else{
			if(Input.GetKey(KeyCode.DownArrow)){
				velocity.z = velocity.z - VelocityAcceleration > -maxSpeed ? velocity.z - VelocityAcceleration : -maxSpeed;
			}
			if(Input.GetKey(KeyCode.UpArrow)){
				velocity.z = velocity.z + VelocityAcceleration < maxSpeed ? velocity.z + VelocityAcceleration : maxSpeed;
			}
			float targetOrientation_Y = transform.localEulerAngles.y;
			if(Input.GetKey(KeyCode.RightArrow)){
				//velocity.z = velocity.z + acceleration < maxSpeed ? velocity.z + acceleration : maxSpeed;
				
				targetOrientation_Y -= rotationAcceleration;
			}
			if(Input.GetKey(KeyCode.LeftArrow)){
				//velocity.z = velocity.z - acceleration > -maxSpeed ? velocity.z - acceleration : -maxSpeed;
				targetOrientation_Y += rotationAcceleration;
            }
            jumpPressed = Input.GetKeyDown(KeyCode.Space);
            jumpUnPressed = Input.GetKeyUp(KeyCode.Space);
            targetOrientation = new Vector3(transform.localEulerAngles.x,targetOrientation_Y, transform.localEulerAngles.z);
			

			transform.localRotation = Quaternion.Euler(targetOrientation.x, targetOrientation.y, targetOrientation.z);

			//Debug.Log("Actual TargetOrientation : " + targetOrientation);
			//Quaternion RotationTo = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(targetOrientation.x, targetOrientation.y, targetOrientation.z), Time.deltaTime * rotateSpeed);
			//Debug.Log("Actual rotation : " + transform.localEulerAngles + " | to : " + RotationTo * Vector3.forward);
			//targetOrientation = transform.localEulerAngles;
		}

        if (jumpPressed && controller.collisions.below)
        {
            animator.SetTrigger("Jump");
            velocity.y = maxJumpVelocity;
        }

        if (jumpUnPressed)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        if (!controller.collisions.below) velocity.y += gravity * Time.deltaTime;
		else if(!jumpPressed) velocity.y = 0;
		
		animator.SetFloat("speed", velocity.z);
		controller.Move(velocity * Time.deltaTime);
 
		velocity = velocity / 3;
	}

    public bool isLeft(Vector3 a, Vector3 b, Vector3 c)
    {
        return ((b.x - a.x) * (c.z - a.z) - (b.z - a.z) * (c.x - a.x)) > 0;
    }
}
