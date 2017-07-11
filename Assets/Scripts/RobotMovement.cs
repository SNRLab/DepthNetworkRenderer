using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RobotMovement : MonoBehaviour {
    const float KeyboardMultiplier = 50f;
    const float JoystickMultiplier = 50f;
    const float JoystickMovementMultiplier = 0.5f;
    const float MovementMultiplier = 50f;
    const int MovementType = 0;
    const float PitchLimit = 60;
    private const bool MovementAllowed = true;
    private int invert = 1;

    public GameObject AirwayCollision;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            invert = -invert;
        }

        if (MovementType == 0) {
            float rollAmount = 0;
            float pitchAmount = 0;
            float moveAmount = 0;

            if (Input.GetKey(KeyCode.RightArrow)) {
                rollAmount -= KeyboardMultiplier;
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                rollAmount += KeyboardMultiplier;
            }

            if (Input.GetKey(KeyCode.DownArrow)) {
                pitchAmount -= KeyboardMultiplier;
            }
            else if (Input.GetKey(KeyCode.UpArrow)) {
                pitchAmount += KeyboardMultiplier;
            }

            if (Input.GetKey(KeyCode.S)) {
                moveAmount -= MovementMultiplier;
            }
            else if (Input.GetKey(KeyCode.W)) {
                moveAmount += MovementMultiplier;
            }
            pitchAmount *= invert;

            RotateTip(rollAmount, pitchAmount, 0);
            MoveTip(moveAmount);
        }
        else if (MovementType == 1) {
            print("not in yet");
        }
    }

    private void RotateTip(float rollAmount, float pitchAmount, float yawAmount) {
        /*
        float currentPitch = gameObject.transform.localEulerAngles.x;
        //print ((currentPitch));
        if (!( ((currentPitch + pitchAmount*Time.deltaTime) > (360 - pitchLimit)) || ((currentPitch + pitchAmount*Time.deltaTime) < pitchLimit) )) {
            pitchAmount = 0f;
        }
        //*/

        //float currentPitch = gameObject.transform.localEulerAngles.z

        gameObject.transform.Rotate(
            (Vector3.forward * rollAmount + Vector3.right * pitchAmount + Vector3.up * yawAmount) * Time.deltaTime);
        //transform.position += move * speed * Time.deltaTime;
    }

    private void MoveTip(float moveAmount) {
        gameObject.transform.Translate(Vector3.forward * moveAmount * Time.deltaTime);
    }
}