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
                SetTextsOfMovieBoxes(true);
                if (resultsOfSearch.Search.Count != 0)
                {
                    SetTextsOfMovieBoxes(false);
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

    private void SetTextsOfMovieBoxes(bool reset)
    {
        if(reset)
        {
            for(int i=0; i<singleResultPanels.Length;i++)
            {
                Text[] textsOfSingleResultPanel = singleResultPanels[i].GetComponentsInChildren<Text>();

                for(int j=0;j<textsOfSingleResultPanel.Length;j++)
                {
                    if (textsOfSingleResultPanel[j].tag == "MovieTitleText")
                    {
                        textsOfSingleResultPanel[j].text = "";
                    }
                    else if (textsOfSingleResultPanel[j].tag == "MovieYearText")
                    {
                        textsOfSingleResultPanel[j].text = "";
                    }
                    else if (textsOfSingleResultPanel[j].tag == "MovieTypeText")
                    {
                        textsOfSingleResultPanel[j].text = "";
                    }
                }
            }
        }
        else
        {
            for(int i=0;i< resultsOfSearch.Search.Count;i++)
            {
                Text[] textsOfSingleResultPanel = singleResultPanels[i].GetComponentsInChildren<Text>();

                for (int j = 0; j < textsOfSingleResultPanel.Length; j++)
                {
                    if (textsOfSingleResultPanel[j].tag == "MovieTitleText")
                    {
                        textsOfSingleResultPanel[j].text = resultsOfSearch.Search[i].Title;
                    }
                    else if (textsOfSingleResultPanel[j].tag == "MovieYearText")
                    {
                        textsOfSingleResultPanel[j].text = resultsOfSearch.Search[i].Year;
                    }
                    else if (textsOfSingleResultPanel[j].tag == "MovieTypeText")
                    {
                        textsOfSingleResultPanel[j].text = resultsOfSearch.Search[i].Type;
                    }
                }
            }
        }
    }
}
