using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Portal Setup")]
    [Tooltip("Tag of the player object")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("The other portal you come out of")]
    [SerializeField] private Portal targetPortal;

    [Tooltip("Transform representing the exit point")]
    [SerializeField] private Transform spawnPoint;

    [Header("Timing")]
    [Tooltip("Time before the teleport happens (for VFX/SFX)")]
    [SerializeField] private float teleportDelay = 0.2f;

    [Tooltip("Cooldown after teleport, during which neither portal works")]
    [SerializeField] private float portalCooldown = 0.5f;

    [Header("Position Offset")]
    [Tooltip("Distance in front of the target portal you spawn")]
    [SerializeField] private float spawnOffset = 1f;

    // internal flag to block triggers
    private bool isCoolingDown = false;

    private void OnTriggerEnter(Collider other)
    {
        // only the player, and only if not cooling down
        if (isCoolingDown) return;
        if (!other.CompareTag(playerTag)) return;

        StartCoroutine(TeleportSequence(other));
    }

    private IEnumerator TeleportSequence(Collider player)
    {
        // block both portals
        isCoolingDown = true;
        targetPortal.isCoolingDown = true;

        // (optional) play enter-VFX here

        // wait before teleporting
        yield return new WaitForSeconds(teleportDelay);

        // calculate the exact exit position
        Vector3 exitPos = targetPortal.spawnPoint.position
                        + (targetPortal.spawnPoint.forward * spawnOffset);

        // disable movement so we don't get stuck
        var pm = player.GetComponent<PlayerMovement>();
        if (pm != null) pm.enabled = false;

        // teleport via CharacterController if available
        var cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = exitPos;
            yield return null;  // wait one frame
            cc.enabled = true;
        }
        else
        {
            player.transform.position = exitPos;
        }

        // (optional) play exit-VFX here

        // wait out the portal cooldown
        yield return new WaitForSeconds(portalCooldown);

        // re-enable player movement
        if (pm != null) pm.enabled = true;

        // unblock both portals
        targetPortal.isCoolingDown = false;
        isCoolingDown = false;
    }
}
