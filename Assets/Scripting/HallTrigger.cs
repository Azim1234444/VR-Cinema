using UnityEngine;
using UnityEngine.Video;

public class HallTrigger : MonoBehaviour
{
    [Header("GUI & Video References")]
    public HallManager manager;         // Your GUI controller
    public VideoPlayer videoPlayer;     // The MovieScreen's VideoPlayer

    [Header("Toggle Cooldown")]
    [Tooltip("Seconds before you can trigger again")]
    public float cooldownTime = 1f;

    bool canTrigger = true;
    bool isVisible = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!canTrigger || !other.CompareTag("Player")) return;

        // Toggle visibility
        if (!isVisible)
        {
            manager.ShowGUI();
            isVisible = true;
        }
        else
        {
            manager.HideGUI();
            if (videoPlayer != null)
                videoPlayer.Stop();
            isVisible = false;
        }

        // Start cooldown
        canTrigger = false;
        Invoke(nameof(ResetTrigger), cooldownTime);
    }

    private void ResetTrigger()
    {
        canTrigger = true;
    }
}
