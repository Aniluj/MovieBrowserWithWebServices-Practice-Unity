using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOfPageController : MonoBehaviour {

    public Text pageIndicator;
    public Blackboard blackboard;
    public SearchButton searchController;
    //private CollectionOfMovieClasses.MovieSearchResults resultsOfSearch;
    //private CollectionOfMovieClasses.Movie movie;

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
        if(nextPage && blackboard.currentPage < blackboard.totalPages)
        {
            blackboard.currentPage += 1;

            blackboard.currentSearch = "http://www.omdbapi.com/?apikey=81876aef&s=" + searchController.actualTextOfMovieToSearch + "&y=" + searchController.movieYearBox.text + "&type=" + searchController.movieType.captionText.text + "&page=" + blackboard.currentPage;
            StartCoroutine(GetPageOfMovie());
        }
        else if (!nextPage && blackboard.currentPage > 1)
        {
            blackboard.currentPage = blackboard.currentPage - 1;

            blackboard.currentSearch = "http://www.omdbapi.com/?apikey=81876aef&s=" + searchController.actualTextOfMovieToSearch + "&y=" + searchController.actualTextOfMovieYearToSearch + "&type=" + searchController.actualTextOfMovieTypeToSearch + "&page=" + blackboard.currentPage;
            StartCoroutine(GetPageOfMovie());
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
                blackboard.resultsOfSearch = JsonUtility.FromJson<CollectionOfMovieClasses.MovieSearchResults>(MovieInfoRequest.downloadHandler.text);
                SetTextsOfMovieBoxes(true);
                if(blackboard.resultsOfSearch.Search.Count != 0)
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
            for(int i = 0; i < blackboard.singleResultPanels.Length; i++)
            {
                Text[] textsOfSingleResultPanel = blackboard.singleResultPanels[i].GetComponentsInChildren<Text>();

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
            for(int i = 0; i < blackboard.resultsOfSearch.Search.Count; i++)
            {
                Text[] textsOfSingleResultPanel = blackboard.singleResultPanels[i].GetComponentsInChildren<Text>();

                for(int j = 0; j < textsOfSingleResultPanel.Length; j++)
                {
                    if(textsOfSingleResultPanel[j].tag == "MovieTitleText")
                    {
                        textsOfSingleResultPanel[j].text = blackboard.resultsOfSearch.Search[i].Title;
                    }
                    else if(textsOfSingleResultPanel[j].tag == "MovieYearText")
                    {
                        textsOfSingleResultPanel[j].text = blackboard.resultsOfSearch.Search[i].Year;
                    }
                    else if(textsOfSingleResultPanel[j].tag == "MovieTypeText")
                    {
                        textsOfSingleResultPanel[j].text = blackboard.resultsOfSearch.Search[i].Type;
                    }
                }
            }
        }
    }
}
