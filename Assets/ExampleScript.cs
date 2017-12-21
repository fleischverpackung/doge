using UnityEngine;
using System.Collections;

public class ExampleScript : MonoBehaviour
{
    public string url = "http%3A%2F%2Fwww.brainjar.com%2Fjava%2Fhost%2Ftest.html";
    IEnumerator Start()
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
                Debug.Log(www.error);
        }
    }
}
