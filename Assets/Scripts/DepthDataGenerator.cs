using System;
using System.IO;
using UnityEngine;

public class DepthDataGenerator : MonoBehaviour {
    public string DataDirectory;
    public Camera BRDFCamera;
    public Camera DepthCamera;

    private bool saveImages = false;

    private void renderCamera(Camera camera, RenderTexture renderTexture, Texture2D outputTexture) {
        Rect oldRect = camera.rect;
        camera.rect = new Rect(0, 0, 1, 1);
        RenderTexture oldActiveTexture = RenderTexture.active;
        RenderTexture oldTargetTexture = camera.targetTexture;
        RenderTexture.active = renderTexture;
        camera.targetTexture = renderTexture;
        camera.Render();
        outputTexture.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        outputTexture.Apply();
        RenderTexture.active = oldActiveTexture;
        camera.targetTexture = oldTargetTexture;
        camera.rect = oldRect;
    }

    private Texture2D renderBRDFCamera() {
        BRDFCamera.enabled = false;
        RenderTexture renderTexture = RenderTexture.GetTemporary(100, 100);
        Texture2D outputTexture = new Texture2D(renderTexture.width, renderTexture.height);
        renderCamera(BRDFCamera, renderTexture, outputTexture);
        BRDFCamera.enabled = true;
        RenderTexture.ReleaseTemporary(renderTexture);
        return outputTexture;
    }

    private Texture2D renderDepthCamera() {
        RenderTexture renderTexture = RenderTexture.GetTemporary(100, 100, 0, RenderTextureFormat.RFloat);
        Texture2D outputTexture =
            new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBAFloat, false);
        renderCamera(DepthCamera, renderTexture, outputTexture);
        RenderTexture.ReleaseTemporary(renderTexture);
        return outputTexture;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("s")) {
            saveImages = !saveImages;
            print(saveImages ? "Started saving images." : "Stopped saving images.");
        }
        if (saveImages) {
            var dateString = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_fff");
            Texture2D brdfTexture = renderBRDFCamera();
            byte[] brdfImagePng = brdfTexture.EncodeToPNG();
            File.WriteAllBytes(Path.Combine(DataDirectory, "brdf_" + dateString + ".png"), brdfImagePng);

            Texture2D depthTexture = renderDepthCamera();
            byte[] depthImageExr = depthTexture.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
            File.WriteAllBytes(Path.Combine(DataDirectory, "depth_" + dateString + ".exr"), depthImageExr);
        }
    }
}