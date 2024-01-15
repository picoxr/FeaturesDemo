using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesRotateAround : MonoBehaviour
{
    public float rotationSpeed = 1000;
    // Start is called before the first frame update
    private void Update()
    {
        transform.RotateAround(transform.parent.transform.position, new Vector3(0,1,0), rotationSpeed * Time.deltaTime);
    }
}
