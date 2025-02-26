using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float rayDuration = 2.0f;
    public float pullDuration = 3.0f;
    public float pullForce = 10.0f;
    public LayerMask groundLayer;
    public Transform rayOrigin;
    public Rigidbody rb;
    public LineRenderer lineRenderer;

    private float rayTimer = 0.0f;
    private float pullTimer = 0.0f;
    private bool isRayActive = false;
    private bool isPulling = false;
    private Vector3 hitPoint;
    private bool canTriggerRaycast = true;
    

    private void Update()
    {
        HandleInput();

        if (isRayActive)
        {
            ProcessRaycast();
        }

        if (isPulling)
        {
            PullPlayer();
            UpdateLineRenderer(true);
        }
        else
        {
            UpdateLineRenderer(false);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.T) && canTriggerRaycast)
        {
            StartRaycast();
        }
    }

    private void ProcessRaycast()
    {
        rayTimer += Time.deltaTime;
        if (rayTimer >= rayDuration)
        {
            isRayActive = false;
            rayTimer = 0.0f;
            if (!isPulling) // Only reset raycast ability if not pulling
            {
                canTriggerRaycast = true;
            }
        }
    }

    private void StartRaycast()
    {
        isRayActive = true;
        canTriggerRaycast = false; // Prevent further raycasts until this one fully completes
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, groundLayer))
        {
            hitPoint = hit.point;
            isPulling = true;
            rayTimer = 0.0f; // Reset ray timer as we found a target
        }
    }

    private void PullPlayer()
    {
        if (pullTimer < pullDuration)
        {
            pullTimer += Time.deltaTime;
            Vector3 direction = (hitPoint - transform.position).normalized;
            rb.AddForce(direction * pullForce);
        }
        else
        {
            isPulling = false;
            pullTimer = 0.0f;
            canTriggerRaycast = true; // Allow new raycast since pulling is done
        }
    }

    private void UpdateLineRenderer(bool isActive)
    {
        if (isActive)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin.position);
            lineRenderer.SetPosition(1, hitPoint);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
