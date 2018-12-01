using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;

public class SpecificDataController : MonoBehaviour {

    public CollectionOfMovieClasses.Movie movie;
    public ImageData.Data imageData;
    public Text specificDataText;
    public GameObject plotPanel;
    public GameObject ratingPanel;
    public GameObject posterPanel;
    public GameObject goToMovieWebsiteButton;
    public Image poster;
    private Texture2D imageTexture;
    private Sprite imageSprite;
    private CollectionOfMovieClasses.Movie movieAux;
    private bool expand;
    private bool shrink;
    private string searchForFullOrShortPlot;
    private string searchForImageInfo;

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
        if(movie.Poster != null && movie.Poster !="N/A")
        {
            posterPanel.SetActive(true);
            poster.gameObject.SetActive(true);
            searchForImageInfo = "https://www.pida.io/data/" + UnityWebRequest.EscapeURL(movie.Poster)+"?format=json";
            Debug.Log(UnityWebRequest.EscapeURL(movie.Poster));
            StartCoroutine(GetPosterImage());
            StartCoroutine(GetPosterInfo());
        }
        if(movie.Director != null && movie.Director != "N/A")
        {
            specificDataText.text += "\nDirector: " + movie.Director;
        }
        if(movie.Writer != null && movie.Writer != "N/A")
        {
            specificDataText.text += "\nWriter: " + movie.Writer;
        }
        if(movie.Actors != null && movie.Actors != "N/A")
        {
            specificDataText.text += "\nActors: " + movie.Actors;
        }
        if(movie.Language != null && movie.Language != "N/A")
        {
            specificDataText.text += "\nLanguage: " + movie.Language;
        }
        if(movie.Country != null && movie.Country != "N/A")
        {
            specificDataText.text += "\nCountry: " + movie.Country;
        }
        if(movie.Awards != null && movie.Awards != "N/A")
        {
            specificDataText.text += "\nAwards: " + movie.Awards;
        }
        if(movie.Production != null && movie.Production != "N/A")
        {
            specificDataText.text += "\nProduction: " + movie.Production;
        }
        if(movie.totalSeasons != null && movie.totalSeasons != "N/A")
        {
            specificDataText.text += "\nTotal Seasons: " + movie.totalSeasons;
        }
        if(movie.Website != null && movie.Website != "N/A")
        {
            goToMovieWebsiteButton.SetActive(true);
        }
        if(movie.Plot != null && movie.Plot != "N/A")
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
        if(movie.Ratings.Count != 0 && movie.Ratings[0].Value != "N/A")
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
        goToMovieWebsiteButton.SetActive(false);

        Text[] plotText = plotPanel.GetComponentsInChildren<Text>();

        for(int i = 0; i < plotText.Length; i++)
        {
            if(plotText[i].tag == "PlotText")
            {
                plotText[i].text = "";
            }
            else if(plotText[i].tag == "ExpandOrShrinkText")
            {
                plotText[i].text = "Expand";
            }
        }

        plotPanel.SetActive(false);
        expand = false;
        shrink = true;

        Text[] posterInfoText = posterPanel.GetComponentsInChildren<Text>();

        for(int i = 0; i < posterInfoText.Length; i++)
        {
            if(posterInfoText[i].tag == "PosterInfoText")
            {
                posterInfoText[i].text = "";
            }
        }

