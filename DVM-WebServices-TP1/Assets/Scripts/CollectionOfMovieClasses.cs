using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionOfMovieClasses : MonoBehaviour {

    [System.Serializable]
    public class Rating {
        public string Source;
        public string Value;
    }

    [System.Serializable]
    public class Movie {
        public string Title;
        public string Year;
        public string Rated;
        public string Released;
        public string Runtime;
        public string Genre;
        public string Director;
        public string Writer ;
        public string Actors;
        public string Plot;
        public string Language;
        public string Country;
        public string Awards ;
        public string Poster;
        public List<Rating> Ratings;
        public string Metascore;
        public string imdbRating;
        public string imdbVotes;
        public string imdbID;
        public string Type;
        public string DVD;
        public string BoxOffice;
        public string Production;
        public string Website;
        public string Response;
    }

    [System.Serializable]
    public class MovieSearchResults {
        public List<Movie> Search;
        public string totalResults;
        public string Response;
    }
}
