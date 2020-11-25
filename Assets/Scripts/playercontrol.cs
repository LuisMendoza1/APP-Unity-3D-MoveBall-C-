using UnityEngine;
using System.Collections;

public class playercontrol : MonoBehaviour {
    float speed = (float)10.0;
       // velocidad = (float)0.08;
    bool pause = false;
    public float velocidad;

    void Update()
    {
        Vector3 dir = Vector3.zero;
        dir.z = -Input.acceleration.x;

        // clamp acceleration vector to the unit sphere
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        if (!pause)
        {
            //transform.Translate(dir * speed);
            transform.Translate(new Vector3(dir.x, 0, 0) * speed);
            if (dir.z * 10 > 3.5)
                transform.position = new Vector3(transform.position.x, transform.position.y, (float)1.2);
            else if (dir.z * 10 < -3.5)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, (float)-1.7);
            }
            else if ((dir.z * 10 < 1) && (dir.z * 10 > -1))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, (float)-0.2);
            }
            transform.position = new Vector3(transform.position.x + velocidad, transform.position.y, transform.position.z);
        }
    }
    float accelerometerUpdateInterval = (float) 1.0 / (float) 60.0;
    float LowPassKernelWidthInSeconds = (float) 1.0;
    private float LowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
    Vector3 iniPos;
    void Start()
    {
        LowPassFilterFactor = accelerometerUpdateInterval / LowPassKernelWidthInSeconds; // tweakable
        lowPassValue = Input.acceleration;
        iniPos = transform.position;

    }

    Vector3 LowPassFilterAccelerometer() {
        lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
        return lowPassValue;
    }

    void OnTriggerEnter(Collider obj)
    {
        reset();
        Handheld.Vibrate();
    }

    void OnColissionEnter(Collision obj)
    {
        reset();
    }

    void reset()
    {
        transform.position = iniPos;
    }

    public void makePause()
    {
        pause = !pause;
    }
}
