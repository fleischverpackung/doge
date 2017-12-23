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

    private float refreshIntervalHTTP = 7;
    private float refreshIntervalTEXT = 0.3f;

    public AudioSource _audioSource;
    private AudioClip[] dogAudio;

    private MeshRenderer bigTexture;
    private Material[] dogelevelTextures;

    private string url = @"https://api.cryptonator.com/api/ticker/doge-eur";
    //string path = "Assets/Resources/texts.json";


    GameObject cam;
    GameObject doge;
    TextMesh[] textfields;
    GameObject[] text3d;
    JSONNode content;
    ParticleSystem particles;
    private float price;
    private float priceOld;
    private float change;
    private float changeOld;

    private Vector3 gyro;
    private int level;
    private float coinReferenceValue = 0.00613704f;
    private float coinPercentage;


    void Start()
    {
        text3d = GameObject.FindGameObjectsWithTag("text");
        /*
        text3d[0] = GameObject.Find("txt_wow");
        text3d[1] = GameObject.Find("txt_many");
        text3d[2] = GameObject.Find("txt_so");
        text3d[3] = GameObject.Find("txt_very");
        text3d[4] = GameObject.Find("txt_much");        
        */

        debug_gyro = GameObject.Find("text_gyro").GetComponent<Text>();
        debug_http = GameObject.Find("text_http").GetComponent<Text>();
        debug_level = GameObject.Find("text_level").GetComponent<Text>();
        debug_error = GameObject.Find("text_error").GetComponent<Text>();

        dogAudio = Resources.LoadAll<AudioClip>("dogeAudio");
        dogelevelTextures = Resources.LoadAll<Material>("dogeTextureslevel");
        bigTexture = GameObject.Find("LevelTexture").GetComponent<MeshRenderer>();
        particles = GameObject.Find("ParticleSystem").GetComponent<ParticleSystem>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        doge = GameObject.Find("Doge");


        

        Input.gyro.enabled = true;

        StartCoroutine(GetUrl());

        moveCamera();

        ReadFile();

        StartCoroutine(SetTexts());
    }

    private void Update()
    {
        Vector3 deviceRotation = Input.gyro.attitude.eulerAngles;
        debug_gyro.text = "Gyro: " + deviceRotation.ToString();

        RotateDoge();
    }



    void moveCamera()
    {
        var dogePos = doge.transform.position;
        //cam.transform.LookAt(dogePos);
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
                    price = float.Parse(N["ticker"]["price"]);
                    changeOld = change;
                    change = float.Parse(N["ticker"]["change"]);

                    CheckLevel();
                    PlayDogeAudio();
                    StartCoroutine(ShowDogeTexture());

                   //particles.emission.rateOverTime = 100f;

                    coinPercentage = (100 / coinReferenceValue) * price;

                    //Debug on Canvas
                    debug_error.text = "connection established";
                    debug_http.text = coinPercentage.ToString("0.000") + "% " +  "price = " + price.ToString("0.000000") + "| change = " + change.ToString("0.000000");
                    debug_level.text = "LEVEL: " + level;
                }
                yield return new WaitForSeconds(refreshIntervalHTTP);
            }
        }
       
    }

    void CheckLevel()
    {        
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
        _audioSource.PlayOneShot(dogAudio[(level * 7) + randomSound]);
        Debug.Log("AudioArrayLength: " + dogAudio.Length + "LEVEL = " + level + "RANDOM = " + randomSound + "Play Dogsound Nr. " + ((level * 4) + randomSound));
    }

    IEnumerator ShowDogeTexture()
    {        
        bigTexture.material = dogelevelTextures[level];
        bigTexture.enabled = true;
       //bigTexture.transform.position + new Vector3 (0, 0,  += 0.5f * Time.deltaTime;)
        yield return new WaitForSeconds(4);
        bigTexture.enabled = false;
    }

    IEnumerator SetTexts()
    {
        while (true)
        {
                   
            int randomTextContent = Random.Range(0, 3);
            int randomTextField = Random.Range(0, 5);
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
                    text3d[2].GetComponent<TextMesh>().text = "much " + content["much"][randomTextContent]; 
                    break;
                case 3:
                    text3d[3].GetComponent<TextMesh>().text = "very " + content["very"][level][randomTextContent];
                    break;
                case 4:
                    text3d[4].GetComponent<TextMesh>().text = "so " + content["so"][randomTextContent] + " " + coinPercentage.ToString("0,00") + "%";
                    break;
            }
            yield return new WaitForSeconds(refreshIntervalTEXT);
        }
    }    


    void ReadFile()
    {
        TextAsset t1 = Resources.Load<TextAsset>("txt");
        string text = t1.text;
        //string text = System.IO.File.ReadAllText(path);
        Debug.Log("TEXTS JSON FILE " + text);
        var N = JSON.Parse(text);
        content = N;
    }

    void RotateDoge()
    {
        doge.transform.Rotate(Vector3.up, 3 * Time.deltaTime);
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

        /*
    float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }*/

}

/*
public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
*/

