using UnityEngine;
using System.Collections;
using SimpleJSON;

public class newGet : MonoBehaviour
{
    private string url = @"https://api.cryptonator.com/api/ticker/doge-eur";
    

    


    IEnumerator Start()
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
                string price = N["ticker"]["price"];
                string change = N["ticker"]["change"];

                Debug.Log("price = " + price + "change = " + change);


                central.Instance.SetPrice(float.Parse(price));
                //central.Instance.GetLevel
                //obj.SetChange(float.Parse(change));
                

            }
        }
    }
    
}