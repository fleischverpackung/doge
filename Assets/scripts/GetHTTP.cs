using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
class ItemArray
{
   // public Item[] items;
}


public class GetHTTP : MonoBehaviour {


    //private string str = @"{"ticker":{"base":"DOGE","target":"EUR","price":"0.00706002","volume":"","change":"0.00000009"},"timestamp":1513860662,"success":true,"error":""}";
	


	void Start () {

        ReadFile();

    }
	

	void Update () {
		
	}

    void ReadFile()
    {
        string text = File.ReadAllText(@"Assets/Resources/data.json");
        Debug.Log(text);
    }
    

    static void ReadStream()
    {
        string path = "Assets/Resources/data.json";
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}
