using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoTileMaterial : MonoBehaviour
{
    public Vector2 textureScale = Vector2.one; // Controls texture tiling manually if needed

    private SpriteRenderer spriteRenderer;
    private Material materialInstance;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Duplicate the material to avoid affecting shared materials
        materialInstance = Instantiate(spriteRenderer.material);
        spriteRenderer.material = materialInstance;

        // Update the tiling based on sprite size and texture scale
        UpdateTiling();
    }

    void UpdateTiling()
    {
        if (spriteRenderer.sprite == null)
        {
            Debug.LogWarning("SpriteRenderer has no sprite assigned.");
            return;
        }

        // Get the world size of the sprite
        Vector2 spriteSize = spriteRenderer.bounds.size;

        // Calculate tiling based on sprite size and custom texture scale
        Vector2 newTiling = new Vector2(
            spriteSize.x / textureScale.x,
            spriteSize.y / textureScale.y
        );

        // Apply tiling to the material's main texture
        materialInstance.mainTextureScale = newTiling;
    }

    // Update tiling when changing values in the Inspector
    void OnValidate()
    {
        if (spriteRenderer != null)
        {
            UpdateTiling();
        }
    }
}
