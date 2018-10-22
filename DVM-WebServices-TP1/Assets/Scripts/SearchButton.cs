using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class SearchButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Search()
    {
        StartCoroutine(GetText());
    }

    public IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://www.omdbapi.com/?apikey=81876aef&s=Matrix"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
