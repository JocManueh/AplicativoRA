using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private GameObject model3DPrefab;

    private GameObject spawnedObject;

    private void OnEnable()
    {
        if (arPlaneManager != null)
            arPlaneManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        if (arPlaneManager != null)
            arPlaneManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            TryPlaceObject(plane);
        }

        foreach (var plane in args.updated)
        {
            TryPlaceObject(plane);
        }
    }

    private void TryPlaceObject(ARPlane plane)
    {
        if (spawnedObject != null)
            return;

        float area = plane.size.x * plane.size.y;

        if (area > 0.3f)
        {
            spawnedObject = Instantiate(
                model3DPrefab,
                plane.transform.position,
                plane.transform.rotation
            );

            StopPlaneDetection();
        }
    }

    private void StopPlaneDetection()
    {
        arPlaneManager.enabled = false;

        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}