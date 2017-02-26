using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    public Transform cam;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Transform>();
    }
    void Update()
    {
        this.transform.position = cam.transform.position + new Vector3(0, 0, 5);
    }
}
