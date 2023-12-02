using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialMove : MonoBehaviour
{
    public Transform needleTransform;
    public float rotationSpeed = 50.0f;
    private float currentRotation = 0.0f;
    private float rotationDirection = 1.0f;

    public LineRenderer lineRenderer;
    public GameObject maskPrefab;
    public DigScript digScript;

    public float lineCooldown;
    public bool canCreateLine = true;

    private bool isTouchingScreen = false;

    void Start()
    {
        lineRenderer.positionCount = 0; // Initialize with no points
    }

    void Update()
    {
        MoveNeedle();

        //Only check for taps after pre game canvas is done
        if (!digScript.preGameCanvas.activeSelf)
        {
            CheckForTaps();
        }
    }

    private void CheckForTaps()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            if (touch.phase == TouchPhase.Began)
            {
                if (canCreateLine)
                {
                    isTouchingScreen = true;
                    CreateLine();
                    StartCoroutine(LineCooldown()); // Start the cooldown timer.
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isTouchingScreen = false;
            }
        }
    }

    private void CreateLine()
    {
        if (lineRenderer != null && isTouchingScreen)
        {
            // Get the current angle of the needle
            float currentAngle = needleTransform.localRotation.eulerAngles.z;
            currentAngle += 180;

            // Calculate the direction vector based on the needle angle
            Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * Vector3.up;

            // Set the positions of the line's start and end points
            Vector3 startPoint = needleTransform.position;
            Vector3 endPoint = needleTransform.position + direction * 15f;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);

            //Turn line black
            Material blackMaterial = new Material(Shader.Find("Unlit/Color"));
            blackMaterial.color = Color.black;
            lineRenderer.material = blackMaterial;

            //Create mask
            GameObject capsuleMask = Instantiate(maskPrefab);
            Transform capsuleTransform = capsuleMask.transform;
            capsuleTransform.position = endPoint;
            capsuleTransform.rotation = Quaternion.Euler(0, 0, currentAngle);

            canCreateLine = false;
        }
    }

    private IEnumerator LineCooldown()
    {
        yield return new WaitForSeconds(lineCooldown);
        canCreateLine = true; // Allow creating lines again after cooldown.
    }

    private void MoveNeedle()
    {
        if (needleTransform != null)
        {
            currentRotation += rotationSpeed * rotationDirection * Time.deltaTime;
            needleTransform.localRotation = Quaternion.Euler(0, 0, currentRotation);

            if (currentRotation >= 90.0f || currentRotation <= -90.0f)
            {
                rotationDirection *= -1.0f;
            }
        }
    }
}