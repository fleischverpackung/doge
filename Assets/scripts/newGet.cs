using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class newGet : MonoBehaviour
{
    public Text hud;

    private string url = @"https://api.cryptonator.com/api/ticker/doge-eur";
    string path = "Assets/Resources/texts.json";


    GameObject cam;
    TextMesh[] textfields;
    GameObject[] text3d;
    JSONNode content;
    private double price;
    private double priceOld;
    private double change;
    private double changeOld;

    private Vector3 gyro;

    private int level;


    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        text3d = GameObject.FindGameObjectsWithTag("text");
        //Debug.Log("Found GameObjects: " + text3d[0]);

        Input.gyro.enabled = true;

        StartCoroutine(GetUrl());

        ReadFile();

        StartCoroutine(SetTexts());
    }

    private void Update()
    {
       hud.text = Input.gyro.attitude.ToString();
    }

  

    IEnumerator GetUrl()
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
                Debug.Log(www.error);

            if (www.text != null)
            {

                var N = JSON.Parse(www.text);
                Debug.Log("JSONCONTENT: " + www.text);

                priceOld = price;
                price = double.Parse(N["ticker"]["price"], System.Globalization.NumberStyles.Any);
                changeOld = change;
                change = double.Parse(N["ticker"]["change"], System.Globalization.NumberStyles.Any);
                Debug.Log("price = " + price + "change = " + change);

                GetLevel();
            }  
        }
    }

    void GetLevel()
    {
        if (change > 0.001f)
            level = 2;
        else if (change > 0.0001f)
            level = 1;
        else if (change > 0.00001f)
            level = 0;
    }

    IEnumerator SetTexts()
    {
        while (true)
        {

        
            int rnd = Random.Range(0, 2);
            Debug.Log("RANDOM: " + rnd);
        

            text3d[0].GetComponent<TextMesh>().text = content["wow"][rnd];
            text3d[1].GetComponent<TextMesh>().text = "many " + content["many"][level][rnd];
            text3d[2].GetComponent<TextMesh>().text = "so " + content["so"][rnd];
            text3d[3].GetComponent<TextMesh>().text = "very " + content["very"][level][rnd];
            text3d[4].GetComponent<TextMesh>().text = "much " + content["much"][rnd];

            yield return new WaitForSeconds(.5f);
        }
    }    


    void ReadFile()
    {
        string text = System.IO.File.ReadAllText(path);
        Debug.Log("TEXTS JSON FILE " + text);

        var N = JSON.Parse(text);
        content = N;
    }
    

}