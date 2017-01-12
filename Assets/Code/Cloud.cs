using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

    private float speed;

	// Use this for initialization
	void Start () {
        speed = Random.Range(0, 3f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(-speed * .1f * GameRun.gameSpeed, 0));
	    if(transform.position.x < 0)
        {
            Destroy(gameObject);
        }
	}
}
