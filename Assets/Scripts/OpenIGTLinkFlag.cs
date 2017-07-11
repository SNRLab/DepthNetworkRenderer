using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenIGTLinkFlag : MonoBehaviour {
    public bool ReceiveTransform = false;
    public bool SendTransform = false;
    public bool SendPoint = false;

    private Vector3 lastPosition;
    private bool movedPosition;

    private const double Epsilon = 0.000001;

    private void Start() {
        lastPosition = transform.localPosition;
    }

    private void Update() {
        if (Mathf.Abs(lastPosition.x - transform.localPosition.x) > Epsilon |
            Mathf.Abs(lastPosition.y - transform.localPosition.y) > Epsilon |
            Mathf.Abs(lastPosition.z - transform.localPosition.z) > Epsilon) {
            movedPosition = true;
            lastPosition = transform.localPosition;
        }
        else {
            movedPosition = false;
        }
    }

    public bool GetMovedPosition() {
        return movedPosition;
    }
}