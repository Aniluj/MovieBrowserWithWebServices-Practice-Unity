using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;

public class SpecificDataController : MonoBehaviour {

    public CollectionOfMovieClasses.Movie movie;
    public Text specificDataText;
    public GameObject plotPanel;
    public GameObject ratingPanel;
    public GameObject posterPanel;
    private CollectionOfMovieClasses.Movie movieAux;
    private bool expand;
    private bool shrink;
    private string searchForFullOrShortPlot;

    void Start () {
		
	}

	void Update () {

	}

    public void SetSpecificData()
    {
        specificDataText.text = "Title: " + movie.Title;
        specificDataText.text += "\nYear: " + movie.Year;
        specificDataText.text += "\nType: " + movie.Type;
        specificDataText.text += "\nRated: " + movie.Rated;
        specificDataText.text += "\nRelease Date: " + movie.Released;
        specificDataText.text += "\nRuntime: " + movie.Runtime;
        specificDataText.text += "\nGenre: " + movie.Genre;
        specificDataText.text += "\nDirector: " + movie.Director;
        if(movie.Writer != null)
        {
            specificDataText.text += "\nWriter: " + movie.Writer;
        }
        if(movie.Actors != null)
        {
            specificDataText.text += "\nActors: " + movie.Actors;
        }
        if(movie.Language != null)
        {
            specificDataText.text += "\nLanguage: " + movie.Language;
        }
        if(movie.Country != null)
        {
            specificDataText.text += "\nCountry: " + movie.Country;
        }
        if(movie.Awards != null)
        {
            specificDataText.text += "\nAwards: " + movie.Awards;
        }
        if(movie.Production != null)
        {
            specificDataText.text += "\nProduction" + movie.Production;
        }
        if(movie.totalSeasons != null)
        {
            specificDataText.text += "\nTotal Seasons: " + movie.totalSeasons;
        }
        if(movie.Plot != null)
        {
            expand = true;
            plotPanel.SetActive(true);
            Text[] plotText = plotPanel.GetComponentsInChildren<Text>();

            for(int i = 0; i < plotText.Length; i++)
            {
                if(plotText[i].tag == "PlotText")
                {
                    plotText[i].text = movie.Plot;
                }
            }
        }
        if(movie.Ratings.Count != 0)
        {
            ratingPanel.SetActive(true);
            Text[] ratingText = ratingPanel.GetComponentsInChildren<Text>();

            for(int i=0; i<ratingText.Length;i++)
            {
                if(ratingText[i].tag == "RatingText")
                {
                    for(int j=0; j<movie.Ratings.Count; j++)
                    {
                        ratingText[i].text += "Value: " + movie.Ratings[j].Value;
                        ratingText[i].text += "\nSource: " + movie.Ratings[j].Source + "\n\n";
                    }
                }
            }
        }
    }

    public void ResetData()
    {
        Text[] ratingText = ratingPanel.GetComponentsInChildren<Text>();

        for(int i = 0; i < ratingText.Length; i++)
        {
            if(ratingText[i].tag == "RatingText")
            {
                ratingText[i].text = "";
            }
        }

        ratingPanel.SetActive(false);

        Text[] plotText = plotPanel.GetComponentsInChildren<Text>();

        for(int i = 0; i < plotText.Length; i++)
        {
            if(plotText[i].tag == "PlotText")
            {
                plotText[i].text = "";
            }
        }

        plotPanel.SetActive(false);
        expand = false;
        shrink = true;
    }

    public void ExpandOrShrink()
    {
        if(expand)
        {
            expand = false;
            shrink = true;
            searchForFullOrShortPlot = "http://www.omdbapi.com/?apikey=81876aef&i=" + movie.imdbID + "&plot=full";
        }
        else if (shrink)
        {
            expand = false;
            shrink = true;
            searchForFullOrShortPlot = "http://www.omdbapi.com/?apikey=81876aef&i=" + movie.imdbID + "&plot=short";
        }
        StartCoroutine(GetFullOrShortPlot());
    }

    public IEnumerator GetFullOrShortPlot()
    {
        using(UnityWebRequest MovieInfoRequest = UnityWebRequest.Get(searchForFullOrShortPlot))
        {
            yield return MovieInfoRequest.SendWebRequest();

            if(MovieInfoRequest.isNetworkError || MovieInfoRequest.isHttpError)
            {
                Debug.Log(MovieInfoRequest.error);
            }
            else
            {
                movieAux = JsonUtility.FromJson<CollectionOfMovieClasses.Movie>(MovieInfoRequest.downloadHandler.text);

                Text[] plotText = plotPanel.GetComponentsInChildren<Text>();

                for(int i = 0; i < plotText.Length; i++)
                {
                    if(plotText[i].tag == "PlotText")
                    {
                        plotText[i].text = movieAux.Plot;
                    }
                    else if(plotText[i].tag == "ExpandOrShrinkText")
                    {
                        if(expand)
                        {
                            plotText[i].text = "Shrink";
                        }
                        else if (shrink)
                        {
                            plotText[i].text = "Expand";
                        }
                    }
                }
            }
        }
    }
}
