using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float destroyTime = 2.1f;
    public Vector3 offSet = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 randomIntensity = new Vector3(0.5f, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.LookAt(Camera.main.transform);
        transform.localPosition += offSet;
        transform.localPosition += new Vector3(Random.Range(-randomIntensity.x, randomIntensity.x),Random.Range(-randomIntensity.y, randomIntensity.y), Random.Range(-randomIntensity.z, randomIntensity.z));
    }

}
