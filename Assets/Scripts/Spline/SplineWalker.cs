using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SplineWalker : MonoBehaviour {
    public enum Mode {
        Once,
        Loop,
        PingPong
    }

    public enum LookMode {
        None,
        Forward,
        Random
    }

    public BezierSpline spline;

    public float duration;

    public LookMode lookMode;

    public bool randomRotation;

    public Mode mode;

    private float progress;
    private bool goingForward = true;

    private void Update() {
        if (goingForward) {
            progress += Time.deltaTime / duration;
            if (progress > 1f) {
                if (mode == Mode.Once) {
                    progress = 1f;
                } else if (mode == Mode.Loop) {
                    progress -= 1f;
                } else {
                    progress = 2f - progress;
                    goingForward = false;
                }
            }
        } else {
            progress -= Time.deltaTime / duration;
            if (progress < 0f) {
                progress = -progress;
                goingForward = true;
            }
        }

        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;

        switch (lookMode) {
            case LookMode.Forward:
                Vector3 up;
                up = randomRotation ? Random.insideUnitSphere : Vector3.up;
                transform.LookAt(position + spline.GetDirection(progress), up);
                break;
            case LookMode.Random:
                transform.LookAt(position + Random.insideUnitSphere, Random.insideUnitSphere);
                break;
        }
    }
}