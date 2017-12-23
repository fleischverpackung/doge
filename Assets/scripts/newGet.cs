using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class newGet : MonoBehaviour
{

    public Object coin;
    private float dropArea = 4f;
    private float dispenserInterval = 0.1f;
    public float coinTarget;

    private Text debug_error;
    
    private TextMesh[] textfields;
    private GameObject[] textObj;
    private Vector3[] textPos;

    private float refreshIntervalHTTP = 8;
    private float refreshIntervalTEXT = .3f;    

    public AudioSource _audioSource;
    private AudioClip[] dogAudio;
    private AudioClip[] coinAudio;

    private MeshRenderer bigTexture;
    private Material[] dogelevelTextures;

    private string url = @"https://api.cryptonator.com/api/ticker/doge-eur";

    GameObject cam;
    GameObject doge;
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
        textObj = GameObject.FindGameObjectsWithTag("text");
        textPos = new Vector3[textObj.Length];
        for (int i=0; i < textObj.Length; i++ )
        {
            textPos[i] = textObj[i].transform.position;
        }
        /*
        text3d[0] = GameObject.Find("txt_wow");
        text3d[1] = GameObject.Find("txt_many");
        text3d[2] = GameObject.Find("txt_so");
        text3d[3] = GameObject.Find("txt_very");
        text3d[4] = GameObject.Find("txt_much");        
        */
        
        debug_error = GameObject.Find("text_error").GetComponent<Text>();
        dogAudio = Resources.LoadAll<AudioClip>("dogeAudio");
        coinAudio = Resources.LoadAll<AudioClip>("coinAudio");
        dogelevelTextures = Resources.LoadAll<Material>("dogeTextureslevel");
        bigTexture = GameObject.Find("LevelTexture").GetComponent<MeshRenderer>();
        particles = GameObject.Find("ParticleSystem").GetComponent<ParticleSystem>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        doge = GameObject.Find("Doge");

        Input.gyro.enabled = true;
        ReadFile();
        StartCoroutine(GetUrl());        
        StartCoroutine(SetTexts());
        StartCoroutine(DispenseCoins());
    }

    private void Update()
    {
        Vector3 deviceRotation = Input.gyro.attitude.eulerAngles;
        coinTarget = Mathf.Round(ExtensionMethods.Remap(coinPercentage, 0, 300, 0, 500));
        doge.transform.Rotate(Vector3.up, 3 * Time.deltaTime);
        MoveCamera();
    }



    void MoveCamera()
    {
        var dogePos = doge.transform.position;
        var newRotation = cam.transform.rotation.eulerAngles;
        cam.transform.eulerAngles = gyro;
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
                    debug_error.text = www.error;

                if (www.text != null)
                {
                    var N = JSON.Parse(www.text);
                    debug_error.text = "";

                    priceOld = price;
                    price = float.Parse(N["ticker"]["price"]);
                    changeOld = change;
                    change = float.Parse(N["ticker"]["change"]);

                    CheckLevel();
                    PlayDogeAudio();
                    StartCoroutine(ShowDogeTexture());
                    coinPercentage = (100 / coinReferenceValue) * price;
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

    IEnumerator DispenseCoins()
    {
        while (true)
        {
            int spawnedCoins = GameObject.FindGameObjectsWithTag("coin").Length;
            if (spawnedCoins < coinTarget)
            {
                Vector3 pos = new Vector3(Random.Range(-dropArea, dropArea), Random.Range(-dropArea, dropArea), Random.Range(3, 4));
                Instantiate(coin, pos, Quaternion.identity);
                _audioSource.PlayOneShot(coinAudio[Random.Range(0, coinAudio.Length)]);
            }
            if (spawnedCoins > coinTarget)
            {
                Destroy(GameObject.FindGameObjectWithTag("coin"));
            }
            yield return new WaitForSecondsRealtime(dispenserInterval);
        }
    }



    void PlayDogeAudio()
    {
        int randomSound = Random.Range(0, 3);
        _audioSource.PlayOneShot(dogAudio[(level * 7) + randomSound]);
        /*
        if (level == 2)
        {
            Handheld.Vibrate();
        }
        */
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
        
            switch (randomTextField)
            {
                case 0:
                    textObj[0].GetComponent<TextMesh>().text = content["wow"][randomTextContent];
                    break;
                case 1:
                    textObj[1].GetComponent<TextMesh>().text = "many " + content["many"][level][randomTextContent];
                    break;
                case 2:
                    textObj[2].GetComponent<TextMesh>().text = "much " + content["much"][randomTextContent]; 
                    break;
                case 3:
                    textObj[3].GetComponent<TextMesh>().text = "very " + content["very"][level][randomTextContent];
                    break;
                case 4:
                    textObj[4].GetComponent<TextMesh>().text = "so " + content["so"][randomTextContent] + " " + coinPercentage.ToString("0,00") + "%";
                    break;
            }

            textObj[randomTextField].transform.position = textPos[randomTextField] + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            yield return new WaitForSeconds(refreshIntervalTEXT);
        }
    }    


    void ReadFile()
    {
        TextAsset t1 = Resources.Load<TextAsset>("txt");
        string text = t1.text;
        Debug.Log("TEXTS JSON FILE " + text);
        var N = JSON.Parse(text);
        content = N;
    }


}


public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}


