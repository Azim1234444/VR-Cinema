using UnityEngine;

public class HallManager : MonoBehaviour
{
    public GameObject hallCanvas;
    public GUIManager guiManager; // <-- add this

    void Start()
    {
        hallCanvas.SetActive(false);
    }

    public void ShowGUI()
    {
        hallCanvas.SetActive(true);
        guiManager.ShowMovieSelect(); // <-- show initial tab
    }

    public void HideGUI()
    {
        hallCanvas.SetActive(false);
    }
}
