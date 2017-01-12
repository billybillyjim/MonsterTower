using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {

    public float rotateSpeed;

    void Start()
    {

    }
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0, 0, -GameRun.hour * 15 * rotateSpeed);
	}
}
