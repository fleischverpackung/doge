using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {


    private int x;



	void Start () {

        x = Random.Range(80, 150);
		
	}


	void Update () {

        this.transform.Rotate(Vector3.up, x * Time.deltaTime);

    }
}
