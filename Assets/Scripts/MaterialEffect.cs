using UnityEngine;
using System.Collections;

// Allows viewing camera in edit mode
[ExecuteInEditMode]
public class MaterialEffect : MonoBehaviour {
    public Material Material;

    void Start() {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, Material);
    }
}