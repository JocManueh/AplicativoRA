using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class CambioCara : MonoBehaviour
{
    [SerializeField] private ARFaceManager arFaceManager;
    [SerializeField] private List<Material> faceMaterials=new List<Material>(); // Lista de materiales para cambiar la cara

    private int indexMascara = 0; // Índice del material actual
                                  // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (arFaceManager==null)
            arFaceManager = FindAnyObjectByType<ARFaceManager>();
 
    }

    public void CambiarTextura()
    {
        foreach (var face in arFaceManager.trackables)
        {
            var Renderer = face.GetComponent<MeshRenderer>();
            Renderer.sharedMaterial = faceMaterials[indexMascara];
        }
        indexMascara++;

        if(indexMascara == faceMaterials.Count)
        {
            indexMascara = 0; // Reinicia el índice si supera la cantidad de materiales
        }
    }
}
