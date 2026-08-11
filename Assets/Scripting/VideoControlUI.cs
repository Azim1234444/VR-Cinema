using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoControlUI : MonoBehaviour
{
    [Header("Video Setup")]
    [Tooltip("The VideoPlayer component on your MovieScreen")]
    public VideoPlayer videoPlayer;

    [Tooltip("The clip that this panel should play")]
    public VideoClip videoClip;

    [Header("UI Buttons")]
    public Button playButton;   //Play or resume
    public Button pauseButton;  //Pause / resume
    public Button stopButton;   //Stop

    private bool isPaused = false;

    void Start()
    {
        // Hook up button events
        if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
        if (pauseButton != null) pauseButton.onClick.AddListener(OnPauseClicked);
        if (stopButton != null) stopButton.onClick.AddListener(OnStopClicked);
    }

    private void OnPlayClicked()
    {
        // If we haven’t loaded this clip yet, set it
        if (videoPlayer.clip != videoClip)
            videoPlayer.clip = videoClip;

        videoPlayer.Play();
        isPaused = false;
    }

    private void OnPauseClicked()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            isPaused = true;
        }
        else if (isPaused)
        {
            videoPlayer.Play();
            isPaused = false;
        }
    }

    private void OnStopClicked()
    {
        videoPlayer.Stop();
        isPaused = false;
    }
}
