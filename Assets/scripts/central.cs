using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class central : MonoBehaviour
{

    public static central Instance { get; private set; }

    private float price = 0;
    private float priceOld = 0;
    private float change = 0;
    private float changeOld = 0;

    void Awake()
    {

        //Check if instance already exists
        if (Instance == null)
            //if not, set instance to this
            Instance = this;
      
        //If instance already exists and it's not this:
        else if (Instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        
        
    }

    private void Update()
    {
        
    }

    public void GetLevel()
    {
        float diff = Mathf.Abs(priceOld - price);
        Debug.Log("DIFFERENCE: " + diff);

        /*
        if (diff > 0.0001)
            return 2;
        else if (diff > 0.001)
            return 1;
        else if (diff > 0.01)
            return 0;
            */
    }


    public void SetPrice(float x)
    {
        Debug.Log("cebtrál price sed:" + x);
        priceOld = price;
        price = x;
    }
    public void SetChange(float x)
    {
        changeOld = change;
        change = x;
    }

    public float GetPrice()
    {
        return price;
    }
    public float GetChange()
    {
        return change;
    }
}
