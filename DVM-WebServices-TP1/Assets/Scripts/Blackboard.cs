using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour {

    public int currentPage = 0;
    public int totalPages = 0;
    public string currentSearch = "";
    public GameObject[] singleResultPanels;
    public CollectionOfMovieClasses.MovieSearchResults resultsOfSearch;
    public CollectionOfMovieClasses.Movie movie;

    void Start () {
		
	}
	
	void Update () {
		
	}
}
