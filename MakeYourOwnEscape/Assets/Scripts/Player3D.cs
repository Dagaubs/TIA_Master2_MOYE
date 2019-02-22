﻿using System.Collections;
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
	private bool useKeyBoard = false;

	private Vector3 targetOrientation;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller3D>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		bool jumpPressed = Input.GetKeyDown(KeyCode.Space), jumpUnPressed = Input.GetKeyDown(KeyCode.Space);
		if(!useKeyBoard){
			Vector3 direction_joystick = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0f);
			//Debug.Log("direction_joystick : " + direction_joystick);
			if(direction_joystick.magnitude > 0.1f){
				Vector3 Z_prime = Camera.main.cameraToWorldMatrix.MultiplyVector(Vector3.forward);
				Vector3 axis = Vector3.Cross(transform.up, Z_prime);
				float angle = Vector3.Angle(transform.up, Z_prime);
                transform.RotateAround(transform.position, axis, angle);
                Debug.Log("direction joystick : " + direction_joystick);
                Debug.DrawRay(transform.position, transform.forward, Color.green);
				Vector3 xy_p = Camera.main.cameraToWorldMatrix.MultiplyVector(direction_joystick);
                float angletest = Vector3.Angle(transform.forward, xy_p - Z_prime);

                Debug.DrawRay(Z_prime, xy_p - Z_prime, Color.magenta);
                Debug.DrawRay(Z_prime, transform.forward, Color.white);
                transform.RotateAround(transform.position, axis, -angle);
                float angle2 = Vector3.Angle(transform.forward, xy_p - Z_prime);
                Debug.Log("angletest : " + angletest + " | angle2 : " + angle2);

                Debug.DrawRay(Z_prime, xy_p - Z_prime, Color.gray);
                Debug.DrawRay(Z_prime, transform.forward, Color.cyan);
                /*
                var line = transform.position + (transform.forward * visionDistance);
                var rotatedLine = Quaternion.AngleAxis(angle2, transform.up) * line;
                Debug.DrawLine(transform.position, rotatedLine, Color.cyan);*/

                //Debug.Break();
                Debug.DrawRay(transform.position, transform.forward, Color.red);
                Debug.DrawLine(Camera.main.transform.position, Z_prime, Color.yellow);
                Debug.DrawLine(Camera.main.transform.position, xy_p, Color.blue);

                Debug.Break();
                /*//Debug.Break();
                if (angle2 > step)
					Debug.Log("Angle above " + step + " : " + angle2);
				else if(angle2 < 0f)
					Debug.Log("NEGATIVE ANGLE : " + angle2);
	
				if(angle2 > step){
					float newAngle = angle2 - removeStep;
					//Debug.Log("Old Angle : " + angle2 + " | new Angle : " + newAngle);
					angle2 = newAngle;
				}
				//angle2 = angle2 > 90f ? angle2 - 180f: angle2;
                */

                float rotatedAngle = angle2 > 180f ? -180f + (angle2 - 180f) : angle2;
                if(angle2 > 0f) 
                    Debug.Log("angle2 : " + angle2 + " | rotated: " + rotatedAngle);
                
				transform.RotateAround(transform.position, transform.up, rotatedAngle);

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
			targetOrientation = new Vector3(transform.localEulerAngles.x,targetOrientation_Y, transform.localEulerAngles.z);
			
			if(jumpPressed && controller.collisions.below){
				animator.SetTrigger("Jump");
				velocity.y = maxJumpVelocity;
			}

			if(jumpUnPressed){
				if (velocity.y > minJumpVelocity)
                {
                    velocity.y = minJumpVelocity;
                }
			}

			transform.localRotation = Quaternion.Euler(targetOrientation.x, targetOrientation.y, targetOrientation.z);

			//Debug.Log("Actual TargetOrientation : " + targetOrientation);
			//Quaternion RotationTo = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(targetOrientation.x, targetOrientation.y, targetOrientation.z), Time.deltaTime * rotateSpeed);
			//Debug.Log("Actual rotation : " + transform.localEulerAngles + " | to : " + RotationTo * Vector3.forward);
			//targetOrientation = transform.localEulerAngles;
		}

		if(!controller.collisions.below) velocity.y += gravity * Time.deltaTime;
		else if(!jumpPressed) velocity.y = 0;
		
		animator.SetFloat("speed", velocity.z);
		controller.Move(velocity * Time.deltaTime);
 
		velocity = velocity / 3;
	}
}
