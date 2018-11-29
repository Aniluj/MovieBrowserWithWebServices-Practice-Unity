using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;


public class SearchButton : MonoBehaviour {

    private IndividualSearch individualSearch;
    public string actualTextOfMovieToSearch;
    public string actualTextOfMovieYearToSearch;
    public string actualTextOfMovieTypeToSearch;
    public Blackboard blackboard;
    public Dropdown movieType;
    public InputField movieSearchBox;
    public InputField movieYearBox;

    public GameObject[] panelsToDeactivate;
    public GameObject[] panelsToActivate;

    void Start () {
	    
	}
	
	void Update () {
		
	}

    public void Search()
    {
        actualTextOfMovieToSearch = movieSearchBox.text;
        actualTextOfMovieYearToSearch = movieYearBox.text;
        actualTextOfMovieTypeToSearch = movieType.captionText.text;

        blackboard.currentSearch = "http://www.omdbapi.com/?apikey=81876aef&s="+actualTextOfMovieToSearch+"&y="+actualTextOfMovieYearToSearch+"&type="+actualTextOfMovieTypeToSearch;
        StartCoroutine(GetMovies());
    }

    public IEnumerator GetMovies()
    {
        using (UnityWebRequest MovieInfoRequest = UnityWebRequest.Get(blackboard.currentSearch))
        {
            yield return MovieInfoRequest.SendWebRequest();
            
            if (MovieInfoRequest.isNetworkError || MovieInfoRequest.isHttpError)
            {
                Debug.Log(MovieInfoRequest.error);
            }
            else
            {
                blackboard.resultsOfSearch = JsonUtility.FromJson<CollectionOfMovieClasses.MovieSearchResults>(MovieInfoRequest.downloadHandler.text);
                SetTextsOfMovieBoxes(true);
                if (blackboard.resultsOfSearch.Search.Count != 0)
                {
                    blackboard.totalPages = int.Parse(blackboard.resultsOfSearch.totalResults) / 10;
                    if(int.Parse(blackboard.resultsOfSearch.totalResults) % 10 > 0)
                    {
                        blackboard.totalPages += 1;
                    }

                    blackboard.currentPage = 1;

                    Debug.Log(blackboard.totalPages);

                    DeactivateAndActivatePanels();

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
            for(int i=0; i< blackboard.singleResultPanels.Length;i++)
            {
                individualSearch = blackboard.singleResultPanels[i].GetComponent<IndividualSearch>();
                individualSearch.id = "";

                Text[] textsOfSingleResultPanel = blackboard.singleResultPanels[i].GetComponentsInChildren<Text>();

                blackboard.totalPages = 0;
                blackboard.currentPage = 0;

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
            for(int i=0;i< blackboard.resultsOfSearch.Search.Count;i++)
            {
                individualSearch = blackboard.singleResultPanels[i].GetComponent<IndividualSearch>();
                individualSearch.id = blackboard.resultsOfSearch.Search[i].imdbID;

                Text[] textsOfSingleResultPanel = blackboard.singleResultPanels[i].GetComponentsInChildren<Text>();
                
                for (int j = 0; j < textsOfSingleResultPanel.Length; j++)
                {
                    if (textsOfSingleResultPanel[j].tag == "MovieTitleText")
                    {
                        textsOfSingleResultPanel[j].text = blackboard.resultsOfSearch.Search[i].Title;
                    }
                    else if (textsOfSingleResultPanel[j].tag == "MovieYearText")
                    {
                        textsOfSingleResultPanel[j].text = blackboard.resultsOfSearch.Search[i].Year;
                    }
                    else if (textsOfSingleResultPanel[j].tag == "MovieTypeText")
                    {
                        textsOfSingleResultPanel[j].text = blackboard.resultsOfSearch.Search[i].Type;
                    }
                }
            }
        }
    }

    public void DeactivateAndActivatePanels()
    {
        if (panelsToDeactivate != null)
        {
            for (int i = 0; i < panelsToDeactivate.Length; i++)
            {
                panelsToDeactivate[i].SetActive(false);
            }
        }
        if (panelsToActivate != null)
        {
            for (int i = 0; i < panelsToActivate.Length; i++)
            {
                panelsToActivate[i].SetActive(true);
            }
        }
    }
}
