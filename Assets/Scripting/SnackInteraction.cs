using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class SnackInteraction : MonoBehaviour
{
    private Rigidbody rb;
    private Camera cam;

    private bool isHeld = false;
    private float holdDistance = 2f;
    private float inactivityTimer = 0f;
    private float destroyAfter = 10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        // Make snack float in air with no gravity or bounce
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.linearDamping = 5f;
        rb.angularDamping = 5f;
    }

    void Update()
    {
        // ?? Track time when not held
        if (!isHeld)
        {
            inactivityTimer += Time.deltaTime;

            if (inactivityTimer >= destroyAfter)
            {
                Destroy(gameObject); // ?? Gone after 10 sec
            }
        }
        else
        {
            inactivityTimer = 0f; // ? Reset timer if holding

            // Scroll to move forward/backward
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
                holdDistance = Mathf.Clamp(holdDistance + scroll, 0.5f, 3f);

            // Move object in front of camera
            Vector3 targetPos = cam.transform.position + cam.transform.forward * holdDistance;
            rb.MovePosition(Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 20f));

            // Rotate object if middle mouse held
            if (Input.GetMouseButton(2))
            {
                float dx = Input.GetAxis("Mouse X");
                float dy = Input.GetAxis("Mouse Y");
                transform.Rotate(cam.transform.up, dx * 100f * Time.deltaTime, Space.World);
                transform.Rotate(cam.transform.right, -dy * 100f * Time.deltaTime, Space.World);
            }
        }

        // ?? Left click to hold
        if (Input.GetMouseButtonDown(0) && !isHeld)
        {
            TryPickup();
        }

        // ?? Right click to drop
        if (Input.GetMouseButtonDown(1) && isHeld)
        {
            Drop();
        }
    }

    void TryPickup()
    {
        Ray ray = cam.ViewportPointToRay(Vector3.one * 0.5f);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f) && hit.collider.gameObject == gameObject)
        {
            isHeld = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void Drop()
    {
        isHeld = false;
        rb.linearVelocity = cam.transform.forward * 0.5f; // soft push
    }
}
