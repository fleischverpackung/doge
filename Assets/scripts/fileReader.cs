using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class fileReader : MonoBehaviour {


    string path = "Assets/Resources/texts.json";
    TextMesh[] textfields;
    GameObject[] text3d;
    JSONNode content;

    void Start () {

        text3d = GameObject.FindGameObjectsWithTag("text");
        Debug.Log("Found GameObjects: " + text3d[0]);
        

        ReadFile();

        StartCoroutine(SetTexts());
    }


    IEnumerator SetTexts()
    {
        int rnd = Random.Range(0, 5);
        Debug.Log("RANDOM: " + rnd);

        //central.instance.GetLevel();

        text3d[0].GetComponent<TextMesh>().text = content["wow"][rnd];
        //text3d[1].GetComponent<TextMesh>().text = content["many"][level][rnd];

        yield return new WaitForSeconds(2);

    }




void ReadFile()
    {
        string text = System.IO.File.ReadAllText(path);
        Debug.Log("TEXTS JSON FILE " + text);

        var N = JSON.Parse(text);
        content = N;
    }

    void ReadString()
    {
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}
