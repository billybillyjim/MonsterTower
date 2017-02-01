using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    [SerializeField]
    private float movementSpeed = .2f;
    [SerializeField]
    private float minX = 6;
    [SerializeField]
    private float minY = 4;
    [SerializeField]
    private float maxX = 41;
    [SerializeField]
    private float maxY = 70;

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
