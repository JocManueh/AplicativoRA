using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private GameObject model3DPrefab;

    [Header("Calibracion de orientacion")]
    [Tooltip("Grados extra en Y que se suman a la rotacion calculada. " +
             "Si el jugador queda detras del leñador en vez de al costado viendo el tronco, " +
             "probá 180. Si queda mirando para el otro lado, probá 90 o -90. Ajustalo hasta " +
             "que en Play, al colocarse, el leñador quede de perfil mirando al tronco.")]
    [SerializeField] private float extraYawOffset = 180f;

    private GameObject spawnedObject;
    private bool placementEnabled = false;

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

    // Llamado por GameManager cuando el jugador presiona "Start"
    public void EnablePlacement()
    {
        placementEnabled = true;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!placementEnabled) return;

        foreach (var plane in args.added)
            TryPlaceObject(plane);

        foreach (var plane in args.updated)
            TryPlaceObject(plane);
    }

    private void TryPlaceObject(ARPlane plane)
    {
        if (spawnedObject != null)
            return;

        // Solo colocamos sobre planos de piso (horizontales, normal hacia arriba).
        // Evita que se enganche una pared o el techo y el objeto aparezca volteado.
        if (plane.alignment != UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            return;

        float area = plane.size.x * plane.size.y;

        if (area > 0.3f)
        {
            // Base: eje Y siempre hacia arriba, "adelante" del GameRoot apuntando hacia
            // donde mira la camara en el momento de colocar.
            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
            if (forward.sqrMagnitude < 0.0001f)
                forward = plane.transform.forward;

            Quaternion baseRotation = Quaternion.LookRotation(forward.normalized, Vector3.up);

            // Offset calibrable: como no sabemos el eje "adelante" real del modelo del
            // leñador (viene de Sketchfab), este valor compensa esa diferencia para que
            // el jugador quede viendo el perfil del leñador talando, no su espalda.
            Quaternion rotation = baseRotation * Quaternion.Euler(0, extraYawOffset, 0);

            spawnedObject = Instantiate(model3DPrefab, plane.transform.position, rotation);

            StopPlaneDetection();

            if (GameManager.Instance != null)
                GameManager.Instance.OnGameplayReady();
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