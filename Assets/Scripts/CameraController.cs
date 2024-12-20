using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player1;  // Reference to Player 1's transform
    public Transform player2;  // Reference to Player 2's transform
    public float offsetY = 2.0f; // Vertical offset from the players
    public float smoothSpeed = 0.125f; // Speed of camera smoothing
    public float minZoom = 5f; // Minimum camera zoom
    public float maxZoom = 10f; // Maximum camera zoom
    public float zoomSpeed = 1f; // Speed of zoom adjustment



    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        // Get positions of both players
        Vector3 player1Pos = player1.position;
        Vector3 player2Pos = player2.position;
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

if(pos.x < 0.0) Debug.Log("I am left of the camera's view.");
if(1.0 < pos.x) Debug.Log("I am right of the camera's view.");
if(pos.y < 0.0) Debug.Log("I am below the camera's view.");
if(1.0 < pos.y) Debug.Log("I am above the camera's view.");


        // Calculate the center point between the players
        Vector3 centerPoint = (player1Pos + player2Pos) / 2;

        // Set camera position
        Vector3 desiredPosition = new Vector3(centerPoint.x, centerPoint.y + offsetY, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Calculate distance between players for zooming
        float distance = Vector3.Distance(player1Pos, player2Pos);
        float targetZoom = Mathf.Clamp(distance / 2, minZoom, maxZoom); // Adjust zoom level
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }
}
