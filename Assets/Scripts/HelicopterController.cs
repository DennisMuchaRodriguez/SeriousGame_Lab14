using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class HelicopterController : MonoBehaviour
{
    [Header("Fuerzas de Vuelo")]
    public float liftForce = 12f;         
    public float forwardForce = 8f;       
    public float turnForce = 5f;          
    public float yawForce = 3f;           

    [Header("Hélices")]
    public Transform mainRotor;           
    public Transform tailRotor;           
    public float maxRotorSpeed = 1500f;   
    private float currentRotorSpeed = 0f;
    public float rotorAcceleration = 200f;

    [Header("Controles")]
    public KeyCode engineKey = KeyCode.E; 
    public KeyCode ascendKey = KeyCode.Space;
    public KeyCode descendKey = KeyCode.LeftControl;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode turnLeftKey = KeyCode.A;
    public KeyCode turnRightKey = KeyCode.D;

    private Rigidbody rb;
    private bool engineOn = false;
    private float currentHoverHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHoverHeight = transform.position.y;
    }

    void Update()
    {
        HandleEngine();
        RotateBlades();
    }

    void FixedUpdate()
    {
        if (!engineOn) return;

        HandleMovement();
        ApplyHover();
        Stabilize();
    }

    private void HandleEngine()
    {
        if (Input.GetKeyDown(engineKey))
        {
            engineOn = !engineOn;
        }
    }

    private void RotateBlades()
    {
  
        float targetSpeed = engineOn ? maxRotorSpeed : 0f;
        currentRotorSpeed = Mathf.MoveTowards(
            currentRotorSpeed,
            targetSpeed,
            rotorAcceleration * Time.deltaTime
        );

        
        if (mainRotor != null)
            mainRotor.Rotate(Vector3.up, currentRotorSpeed * Time.deltaTime);

        if (tailRotor != null)
            tailRotor.Rotate(Vector3.forward, currentRotorSpeed * Time.deltaTime);
    }

    private void HandleMovement()
    {
        float powerFactor = currentRotorSpeed / maxRotorSpeed;


        float lift = (Input.GetKey(ascendKey) ? 1f : (Input.GetKey(descendKey) ? -1f : 0f));
        rb.AddForce(Vector3.up * lift * liftForce * powerFactor, ForceMode.Force);

        
        float forward = (Input.GetKey(forwardKey) ? 1f : (Input.GetKey(backwardKey) ? -1f : 0f));
        rb.AddForce(transform.forward * forward * forwardForce * powerFactor, ForceMode.Force);

    
        float turn = (Input.GetKey(turnRightKey) ? 1f : (Input.GetKey(turnLeftKey) ? -1f : 0f));
        rb.AddTorque(Vector3.up * turn * yawForce * powerFactor, ForceMode.Force);
    }

    private void ApplyHover()
    {
        
        if (!Input.GetKey(ascendKey) && !Input.GetKey(descendKey))
        {
            float heightDifference = currentHoverHeight - transform.position.y;
            rb.AddForce(Vector3.up * heightDifference * 2f, ForceMode.Force);
        }
        else
        {
            currentHoverHeight = transform.position.y;
        }
    }

    private void Stabilize()
    {
     
        rb.angularVelocity *= 0.95f;


        if (Mathf.Abs(transform.rotation.eulerAngles.z) > 5f ||
            Mathf.Abs(transform.rotation.eulerAngles.x) > 5f)
        {
            Quaternion targetRotation = Quaternion.Euler(
                0f,
                transform.rotation.eulerAngles.y,
                0f
            );
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                2f * Time.deltaTime
            );
        }
    }
}