using UnityEngine;

/*
 * Texture Resizer
 * Author: Johannes Bluhm
 * Change texture scale during edit mode
 */

[ExecuteInEditMode]
public class TextureResizer : MonoBehaviour {
    public Vector2 size = new Vector2(1, 1);
    private new Renderer renderer;

    private void OnValidate() {
        if (!renderer) {
            renderer = GetComponent<Renderer>();
        }
        
        Material tempMaterial = new Material(renderer.sharedMaterial);
        tempMaterial.mainTextureScale = size;
        renderer.sharedMaterial = tempMaterial;
    }
}