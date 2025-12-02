using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    // Rotate left and right with the mouse
    // Look up and down with the camera based on mouse vertical
    // Jump if space bar (single jump)
    // Stretch goals for today: sprint, crouch

    public float moveSpeed = 2.0f;
    public float strafeSpeed = 1.5f;
    public float yawSpeed = 260.0f;
    public float pitchSpeed = 260.0f;
    public float jumpForce = 6.0f;
    public Transform groundRef;
    public float maxPitch = 70.0f;
    public float minPitch = -70.0f;

    private Vector3 jumpDir;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpDir = transform.up;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // grab variables for all the input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        float yaw = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y");

        Vector3 vec = Vector3.zero;
        vec += transform.right * h * strafeSpeed;
        vec += transform.forward * v * moveSpeed;
        vec.y = rb.velocity.y;

        rb.velocity = vec;

        transform.localEulerAngles += new Vector3(
            0,
            yaw * yawSpeed * Time.deltaTime,
            0);
        
        float pitchDelta = -1 * pitch * pitchSpeed * Time.deltaTime;
        float newPitch = Camera.main.transform.localEulerAngles.x + pitchDelta;
        newPitch = AngleWithin180(newPitch);

        newPitch = Mathf.Clamp(newPitch, minPitch, maxPitch);

        Camera.main.transform.localEulerAngles = new Vector3(
            newPitch,
            Camera.main.transform.localEulerAngles.y,
            Camera.main.transform.localEulerAngles.z);

        if (jump && Physics.Raycast(groundRef.position, transform.up * -1, .015f))
        {
            rb.AddForce(jumpForce * jumpDir, ForceMode.Impulse);
        }
    }

//precondition: angle is between 0 and 360
    public static float AngleWithin180(float angle)
    {
        //if (angle <=180)
        //{
        //    return angle;
        //}

        //return angle - 360;

        return angle <= 180 ? angle : angle - 360;
        //ternary operator syntax: condition ? ifTrueValue : ifFalseValue
        // if the condition is true, the whole expression evaluates to ifTrueValue
        // otherwise it equals the ifFalseValue
    }
}
