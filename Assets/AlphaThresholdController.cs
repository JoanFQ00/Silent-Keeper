using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class AlphaThresholdController : MonoBehaviour
{
    [Range(-0.5f, 0.5f)]
    public float offsetX = 0f;

    [Range(-0.5f, 0.5f)]
    public float offsetY = 0f;

    [Range(0f, 3f)]
    public float scale = 1f;

    private Material material;

    void Awake()
    {
        SetupMaterial();
    }

    void OnEnable()
    {
        SetupMaterial();
    }

    void SetupMaterial()
    {
        RawImage rawImage = GetComponent<RawImage>();
        if (rawImage != null)
        {
            if (Application.isPlaying)
            {
                material = Instantiate(rawImage.material);
                rawImage.material = material;
            }
            else
            {
                material = rawImage.material;
            }
        }
    }

    void Update()
    {
        if (material != null)
        {
            material.SetVector("_Offset", new Vector2(offsetX, offsetY));
            material.SetVector("_Scale", new Vector2(scale, scale));  // Escala uniforme
        }
    }

    void OnValidate()
    {
        if (material != null)
        {
            material.SetVector("_Offset", new Vector2(offsetX, offsetY));
            material.SetVector("_Scale", new Vector2(scale, scale));  // Escala uniforme
        }
    }
}