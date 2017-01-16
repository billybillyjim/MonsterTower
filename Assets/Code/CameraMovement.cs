using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    [SerializeField]
    private float movementSpeed = .3f;
    private float minX = 6;
    private float minY = 4;
    private float maxX = 41;
    private float maxY = 50;

	// Update is called once per frame
	void Update () {

        if(transform.position.x < minX)
        {
            transform.position = new Vector3(minX + .01f, transform.position.y, transform.position.z);
            //transform.Translate(new Vector3((Input.GetAxis("Horizontal") * movementSpeed) + .5f, Input.GetAxis("Vertical") * movementSpeed));
        }
        if(transform.position.x > maxX)
        {
           transform.position = new Vector3(maxX - .01f, transform.position.y, transform.position.z);
           // transform.Translate(new Vector3((Input.GetAxis("Horizontal") * movementSpeed) - .5f, Input.GetAxis("Vertical") * movementSpeed));
        }
        if(transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY + .01f, transform.position.z);
        }
        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY - .01f, transform.position.z);
        }
        if(transform.position.x > minX &&
            transform.position.x < maxX &&
            transform.position.y > minY &&
            transform.position.y < maxY)
        {
            transform.Translate(new Vector3(Input.GetAxis("Horizontal") * movementSpeed, Input.GetAxis("Vertical") * movementSpeed));
        }
        
	}
}
