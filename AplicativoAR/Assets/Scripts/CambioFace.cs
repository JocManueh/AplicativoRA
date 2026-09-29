using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CambioCara : MonoBehaviour
{
    [SerializeField] private ARFaceManager faceManager;
    [SerializeField] private List<Material> mascaras = new List<Material>();

    private int nroMascara = 0;

    private void Awake()
    {
        if (faceManager == null)
            faceManager = FindAnyObjectByType<ARFaceManager>();
    }

    public void CambioTextura()
    {
        foreach (var face in faceManager.trackables)
        {
            var renderer = face.GetComponent<MeshRenderer>();

            renderer.sharedMaterial = mascaras[nroMascara];
        }

        nroMascara++;

        if (nroMascara == mascaras.Count)
        {
            nroMascara = 0;
        }
    }
}
