using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOfPageController : MonoBehaviour {

    public Text pageIndicator;
    public Blackboard blackboard;
    public SearchButton searchController;
    private CollectionOfMovieClasses.MovieSearchResults resultsOfSearch;
    private CollectionOfMovieClasses.Movie movie;

    void Start ()
    {
        pageIndicator.text = blackboard.currentPage + "/" + blackboard.totalPages;
    }
	
	void Update () 
    {
        pageIndicator.text = blackboard.currentPage + "/" + blackboard.totalPages;
    }

    public void ChangePage(bool nextPage)
    {
        if(nextPage)
        {
            Debug.Log(blackboard.currentPage);
            blackboard.currentPage += 1;
            blackboard.currentSearch = "http://www.omdbapi.com/?apikey=81876aef&s=" + searchController.movieSearchBox.text + "&y=" + searchController.movieYearBox.text + "&type=" + searchController.movieType.captionText.text + "&page=" + blackboard.currentPage;
            StartCoroutine(GetPageOfMovie());
        }
        else if (!nextPage)
        {
            if(blackboard.currentPage != 1)
            {
                Debug.Log(blackboard.currentPage);
                blackboard.currentPage = blackboard.currentPage - 1;
                Debug.Log(blackboard.currentPage);
                blackboard.currentSearch += "http://www.omdbapi.com/?apikey=81876aef&s=" + searchController.movieSearchBox.text + "&y=" + searchController.movieYearBox.text + "&type=" + searchController.movieType.captionText.text + "&page=" + blackboard.currentPage;
                StartCoroutine(GetPageOfMovie());
            }
        }
    }

    public IEnumerator GetPageOfMovie()
    {
        using(UnityWebRequest MovieInfoRequest = UnityWebRequest.Get(blackboard.currentSearch))
        {
            yield return MovieInfoRequest.SendWebRequest();

            if(MovieInfoRequest.isNetworkError || MovieInfoRequest.isHttpError)
            {
                Debug.Log(MovieInfoRequest.error);
            }
            else
            {
                resultsOfSearch = JsonUtility.FromJson<CollectionOfMovieClasses.MovieSearchResults>(MovieInfoRequest.downloadHandler.text);
                SetTextsOfMovieBoxes(true);
                if(resultsOfSearch.Search.Count != 0)
                {
                    SetTextsOfMovieBoxes(false);
                }
                else
                {
                    Debug.Log("No se han encontrado resultados para la busqueda solicitada");
                }
            }
        }
    }

    private void SetTextsOfMovieBoxes(bool reset) 
    {
        if(reset)
        {
            for(int i = 0; i < searchController.singleResultPanels.Length; i++)
            {
                Text[] textsOfSingleResultPanel = searchController.singleResultPanels[i].GetComponentsInChildren<Text>();

                for(int j = 0; j < textsOfSingleResultPanel.Length; j++)
                {
                    if(textsOfSingleResultPanel[j].tag == "MovieTitleText")
                    {
                        textsOfSingleResultPanel[j].text = "";
                    }
                    else if(textsOfSingleResultPanel[j].tag == "MovieYearText")
                    {
                        textsOfSingleResultPanel[j].text = "";
                    }
                    else if(textsOfSingleResultPanel[j].tag == "MovieTypeText")
                    {
                        textsOfSingleResultPanel[j].text = "";
                    }
                }
            }
        }
        else
        {
            for(int i = 0; i < resultsOfSearch.Search.Count; i++)
            {
                Text[] textsOfSingleResultPanel = searchController.singleResultPanels[i].GetComponentsInChildren<Text>();

                for(int j = 0; j < textsOfSingleResultPanel.Length; j++)
                {
                    if(textsOfSingleResultPanel[j].tag == "MovieTitleText")
                    {
                        textsOfSingleResultPanel[j].text = resultsOfSearch.Search[i].Title;
                    }
                    else if(textsOfSingleResultPanel[j].tag == "MovieYearText")
                    {
                        textsOfSingleResultPanel[j].text = resultsOfSearch.Search[i].Year;
                    }
                    else if(textsOfSingleResultPanel[j].tag == "MovieTypeText")
                    {
                        textsOfSingleResultPanel[j].text = resultsOfSearch.Search[i].Type;
                    }
                }
            }
        }
    }
}
