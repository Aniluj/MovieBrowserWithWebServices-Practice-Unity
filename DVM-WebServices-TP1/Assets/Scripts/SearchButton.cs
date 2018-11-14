using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;


public class SearchButton : MonoBehaviour {

    private CollectionOfMovieClasses.MovieSearchResults resultsOfSearch;
    private CollectionOfMovieClasses.Movie movie;
    private string actualTextOfMovieToSearch;
    public Dropdown movieType;
    public InputField movieSearchBox;
    public InputField movieYearBox;
    public GameObject[] singleResultPanels;

	void Start () {
	    
	}
	
	void Update () {
		
	}

    public void Search()
    {
        actualTextOfMovieToSearch = "http://www.omdbapi.com/?apikey=81876aef&s="+movieSearchBox.text+"&y="+movieYearBox.text+"&type="+movieType.captionText.text;
        StartCoroutine(GetMovies());
    }

    public IEnumerator GetMovies()
    {
        using (UnityWebRequest MovieInfoRequest = UnityWebRequest.Get(actualTextOfMovieToSearch))
        {
            yield return MovieInfoRequest.SendWebRequest();

            if (MovieInfoRequest.isNetworkError || MovieInfoRequest.isHttpError)
            {
                Debug.Log(MovieInfoRequest.error);
            }
            else
            {
                resultsOfSearch = JsonUtility.FromJson<CollectionOfMovieClasses.MovieSearchResults>(MovieInfoRequest.downloadHandler.text);
                if(resultsOfSearch.Search[0] != null)
                {
                    Debug.Log(resultsOfSearch.Search[0].imdbID);
                    for(int i = 0; i < singleResultPanels.Length; i++)
                    {
                        Debug.Log("pelaca");
                        Button titleButton = singleResultPanels[i].transform.Find("Title Button").GetComponent<Button>();
                        Debug.Log(titleButton.name);
                        Text[] yearAndType = singleResultPanels[i].GetComponents<Text>();
                    }
                }
                else
                {
                    Debug.Log("No se han encontrado resultados para la busqueda solicitada");
                }
                //movie = JsonUtility.FromJson<CollectionOfMovieClasses.Movie>(MovieInfoRequest.downloadHandler.text);
                //Debug.Log(movie.Type);
            }
        }
    }
}
