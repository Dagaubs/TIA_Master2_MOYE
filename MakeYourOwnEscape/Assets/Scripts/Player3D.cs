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
	private float rotateSpeed = 0.1f, step = 90f, removeStep = 180f, maxStep = 40f, timeToRecover;

	

	[SerializeField]
    private float gravity, maxJumpVelocity, minJumpVelocity;

	[SerializeField]
	private bool useKeyBoard = false, _break;

    private bool mouseDown = false, inAir = false, justLanded = false;

    public bool gameRunning = false;

	private Vector3 targetOrientation;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller3D>();
		animator = GetComponent<Animator>();
	}

    public void SpawnPlayer()
    {
        // TODO : ANIMATION SPAWN
        gameRunning = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameRunning)
        {
            /*
            if (Input.GetMouseButtonDown(1))
            {
                mouseDown = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                mouseDown = false;
            }
            */
            bool jumpPressed, jumpUnPressed;
            float angle2 = 0f;
            if (!useKeyBoard)
            {
                Vector3 direction_joystick = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), 0f, CrossPlatformInputManager.GetAxis("Vertical"));
                jumpPressed = CrossPlatformInputManager.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space);
                jumpUnPressed = CrossPlatformInputManager.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Space);
                if (direction_joystick.magnitude > 0.1f /*&& mouseDown*/)
                {
                    Vector3 Z_prime = Camera.main.cameraToWorldMatrix.MultiplyVector(Vector3.zero);
                    //Vector3 axis = Vector3.Cross(transform.up, Z_prime);
                    //float angle = Vector3.Angle(transform.up, Z_prime);
                    //transform.RotateAround(transform.position, axis, angle);

                    //Vector3 xy_p = Camera.main.cameraToWorldMatrix.MultiplyVector(direction_joystick);
                    Vector3 xy_p = direction_joystick;

                    angle2 = Vector3.Angle(transform.forward, xy_p - Z_prime);
                    /*
                    Debug.DrawRay(Z_prime, xy_p - Z_prime, Color.gray);
                    Debug.DrawRay(Z_prime, transform.forward, Color.cyan);

                    Debug.DrawLine(Camera.main.transform.position, Z_prime, Color.yellow);
                    Debug.DrawLine(Camera.main.transform.position, xy_p, Color.blue);
                    */
                    bool _isLeft = isLeft(-transform.forward, transform.forward, xy_p);
                    if (_isLeft)
                    {
                        angle2 = -2 * angle2 / 5;
                    }
                    else
                    {
                        angle2 = 2 * angle2 / 5;
                    }
                    
                    if (_break)
                        Debug.Break();

                    transform.RotateAround(transform.position, transform.up, angle2);

                    float speed = VelocityAcceleration * direction_joystick.magnitude;
                    velocity = new Vector3(0f, 0f, speed);
                    speed = 0f;
                }
            }
            else {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    velocity.z = velocity.z - VelocityAcceleration > -maxSpeed ? velocity.z - VelocityAcceleration : -maxSpeed;
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    velocity.z = velocity.z + VelocityAcceleration < maxSpeed ? velocity.z + VelocityAcceleration : maxSpeed;
                }
                float targetOrientation_Y = transform.localEulerAngles.y;

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    targetOrientation_Y -= rotationAcceleration;
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    targetOrientation_Y += rotationAcceleration;
                }

                jumpPressed = Input.GetKeyDown(KeyCode.Space);
                jumpUnPressed = Input.GetKeyUp(KeyCode.Space);
                targetOrientation = new Vector3(transform.localEulerAngles.x, targetOrientation_Y, transform.localEulerAngles.z);


                transform.localRotation = Quaternion.Euler(targetOrientation.x, targetOrientation.y, targetOrientation.z);
            }

            if (jumpPressed && controller.collisions.below)
            {
                animator.SetTrigger("Jump");
                inAir = true;
                velocity.y = maxJumpVelocity;
            }

            if (jumpUnPressed)
            {
                if (velocity.y > minJumpVelocity)
                {
                    velocity.y = minJumpVelocity;
                }
            }

            if (!controller.collisions.below)
            {
                velocity.y += gravity * Time.deltaTime;
                /*
                if (!inAir && !justLanded)
                {
                    inAir = true;
                    animator.SetTrigger("InAir");
                }
                */
            }
            else if (!jumpPressed)
            {
                if (inAir)
                {
                    animator.SetTrigger("Land");
                    StartCoroutine(RecoverFromLanding());
                    inAir = false;
                }
                velocity.y = 0;
            }

            animator.SetFloat("speed", velocity.z);
            controller.Move(velocity * Time.deltaTime, angle2);

            velocity = velocity / 3;
        }
	}

    public bool isLeft(Vector3 a, Vector3 b, Vector3 c)
    {
        return ((b.x - a.x) * (c.z - a.z) - (b.z - a.z) * (c.x - a.x)) > 0;
    }

    private IEnumerator RecoverFromLanding()
    {
        justLanded = true;
        float timePassed = 0f;
        while (timePassed < timeToRecover)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        justLanded = false;
    }
}
