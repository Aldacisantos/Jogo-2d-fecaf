using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    public enum FollowMode
    {
        StrictFollow,       // Camera exactly follows player
        SmoothFollow,       // Camera smoothly follows player
        LerpFollow,         // Camera lerps toward player
        BoundedFollow       // Camera follows but stays within bounds
    }

    [Header("Target Settings")]
    [Tooltip("The target transform to follow")]
    public Transform target;

    [Header("Follow Settings")]
    [Tooltip("The follow mode to use")]
    public FollowMode followMode = FollowMode.SmoothFollow;

    [Tooltip("Offset from the target position")]
    public Vector2 offset = new Vector2(0f, 0f);

    [Tooltip("How quickly the camera catches up to the target (for SmoothFollow and LerpFollow)")]
    [Range(0.1f, 10f)] public float followSpeed = 5f;

    [Header("Boundary Settings")]
    [Tooltip("Should the camera be bounded?")]
    public bool useBounds = false;

    [Tooltip("Minimum camera position (world coordinates)")]
    public Vector2 minBounds = new Vector2(-10f, -10f);

    [Tooltip("Maximum camera position (world coordinates)")]
    public Vector2 maxBounds = new Vector2(10f, 10f);

    [Header("Advanced Settings")]
    [Tooltip("Should the camera maintain its Z position?")]
    public bool maintainZPosition = true;

    [Tooltip("The initial Z position of the camera")]
    public float initialZPosition = -10f;

    private Camera _camera;
    private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        
        if (maintainZPosition)
        {
            initialZPosition = transform.position.z;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow2D: No target assigned!");
            return;
        }

        // Calculate target position with offset
        Vector3 targetPosition = target.position + (Vector3)offset;

        // Maintain Z position if configured
        if (maintainZPosition)
        {
            targetPosition.z = initialZPosition;
        }

        // Apply the selected follow mode
        switch (followMode)
        {
            case FollowMode.StrictFollow:
                transform.position = targetPosition;
                break;

            case FollowMode.SmoothFollow:
                // SmoothDamp gives a nice easing effect
                transform.position = Vector3.SmoothDamp(
                    transform.position, 
                    targetPosition, 
                    ref _velocity, 
                    1f / followSpeed
                );
                break;

            case FollowMode.LerpFollow:
                // Linear interpolation
                transform.position = Vector3.Lerp(
                    transform.position, 
                    targetPosition, 
                    followSpeed * Time.deltaTime
                );
                break;

            case FollowMode.BoundedFollow:
                // Smooth follow with bounds checking
                Vector3 boundedPosition = Vector3.SmoothDamp(
                    transform.position, 
                    targetPosition, 
                    ref _velocity, 
                    1f / followSpeed
                );
                transform.position = ApplyBounds(boundedPosition);
                break;
        }

        // If using bounds but not in BoundedFollow mode, apply bounds anyway
        if (useBounds && followMode != FollowMode.BoundedFollow)
        {
            transform.position = ApplyBounds(transform.position);
        }
    }

    private Vector3 ApplyBounds(Vector3 position)
    {
        if (!useBounds) return position;

        // Calculate camera's viewport size in world units
        float cameraHeight = _camera.orthographicSize;
        float cameraWidth = cameraHeight * _camera.aspect;

        // Clamp position to stay within bounds while keeping target in view
        position.x = Mathf.Clamp(
            position.x,
            minBounds.x + cameraWidth,
            maxBounds.x - cameraWidth
        );

        position.y = Mathf.Clamp(
            position.y,
            minBounds.y + cameraHeight,
            maxBounds.y - cameraHeight
        );

        return position;
    }

    // Draw bounds in editor for visualization
    private void OnDrawGizmosSelected()
    {
        if (!useBounds) return;

        Gizmos.color = Color.green;
        Vector3 center = (minBounds + maxBounds) / 2f;
        Vector3 size = maxBounds - minBounds;
        Gizmos.DrawWireCube(center, size);
    }
}