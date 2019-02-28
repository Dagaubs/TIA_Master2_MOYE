using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Controller3D : MonoBehaviour {


	[SerializeField]
    private LayerMask _collisionMask;

    [SerializeField]
    private LayerMask _collisionMaskOrigin;

    private BoxCollider _collider;
    private RaycastOrigins raycastOrigins;

    [SerializeField]
	private float skinWidth = .015f;
    //public float skinHeight;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public CollisionInfo collisions;

    private Rigidbody _rigidbody;

    public CollisionHitInfo hitInfo;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;

	void Start(){
		_collider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        CalculateRaySpacing();
	}

	public void Move(Vector3 velocity, float angle, float scale)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        hitInfo.Reset();

        //Debug.Log("velocity after Move = " + velocity);
        if (angle != 0f)
        {
            HorizontalCollisions(ref angle);
            //transform.RotateAround(transform.position, transform.up, angle);
        }/*
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }*/
        if(velocity.z != 0){
            BackAndForwardCollisions(ref velocity);
        }

        //Debug.Log("velocity before Translate = " + velocity);
        /*if(velocity.x != 0){
			transform.rotation = velocity.x > 0 ? Quaternion.Euler(Vector3.right) : Quaternion.Euler(Vector3.left);
		}
		if(velocity.z != 0){
			transform.rotation = velocity.z > 0 ? Quaternion.Euler(Vector3.up) : Quaternion.Euler(Vector3.down);
        }*/

        /*if(velocity != Vector3.zero)
            Debug.Break();
        */
        if(_rigidbody != null)
        {
            collisions.below = Mathf.Abs(_rigidbody.velocity.y) < 0.01f;
        }
        transform.Translate(velocity * scale);
    }

    private void BackAndForwardCollisions(ref Vector3 velocity){
        float direction = Mathf.Sign(velocity.z);
        float rayLength = Mathf.Abs(velocity.z) + skinWidth;

        BackAndForwardLineCheck(ref velocity, raycastOrigins.feet, direction, rayLength, Color.red);
        BackAndForwardLineCheck(ref velocity, raycastOrigins.knees, direction, rayLength, Color.yellow);
        BackAndForwardLineCheck(ref velocity, raycastOrigins.pelvis, direction, rayLength, Color.green);
        BackAndForwardLineCheck(ref velocity, raycastOrigins.shoulders, direction, rayLength, Color.blue);
        BackAndForwardLineCheck(ref velocity, raycastOrigins.head, direction, rayLength, Color.black); 
    }

    private void HorizontalCollisions(ref float angle)
    {
        float directionX = Mathf.Sign(angle);
        Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.pelvis.topLeft : raycastOrigins.pelvis.topRight;
        Vector3 transpose = Quaternion.AngleAxis(angle, transform.up) * rayOrigin;
        float rayLength = (transpose - rayOrigin).magnitude;

        HorizontalLineCheck(ref angle, raycastOrigins.feet, directionX, rayLength, Color.red);
        HorizontalLineCheck(ref angle, raycastOrigins.knees, directionX, rayLength, Color.yellow);
        HorizontalLineCheck(ref angle, raycastOrigins.pelvis, directionX, rayLength, Color.green);
        HorizontalLineCheck(ref angle, raycastOrigins.shoulders, directionX, rayLength, Color.blue);
        HorizontalLineCheck(ref angle, raycastOrigins.head, directionX, rayLength, Color.black);      
    }

    private void HorizontalLineCheck(ref float angle, QuadVector line, float directionX, float rayLength, Color rayColor){
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (directionX == -1) ? line.topLeft : line.topRight;
            rayOrigin += -transform.forward * (horizontalRaySpacing * i);
            RaycastHit hit;
            Debug.DrawRay(rayOrigin, directionX == -1 ? -transform.right * rayLength : transform.right * rayLength, rayColor);
            
            if (Physics.Raycast(rayOrigin, transform.right * directionX, out hit, rayLength, _collisionMask))
            {
                //Debug.Log("you hit something : " + hit.collider.gameObject.name);
                angle = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                hitInfo.gameObject = hit.collider.gameObject;
                //Debug.Log("touched " + (directionX == -1 ? "left " : "right ") + ": " + hitInfo.gameObject.name);
                collisions.behind = directionX == -1;
                collisions.forward = directionX == 1;
            }
        }
    }

    private void BackAndForwardLineCheck(ref Vector3 velocity, QuadVector line, float directionZ, float rayLength, Color rayColor){
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (directionZ == -1) ? line.bottomLeft : line.topLeft;
            Debug.DrawLine(rayOrigin, rayOrigin + Vector3.up * 0.05f, Color.gray);
            rayOrigin += transform.right * (horizontalRaySpacing * i);
            RaycastHit hit;
            Debug.DrawRay(rayOrigin, transform.forward * directionZ * rayLength, rayColor);
            
            if (Physics.Raycast(rayOrigin, transform.forward * directionZ, out hit, rayLength, _collisionMask))
            {
                //Debug.Log("you hit something : " + hit.collider.gameObject.name);
                velocity.z = (hit.distance - skinWidth) * directionZ;
                rayLength = hit.distance;

                hitInfo.gameObject = hit.collider.gameObject;
                //Debug.Log("touched " + (directionZ == -1 ? "behind " : "forward ") + ": " + hitInfo.gameObject.name);
                collisions.behind = directionZ == -1;
                collisions.forward = directionZ == 1;
            }
        }
        //Debug.Break();
    }

    private void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        if(directionY == -1 ) VerticalLineCheck(ref velocity, raycastOrigins.feet, directionY, rayLength, Color.red);
        else
            VerticalLineCheck(ref velocity, raycastOrigins.head, directionY, rayLength, Color.black);
        /*VerticalLineCheck(ref velocity, raycastOrigins.knees, directionZ, rayLength, Color.yellow);
        VerticalLineCheck(ref velocity, raycastOrigins.pelvis, directionZ, rayLength, Color.green);
        VerticalLineCheck(ref velocity, raycastOrigins.shoulders, directionZ, rayLength, Color.blue);
        VerticalLineCheck(ref velocity, raycastOrigins.head, directionZ, rayLength, Color.black);*/
    }

    private void VerticalLineCheck(ref Vector3 velocity, QuadVector line, float directionY, float rayLength, Color rayColor){
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector3 rayOrigin = (directionY == -1) ? line.bottomLeft : line.topLeft;
            rayOrigin += Vector3.right * (verticalRaySpacing * i/* + velocity.z*/);

            Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, rayColor);
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, _collisionMask))
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                hitInfo.gameObject = hit.collider.gameObject;
                //Debug.Log("touched " + (directionY == -1 ? "below " : "above ") + ": " + hitInfo.gameObject.name);
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
                //Debug.Break();
            }
        }
    }
    

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = _collider.bounds;
        Vector3 center = _collider.center + transform.position;
        bounds.Expand(skinWidth * -2);
        float midDistance = (bounds.center.y-bounds.min.y)/2;
        float knees_height = bounds.min.y + midDistance;
        float shoulders_height = bounds.center.y+midDistance;
        float size = bounds.max.x - bounds.center.x;

        Vector3 feetCenter = center + (-transform.up * midDistance*2);
        Vector3 kneesCenter = feetCenter + transform.up * midDistance;
        Vector3 pelvisCenter = center;
        Vector3 shouldersCenter = center + transform.up * midDistance;
        Vector3 headCenter = center + (transform.up * midDistance*2);
        Vector3 axis = headCenter - feetCenter;


        raycastOrigins.feet.topLeft = feetCenter + (-transform.right + transform.forward) * size;
        raycastOrigins.feet.bottomLeft = feetCenter + (-transform.right + -transform.forward) * size;
        raycastOrigins.feet.topRight = feetCenter + (transform.right + transform.forward) * size;
        raycastOrigins.feet.bottomRight = feetCenter + (transform.right + -transform.forward) * size;
        
        raycastOrigins.knees.topLeft = kneesCenter + (-transform.right + transform.forward) * size;
        raycastOrigins.knees.bottomLeft = kneesCenter + (-transform.right + -transform.forward) * size;
        raycastOrigins.knees.topRight = kneesCenter + (transform.right + transform.forward) * size;
        raycastOrigins.knees.bottomRight = kneesCenter + (transform.right + -transform.forward) * size;
        
        raycastOrigins.pelvis.topLeft = pelvisCenter + (-transform.right + transform.forward) * size;
        raycastOrigins.pelvis.bottomLeft = pelvisCenter + (-transform.right + -transform.forward) * size;
        raycastOrigins.pelvis.topRight = pelvisCenter + (transform.right + transform.forward) * size;
        raycastOrigins.pelvis.bottomRight = pelvisCenter + (transform.right + -transform.forward) * size;

        raycastOrigins.shoulders.topLeft = shouldersCenter + (-transform.right + transform.forward) * size;
        raycastOrigins.shoulders.bottomLeft = shouldersCenter + (-transform.right + -transform.forward) * size;
        raycastOrigins.shoulders.topRight = shouldersCenter + (transform.right + transform.forward) * size;
        raycastOrigins.shoulders.bottomRight = shouldersCenter + (transform.right + -transform.forward) * size;

        raycastOrigins.head.topLeft = headCenter + (-transform.right + transform.forward) * size;
        raycastOrigins.head.bottomLeft = headCenter + (-transform.right + -transform.forward) * size;
        raycastOrigins.head.topRight = headCenter + (transform.right + transform.forward) * size;
        raycastOrigins.head.bottomRight = headCenter + (transform.right + -transform.forward) * size;
        
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.z / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    private struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;

        public QuadVector feet, knees, pelvis, shoulders, head;
    }

    private struct QuadVector
    {
        public Vector3 topLeft, topRight, bottomLeft, bottomRight;
    }
    
    public struct CollisionInfo
    {
        public bool 
        above, below,
        forward, behind;


        public void Reset()
        {
            above = below = false;
            forward = behind = false;
        }
    }

    public struct CollisionHitInfo
    {
        public GameObject gameObject;

        public void Reset()
        {
            gameObject = null;
        }
    }
}
