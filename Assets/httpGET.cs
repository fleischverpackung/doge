using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class httpGET : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(Check());
    }

    private IEnumerator Check()
    {
        WWW w = new WWW("http%3A%2F%2Fwww.brainjar.com%2Fjava%2Fhost%2Ftest.html"); //https%3A%2F%2Fapi.cryptonator.com%2Fapi%2Fticker%2Fdoge-eur
        yield return w;
        if (w.error != null)
        {
            Debug.Log("Error .. " + w.error);
            // for example, often 'Error .. 404 Not Found'
        }
        else
        {
            Debug.Log("Found ... ==>" + w.text + "<==");
            // don't forget to look in the 'bottom section'
            // of Unity console to see the full text of
            // multiline console messages.
        }

        /* example code to separate all that text in to lines:
        longStringFromFile = w.text
        List<string> lines = new List<string>(
            longStringFromFile
            .Split(new string[] { "\r","\n" },
            StringSplitOptions.RemoveEmptyEntries) );
        // remove comment lines...
        lines = lines
            .Where(line => !(line.StartsWith("//")
                            || line.StartsWith("#")))
            .ToList();
        */

    }
}