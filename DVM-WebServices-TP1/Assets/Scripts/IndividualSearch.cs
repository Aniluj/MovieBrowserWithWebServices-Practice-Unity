using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class IndividualSearch : MonoBehaviour {

    public string id;
    public CollectionOfMovieClasses.Movie movie;
    public GameObject[] panelsToDeactivate;
    public GameObject[] panelsToActivate;
    private string currentIndividualSearch;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void MakeIndividualSearch()
    {
        currentIndividualSearch = "http://www.omdbapi.com/?apikey=81876aef&i=" + id;
        StartCoroutine(GetIndividualMovie());
    }

    public IEnumerator GetIndividualMovie()
    {
        using (UnityWebRequest MovieInfoRequest = UnityWebRequest.Get(currentIndividualSearch))
        {
            yield return MovieInfoRequest.SendWebRequest();

            if (MovieInfoRequest.isNetworkError || MovieInfoRequest.isHttpError)
            {
                Debug.Log(MovieInfoRequest.error);
            }
            else
            {
                movie = JsonUtility.FromJson<CollectionOfMovieClasses.Movie>(MovieInfoRequest.downloadHandler.text);

                if(movie.imdbID != null)
                {
                    if (panelsToActivate != null)
                    {
                        for (int i = 0; i < panelsToActivate.Length; i++)
                        {
                            panelsToActivate[i].SetActive(true);
                        }
                    }
                    if (panelsToDeactivate != null)
                    {
                        for (int i = 0; i < panelsToDeactivate.Length; i++)
                        {
                            panelsToDeactivate[i].SetActive(false);
                        }
                    }
                }
            }
        }
    }

    private void ActivateAndDeactivatePanels()
    {
        if (panelsToActivate != null)
        {
            for (int i = 0; i < panelsToActivate.Length; i++)
            {
                panelsToActivate[i].SetActive(true);
            }
        }
        if (panelsToDeactivate != null)
        {
            for (int i = 0; i < panelsToDeactivate.Length; i++)
            {
                panelsToDeactivate[i].SetActive(false);
            }
        }
    }
}
