using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to camera to have it orbit around target
public class Turntable : MonoBehaviour
{

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime);
        transform.LookAt(target);
    }
}
