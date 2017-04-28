using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    public float timer = .5f;
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update ()
    {
        if(timer <= 0)
            Destroy(gameObject);
        else
        {
            timer -= Time.deltaTime;
        }
                   	
	}
}
