using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;


public class SearchButton : MonoBehaviour {

    private CollectionOfMovieClasses.MovieSearchResults resultsOfSearch;
    private CollectionOfMovieClasses.Movie movie;

	void Start () {
	    
	}
	
	void Update () {
		
	}

    public void Search()
    {
        StartCoroutine(GetText());
    }

    public IEnumerator GetText()
    {
        using (UnityWebRequest MovieInfoRequest = UnityWebRequest.Get("http://www.omdbapi.com/?apikey=81876aef&s=Matrix"))
        {
            yield return MovieInfoRequest.SendWebRequest();

            if (MovieInfoRequest.isNetworkError || MovieInfoRequest.isHttpError)
            {
                Debug.Log(MovieInfoRequest.error);
            }
            else
            {
                resultsOfSearch = JsonUtility.FromJson<CollectionOfMovieClasses.MovieSearchResults>(MovieInfoRequest.downloadHandler.text);
                Debug.Log(resultsOfSearch.Search[0].Year);
               // movie = JsonUtility.FromJson<CollectionOfMovieClasses.Movie>(MovieInfoRequest.downloadHandler.text);
               // Debug.Log(movie.Type);
            }
        }
    }
}
