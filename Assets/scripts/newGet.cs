using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class newGet : MonoBehaviour
{
    private Text debug_gyro;
    private Text debug_http;
    private Text debug_level;
    private Text debug_error;

    private float refreshIntervalHTTP = 5;
    private float refreshIntervalTEXT = 0.3f;

    public AudioSource _audioSource;
    private AudioClip[] dogAudio;

    private MeshRenderer bigTexture;
    private Material[] dogelevelTextures;

    private string url = @"https://api.cryptonator.com/api/ticker/doge-eur";
    string path = "Assets/Resources/texts.json";


    GameObject cam;
    GameObject shiba;
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
        shiba = GameObject.FindGameObjectWithTag("shiba");
        text3d = GameObject.FindGameObjectsWithTag("text");
        bigTexture = GameObject.Find("LevelTexture").GetComponent<MeshRenderer>();

        debug_gyro = GameObject.Find("text_gyro").GetComponent<Text>();
        debug_http = GameObject.Find("text_http").GetComponent<Text>();
        debug_level = GameObject.Find("text_level").GetComponent<Text>();
        debug_error = GameObject.Find("text_error").GetComponent<Text>();

        dogAudio = Resources.LoadAll<AudioClip>("dogeAudio");
        dogelevelTextures = Resources.LoadAll<Material>("dogeTextureslevel");




        Input.gyro.enabled = true;

        StartCoroutine(GetUrl());

        ReadFile();

        StartCoroutine(SetTexts());
    }

    private void Update()
    {
        Vector3 deviceRotation = Input.gyro.attitude.eulerAngles;

        debug_gyro.text = "Gyro: " + deviceRotation.ToString();

        

        RotateDoge();
        shiba.transform.Rotate(deviceRotation);
    }

  

    IEnumerator GetUrl()
    {
        while(true)
        {
            using (WWW www = new WWW(url))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                    //Debug.Log(www.error);
                    debug_level.text = www.error;

                if (www.text != null)
                {
                    var N = JSON.Parse(www.text);

                    priceOld = price;
                    price = double.Parse(N["ticker"]["price"], System.Globalization.NumberStyles.Any); // prevent error small numbers "Ex007m" etc..
                    changeOld = change;
                    change = double.Parse(N["ticker"]["change"], System.Globalization.NumberStyles.Any);
                    //Debug.Log("price = " + price + "change = " + change);

                    CheckLevel();
                    PlayDogeAudio();
                    StartCoroutine(ShowDogeTexture());

                    //Debug on Canvas
                    debug_error.text = "connection established";
                    debug_http.text = "price = " + price + "| change = " + change;
                    debug_level.text = "LEVEL: " + level;
                }
                yield return new WaitForSeconds(refreshIntervalHTTP);
            }
        }
       
    }

    void CheckLevel()
    {
        /*
        if (change > 0.001f)
            level = 2;
        else if (change > 0.0001f)
            level = 1;
        else if (change > 0.00001f)
            level = 0;
            */
        if (price < priceOld)
            level = 0;
        else if (price == priceOld)
            level = 1;
        else if (price > priceOld)
            level = 2;

        

        
    }

    void PlayDogeAudio()
    {
        int randomSound = Random.Range(0, 3);
        _audioSource.PlayOneShot(dogAudio[(level * 4) + randomSound]);
        Debug.Log("LEVEL = " + level + "RANDOM = " + randomSound);
        Debug.Log("Play Dogsound Nr. " + ((level * 4) + randomSound));
        Debug.Log("AudioArray: " + dogAudio.Length);
    }

    IEnumerator ShowDogeTexture()
    {        
        bigTexture.material = dogelevelTextures[level];
        bigTexture.enabled = true;
        yield return new WaitForSeconds(5);
        bigTexture.enabled = false;
    }

    IEnumerator SetTexts()
    {
        while (true)
        {

        
            int randomTextContent = Random.Range(0, 2);
            int randomTextField = Random.Range(0, 4);
            //Debug.Log("RANDOM: " + randomTextContent);
        
            switch (randomTextField)
            {
                case 0:
                    text3d[0].GetComponent<TextMesh>().text = content["wow"][randomTextContent];
                    break;
                case 1:
                    text3d[1].GetComponent<TextMesh>().text = "many " + content["many"][level][randomTextContent];
                    break;
                case 2:
                    text3d[2].GetComponent<TextMesh>().text = "so " + content["so"][randomTextContent];
                    break;
                case 3:
                    text3d[3].GetComponent<TextMesh>().text = "very " + content["very"][level][randomTextContent];
                    break;
                case 4:
                    text3d[4].GetComponent<TextMesh>().text = "much " + content["much"][randomTextContent];
                    break;
            }
            yield return new WaitForSeconds(refreshIntervalTEXT);
        }
    }    


    void ReadFile()
    {
        string text = System.IO.File.ReadAllText(path);
        Debug.Log("TEXTS JSON FILE " + text);
        var N = JSON.Parse(text);
        content = N;
    }

    void RotateDoge()
    {
        shiba.transform.Rotate(Vector3.up, 3 * Time.deltaTime);
    }

    void ShowLevelTexture()
    {

    }

    /*
    Quaternion RotateText()
    {
        var rotation : Quaternion;
        var radius = Vector3(5, 0, 0);
        var currentRotation = 0.0;

        currentRotation += Time.deltaTime * 100;
        rotation.eulerAngles = Vector3(0, currentRotation, 0);
        transform.position = rotation * radius;
    }
    */
    

}