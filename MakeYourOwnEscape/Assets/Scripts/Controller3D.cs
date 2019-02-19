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

    public CollisionHitInfo hitInfo;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;


	void Start(){
		_collider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
	}

	public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        hitInfo.Reset();

        //Debug.Log("velocity after Move = " + velocity);
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
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
        
        transform.Translate(velocity);
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

    private void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        HorizontalLineCheck(ref velocity, raycastOrigins.feet, directionX, rayLength, Color.red);
        HorizontalLineCheck(ref velocity, raycastOrigins.knees, directionX, rayLength, Color.yellow);
        HorizontalLineCheck(ref velocity, raycastOrigins.pelvis, directionX, rayLength, Color.green);
        HorizontalLineCheck(ref velocity, raycastOrigins.shoulders, directionX, rayLength, Color.blue);
        HorizontalLineCheck(ref velocity, raycastOrigins.head, directionX, rayLength, Color.black);      
    }

    private void HorizontalLineCheck(ref Vector3 velocity, QuadVector line, float directionX, float rayLength, Color rayColor){
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (directionX == -1) ? line.bottomLeft : line.bottomRight;
            rayOrigin += Vector3.forward * (horizontalRaySpacing * i);
            RaycastHit hit;
            Debug.DrawRay(rayOrigin, directionX == -1 ? Vector3.left * rayLength : Vector3.right * rayLength, rayColor);
            
            if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, _collisionMask))
            {
                //Debug.Log("you hit something : " + hit.collider.gameObject.name);
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                hitInfo.gameObject = hit.collider.gameObject;
                Debug.Log("touched " + (directionX == -1 ? "left " : "right ") + ": " + hitInfo.gameObject.name);
                collisions.behind = directionX == -1;
                collisions.forward = directionX == 1;
            }
        }
    }

    private void BackAndForwardLineCheck(ref Vector3 velocity, QuadVector line, float directionZ, float rayLength, Color rayColor){
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (directionZ == -1) ? line.bottomLeft : line.topLeft;
            rayOrigin += Vector3.right * (horizontalRaySpacing * i);
            RaycastHit hit;
            Debug.DrawRay(rayOrigin, Vector3.forward * directionZ * rayLength, rayColor);
            
            if (Physics.Raycast(rayOrigin, Vector3.forward * directionZ, out hit, rayLength, _collisionMask))
            {
                //Debug.Log("you hit something : " + hit.collider.gameObject.name);
                velocity.x = (hit.distance - skinWidth) * directionZ;
                rayLength = hit.distance;

                hitInfo.gameObject = hit.collider.gameObject;
                Debug.Log("touched " + (directionZ == -1 ? "behind " : "forward ") + ": " + hitInfo.gameObject.name);
                collisions.behind = directionZ == -1;
                collisions.forward = directionZ == 1;
            }
        }
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
        bounds.Expand(skinWidth * -2);
        float midDistance = (bounds.center.y-bounds.min.y)/2;
        float knees_height = bounds.min.y + midDistance;
        float shoulders_height = bounds.center.y+midDistance;

        raycastOrigins.feet.topLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        raycastOrigins.feet.bottomLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        raycastOrigins.feet.topRight = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        raycastOrigins.feet.bottomRight = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        
        raycastOrigins.knees.topLeft = new Vector3(bounds.min.x, knees_height, bounds.max.z);
        raycastOrigins.knees.bottomLeft = new Vector3(bounds.min.x, knees_height, bounds.min.z);
        raycastOrigins.knees.topRight = new Vector3(bounds.max.x, knees_height, bounds.max.z);
        raycastOrigins.knees.bottomRight = new Vector3(bounds.max.x, knees_height, bounds.min.z);
        
        raycastOrigins.pelvis.topLeft = new Vector3(bounds.min.x, bounds.center.y, bounds.max.z);
        raycastOrigins.pelvis.bottomLeft = new Vector3(bounds.min.x, bounds.center.y, bounds.min.z);
        raycastOrigins.pelvis.topRight = new Vector3(bounds.max.x, bounds.center.y, bounds.max.z);
        raycastOrigins.pelvis.bottomRight = new Vector3(bounds.max.x, bounds.center.y, bounds.min.z);
        
        raycastOrigins.shoulders.topLeft = new Vector3(bounds.min.x, shoulders_height, bounds.max.z);
        raycastOrigins.shoulders.bottomLeft = new Vector3(bounds.min.x, shoulders_height, bounds.min.z);
        raycastOrigins.shoulders.topRight = new Vector3(bounds.max.x, shoulders_height, bounds.max.z);
        raycastOrigins.shoulders.bottomRight = new Vector3(bounds.max.x, shoulders_height, bounds.min.z);

        raycastOrigins.head.topLeft = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        raycastOrigins.head.bottomLeft = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        raycastOrigins.head.topRight = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
        raycastOrigins.head.bottomRight = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
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