        posterPanel.SetActive(false);
        poster.gameObject.SetActive(false);
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
            expand = true;
            shrink = false;
            searchForFullOrShortPlot = "http://www.omdbapi.com/?apikey=81876aef&i=" + movie.imdbID + "&plot=short";
        }
        StartCoroutine(GetFullOrShortPlot());
    }

    public IEnumerator GetPosterImage()
    {
        using(UnityWebRequest MovieInfoRequest = UnityWebRequestTexture.GetTexture(movie.Poster))
        {
            yield return MovieInfoRequest.SendWebRequest();

            if(MovieInfoRequest.isNetworkError || MovieInfoRequest.isHttpError)
            {
               // Debug.Log(MovieInfoRequest.error);
            }
            else
            {
                imageTexture = ((DownloadHandlerTexture)MovieInfoRequest.downloadHandler).texture;
                imageSprite = Sprite.Create(imageTexture, new Rect(0,0,imageTexture.width,imageTexture.height), new Vector2(0,0));
                poster.sprite = imageSprite;
            }
        }
    }

    public IEnumerator GetPosterInfo()
    {
        using(UnityWebRequest MovieInfoRequest = UnityWebRequest.Get(searchForImageInfo))
        {
            yield return MovieInfoRequest.SendWebRequest();

            if(MovieInfoRequest.isNetworkError || MovieInfoRequest.isHttpError)
            {
                Debug.Log(MovieInfoRequest.error);
            }
            else
            {
                imageData = JsonUtility.FromJson<ImageData.Data>(MovieInfoRequest.downloadHandler.text);
                if(imageData != null)
                {
                    SetPosterInfoText();
                }
            }
        }
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
                            plotText[i].text = "Expand";
                        }
                        else if (shrink)
                        {
                            plotText[i].text = "Shrink";
                        }
                    }
                }
            }
        }
    }

    public void GoToIMDBWebsite()
    {
        Application.OpenURL("https://www.imdb.com/title/" + movie.imdbID);
    }

    public void GoToMovieWebsite()
    {
        Application.OpenURL(movie.Website);
    }

    public void SetPosterInfoText()
    {
        Text[] posterInfoText = posterPanel.GetComponentsInChildren<Text>();

        for(int i = 0; i < posterInfoText.Length; i++)
        {
            if(posterInfoText[i].tag == "PosterInfoText")
            {
                if(imageData.FileType != null)
                {
                    posterInfoText[i].text += "File Type: " + imageData.FileType + "\n\n";
                }
                if(imageData.FileTypeExtension != null)
                {
                    posterInfoText[i].text += "File Type Extension: " + imageData.FileTypeExtension + "\n\n";
                }
                if(imageData.MIMEType != null)
                {
                    posterInfoText[i].text += "MIME Type: " + imageData.MIMEType + "\n\n";
                }
                if(imageData.JFIFVersion != 0)
                {
                    posterInfoText[i].text += "JFIF Version: " + imageData.JFIFVersion + "\n\n";
                }
                if(imageData.ResolutionUnit != null)
                {
                    posterInfoText[i].text += "Resolution Unit: " + imageData.ResolutionUnit + "\n\n";
                }
                if(imageData.XResolution != 0)
                {
                    posterInfoText[i].text += "X Resolution: " + imageData.XResolution + "\n\n";
                }
                if(imageData.YResolution != 0)
                {
                    posterInfoText[i].text += "Y Resoution: " + imageData.YResolution + "\n\n";
                }
                if(imageData.ProfileCMMType != null)
                {
                    posterInfoText[i].text += "Profile CMM Type: " + imageData.ProfileCMMType + "\n\n";
                }
                if(imageData.ProfileVersion != null)
                {
                    posterInfoText[i].text += "Profile Version: " + imageData.ProfileVersion + "\n\n";
                }
                if(imageData.ProfileClass != null)
                {
                    posterInfoText[i].text += "Profile Class: " + imageData.ProfileClass + "\n\n";
                }
                if(imageData.ColorSpaceData != null)
                {
                    posterInfoText[i].text += "Color Space Data: " + imageData.ColorSpaceData + "\n\n";
                }
                if(imageData.ProfileConnectionSpace != null)
                {
                    posterInfoText[i].text += "Profile Connection Space: " + imageData.ProfileConnectionSpace + "\n\n";
                }
                if(imageData.ProfileDateTime != null)
                {
                    posterInfoText[i].text += "Profile Date Time: " + imageData.ProfileDateTime + "\n\n";
                }
                if(imageData.ProfileFileSignature != null)
                {
                    posterInfoText[i].text += "Profile File Signature: " + imageData.ProfileFileSignature + "\n\n";
                }
                if(imageData.PrimaryPlatform != null)
                {
                    posterInfoText[i].text += "Primary Platform: " + imageData.PrimaryPlatform + "\n\n";
                }
                if(imageData.CMMFlags != null)
                {
                    posterInfoText[i].text += "CMM Flags: " + imageData.CMMFlags + "\n\n";
                }
                if(imageData.DeviceManufacturer != null)
                {
                    posterInfoText[i].text += "Device Manufacturer: " + imageData.DeviceManufacturer + "\n\n";
                }
                if(imageData.DeviceModel != null)
                {
                    posterInfoText[i].text += "Device Model: " + imageData.DeviceModel + "\n\n";
                }
                if(imageData.DeviceAttributes != null)
                {
                    posterInfoText[i].text += "Device Attributes: " + imageData.DeviceAttributes + "\n\n";
                }
                if(imageData.RenderingIntent != null)
                {
                    posterInfoText[i].text += "Renderin Intent: " + imageData.RenderingIntent + "\n\n";
                }
                if(imageData.ConnectionSpaceIlluminant != null)
                {
                    posterInfoText[i].text += "Connection Space Illuminant: " + imageData.ConnectionSpaceIlluminant + "\n\n";
                }
                if(imageData.ProfileCreator != null)
                {
                    posterInfoText[i].text += "Profile Creator: " + imageData.ProfileCreator + "\n\n";
                }
                if(imageData.ProfileID != 0)
                {
                    posterInfoText[i].text += "Profile ID: " + imageData.ProfileID + "\n\n";
                }
                if(imageData.ProfileDescription != null)
                {
                    posterInfoText[i].text += "Profile Description: " + imageData.ProfileDescription + "\n\n";
                }
                if(imageData.ProfileCopyright != null)
                {
                    posterInfoText[i].text += "Profile Copyright: " + imageData.ProfileCopyright + "\n\n";
                }
                if(imageData.MediaWhitePoint != null)
                {
                    posterInfoText[i].text += "Media White Point: " + imageData.MediaWhitePoint + "\n\n";
                }
                if(imageData.MediaBlackPoint != null)
                {
                    posterInfoText[i].text += "Media Black Point: " + imageData.MediaBlackPoint + "\n\n";
                }
                if(imageData.RedMatrixColumn != null)
                {
                    posterInfoText[i].text += "Red Matrix Column: " + imageData.RedMatrixColumn + "\n\n";
                }
                if(imageData.GreenMatrixColumn != null)
                {
                    posterInfoText[i].text += "Green Matrix Column: " + imageData.GreenMatrixColumn + "\n\n";
                }
                if(imageData.BlueMatrixColumn != null)
                {
                    posterInfoText[i].text += "Blue Matrix Column: " + imageData.BlueMatrixColumn + "\n\n";
                }
                if(imageData.ImageWidth != 0)
                {
                    posterInfoText[i].text += "Image Width: " + imageData.ImageWidth + "\n\n";
                }
                if(imageData.ImageHeight !=0)
                {
                    posterInfoText[i].text += "Image Height: " + imageData.ImageHeight + "\n\n";
                }
                if(imageData.EncodingProcess !=null)
                {
                    posterInfoText[i].text += "Encoding Process: " + imageData.EncodingProcess + "\n\n";
                }
                if(imageData.BitsPerSample != 0)
                {
                    posterInfoText[i].text += "Bits Per Sample: " + imageData.BitsPerSample + "\n\n";
                }
                if(imageData.ColorComponents != 0)
                {
                    posterInfoText[i].text += "Color Components: " + imageData.ColorComponents + "\n\n";
                }
                if(imageData.YCbCrSubSampling != null)
                {
                    posterInfoText[i].text += "YCbCr Sub Sampling: " + imageData.YCbCrSubSampling + "\n\n";
                }
                if(imageData.ImageSize != null)
                {
                    posterInfoText[i].text += "Image Size: " + imageData.ImageSize + "\n\n";
                }
                if(imageData.Megapixels != 0)
                {
                    posterInfoText[i].text += "Megapixels: " + imageData.Megapixels + "\n\n";
                }
            }
        }
    }
}
