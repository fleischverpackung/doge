using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinDispenser : MonoBehaviour {

    public Object[] pickupPrefabs;
    public AudioSource _audioSource;
    public AudioClip[] _audioClip;
    private int dropArea = 5;
    private float dispenseInterval = 0.3f;
    private int maxCoins = 200;
    private int timer = 0;
    private bool doDispense = true;
    private Vector3 posShiba;

    // Use this for initialization
    void Start () {
        StartCoroutine(DispenserOn());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DispenserOn()
    {
        while (doDispense)
        {
            
            if (doDispense)
            {
                //int drugCase = (Random.Range(0, 4));
                Vector3 pos = new Vector3(Random.Range(-dropArea, dropArea), Random.Range(-dropArea, dropArea), Random.Range(-dropArea, dropArea));
               
                Instantiate(pickupPrefabs[0], pos, Quaternion.identity);
                //clone.rigidbody.AddForce(transform.forward * 8000);
                
                    //int rand = (Random.Range(0, 3));
                    _audioSource.PlayOneShot(_audioClip[0]);

                    Debug.Log("Dropped Coin @ " + pos);
                


            }
            yield return new WaitForSecondsRealtime(dispenseInterval);
        }
    }
}
