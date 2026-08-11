using UnityEngine;

public class SnackSpawner : MonoBehaviour
{
    [Header("Snack Prefabs (match your buttons)")]
    public GameObject[] snackPrefabs;            // assign in Inspector: Coffee, Hotdog, etc.
    public float spawnDistance = 1f;             // 1 unit in front of camera

    private void SpawnSnack(int index)
    {
        if (index < 0 || index >= snackPrefabs.Length) return;

        // compute spawn position in front of camera
        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position + cam.transform.forward * spawnDistance;

        // instantiate & add interaction script
        GameObject snack = Instantiate(snackPrefabs[index], pos, Random.rotation);
        snack.AddComponent<SnackInteraction>();
        // ensure no gravity
        var rb = snack.GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = false;
    }

    // these methods can be hooked up from your UI OnClick()
    public void OnCoffeeClicked() => SpawnSnack(0);
    public void OnHotdogClicked() => SpawnSnack(1);
    public void OnPizzaClicked() => SpawnSnack(2);
    public void OnSodaClicked() => SpawnSnack(3);
    public void OnDonutClicked() => SpawnSnack(4);
}
