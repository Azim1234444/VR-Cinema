using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [Header("Main GUI Panels")]
    public GameObject movieSelectPanel;
    public GameObject snackPanel;
    public GameObject[] movieInfoPanels; // Array for GUI2–GUI9

    [Header("Tab Buttons")]
    public GameObject btnMovieSelector;
    public GameObject btnFoodBeverages;

    void Start()
    {
        HideAll();
    }

    public void ShowMovieSelect()
    {
        HideAll();
        movieSelectPanel.SetActive(true);
        btnMovieSelector.SetActive(true);
        btnFoodBeverages.SetActive(true);
    }

    public void ShowSnackPanel()
    {
        HideAll();
        snackPanel.SetActive(true);
        btnMovieSelector.SetActive(true);
        btnFoodBeverages.SetActive(true);
    }

    public void ShowMovieInfo(int index)
    {
        HideAll();
        if (index >= 0 && index < movieInfoPanels.Length)
            movieInfoPanels[index].SetActive(true);

        btnMovieSelector.SetActive(true);
        btnFoodBeverages.SetActive(true);
    }

    private void HideAll()
    {
        movieSelectPanel.SetActive(false);
        snackPanel.SetActive(false);
        foreach (var panel in movieInfoPanels)
        {
            panel.SetActive(false);
        }
    }
}
