using UnityEngine;

public class HelicopterBlades : MonoBehaviour
{
    [Header("Hélices")]
    public Transform mainRotor;     
    public Transform tailRotor;    
    public float maxRotorSpeed = 1000f;

    [Header("Motor")]
    public bool engineOn = false;
    public float currentRotorSpeed = 0f;
    public float accelerationSpeed = 50f;
    public float decelerationSpeed = 100f;

    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.E))
        {
            engineOn = !engineOn;
        }

    
        if (engineOn)
        {
            currentRotorSpeed = Mathf.MoveTowards(
                currentRotorSpeed,
                maxRotorSpeed,
                accelerationSpeed * Time.deltaTime
            );
        }
        else
        {
            currentRotorSpeed = Mathf.MoveTowards(
                currentRotorSpeed,
                0f,
                decelerationSpeed * Time.deltaTime
            );
        }

   
        if (mainRotor != null)
        {
            mainRotor.Rotate(Vector3.up, currentRotorSpeed * Time.deltaTime);
        }

        if (tailRotor != null)
        {
            tailRotor.Rotate(Vector3.forward, currentRotorSpeed * Time.deltaTime);
        }
    }
}
