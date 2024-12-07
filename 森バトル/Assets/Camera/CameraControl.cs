using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 170f;    // Horizontal Moving Speed of Camera
    public float rotationSpeed = 100f;  // Vertical Moving Speed of Camera
    public float zoomSpeed = 200f; // Mouse Wheel Speed
    public float maxY = 708f;        // Camera Coordinate  Ymax
    public float minY = 200f;         // Camera Coordinate Ymin
    public float minX = -77f;        // Xmin
    public float maxX = 87f;         // Xmax
    public float maxZ = 40f;         // Zmax
    public float minZ = -89f;        // Zmin
    public float minZoomDistance = 20f;
    public float maxZoomDistance = 500f;

    private Camera cam;
    private GameObject target; // Empty Object
    private Vector3 targetOffset;

    void Start()
    {
        cam = GetComponent<Camera>();

        //　Making target Object
        target = new GameObject("CameraTarget");
        target.transform.position = Vector3.zero; // starting position
        
        targetOffset = cam.transform.position - target.transform.position;

    }

    void Update()
    { 
        TargetMovement();
        CameraRotation();
        CameraZoom();
        if (target.transform.position.y != 0f)
        {
            target.transform.position = new Vector3(target.transform.position.x, 0f, target.transform.position.z);
        }
        ApplyPositionLimits(target.transform);
        ApplyPositionLimits(this.transform);// Limiting of Camera
        FollowTarget();
    }

    // WASD key
    bool TargetMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        // W/S key
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += this.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= this.transform.forward;
        }

        // A/D key
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= this.transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += this.transform.right;
        }
        target.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        if (moveDirection == Vector3.zero)
        {
            return false;
        }
        else { return true; }
    }

    // Camera will look around the empty target
    void CameraRotation()
    {
        if (Input.GetMouseButton(1) && !TargetMovement()) // While putting right bottom
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            
            cam.transform.RotateAround(target.transform.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
            
            cam.transform.RotateAround(target.transform.position, cam.transform.right, -mouseY * rotationSpeed * Time.deltaTime);
            
            targetOffset = cam.transform.position - target.transform.position;
        }
    }
    void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f) // While moving mouse wheel
        {
            targetOffset += targetOffset.normalized * scroll * zoomSpeed * Time.deltaTime;

            targetOffset = Vector3.ClampMagnitude(targetOffset, maxZoomDistance);
            if (targetOffset.magnitude < minZoomDistance)
            {
                targetOffset = targetOffset.normalized * minZoomDistance;
            }
        }
    }

    // Target Empty Object
    void FollowTarget()
    {
        cam.transform.position = target.transform.position + targetOffset;
        cam.transform.LookAt(target.transform);
    }

    // target position
    void ApplyPositionLimits(Transform targetTransform)
    {
        Vector3 position = targetTransform.position;

        position.y = Mathf.Clamp(position.y, minY, maxY);
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);

        targetTransform.position = position;
    }
}
