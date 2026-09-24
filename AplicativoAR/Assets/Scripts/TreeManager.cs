using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public GameObject logEmpty, logLeft, logRight;
    public float logHeight = 1.2f; // Altura LOCAL de un tronco dentro de GameRoot
    private List<GameObject> logsOnScreen = new List<GameObject>();

    void Start()
    {
        // Crear los 5 troncos iniciales
        for (int i = 0; i < 5; i++)
        {
            SpawnLog(i, i == 0);
        }
    }

    public void SpawnLog(int positionIndex, bool forceEmpty)
    {
        GameObject logToSpawn = logEmpty;
        if (!forceEmpty)
        {
            int random = Random.Range(0, 3);
            if (random == 1) logToSpawn = logLeft;
            else if (random == 2) logToSpawn = logRight;
        }

        // Instanciar como hijo directo de GameRoot
        GameObject newLog = Instantiate(logToSpawn, transform);

        // POSICIÓN Y ROTACIÓN LOCAL (Mantiene la escala e inclinación exacta de GameRoot en AR)
        newLog.transform.localPosition = new Vector3(0, positionIndex * logHeight, 0);
        newLog.transform.localRotation = Quaternion.identity;

        logsOnScreen.Add(newLog);
    }

    public string ChopBottomLog()
    {
        if (logsOnScreen.Count == 0) return "Empty";

        Destroy(logsOnScreen[0]);
        logsOnScreen.RemoveAt(0);

        // Bajar todos los troncos en su eje Y local
        foreach (var log in logsOnScreen)
        {
            log.transform.localPosition -= new Vector3(0, logHeight, 0);
        }

        SpawnLog(logsOnScreen.Count, false);

        return logsOnScreen[0].tag;
    }
}