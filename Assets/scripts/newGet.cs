using UnityEngine;
using System.Collections;
using SimpleJSON;

public class newGet : MonoBehaviour
{
    public string url = @"https://api.cryptonator.com/api/ticker/doge-eur";
    IEnumerator Start()
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
                Debug.Log(www.error);

            if (www.text != null)
            {
                Debug.Log("JSONCONTENT: " + www.text);
                var N = JSON.Parse(www.text);
                var name = N["ticker"]["price"];
                Debug.Log("JSON PRICE: " + name);

            }
        }
    }
}