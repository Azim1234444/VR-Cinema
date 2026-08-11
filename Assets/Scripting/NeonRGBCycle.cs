using UnityEngine;

public class NeonRGBController : MonoBehaviour
{
    [Tooltip("How fast the color shifts between RGB")]
    public float cycleSpeed = 1f;

    private Material[] materials;

    void Start()
    {
        // Get MeshRenderers from all children
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        materials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            // Instantiate materials so changes don't affect shared assets
            materials[i] = renderers[i].material;
        }
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * cycleSpeed, 3f);
        Color color;

        if (t < 1f)
            color = Color.Lerp(Color.red, Color.green, t);
        else if (t < 2f)
            color = Color.Lerp(Color.green, Color.blue, t - 1f);
        else
            color = Color.Lerp(Color.blue, Color.red, t - 2f);

        foreach (Material mat in materials)
        {
            mat.color = color;

            if (mat.HasProperty("_EmissionColor"))
            {
                mat.SetColor("_EmissionColor", color * 2f); // Glow!
            }
        }
    }
}
