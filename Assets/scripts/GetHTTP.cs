using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
[System.Serializable]
public class PlayerInfo
{
    public static string target;

    public static PlayerInfo CreateFromJSON(string jsonString)
    {
        
        JsonUtility.FromJson<PlayerInfo>(jsonString);
        Debug.Log(target);
        //return


    }
    

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}
*/

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
        //PlayerInfo.CreateFromJSON(text);
        //Debug.Log("READ: " + PlayerInfo.target);
    }
    

    static void ReadStream()
    {
        string path = "Assets/Resources/data.json";
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }


}

