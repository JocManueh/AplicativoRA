using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public GameObject logEmpty, logLeft, logRight;
    public float logHeight = 1.5f; // Altura de cada tronco (Ajusta según tu modelo)
    private List<GameObject> logsOnScreen = new List<GameObject>();

    void Start()
    {
        // Generar los primeros 5 troncos
        for (int i = 0; i < 5; i++) SpawnLog(i, i == 0);
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
        Vector3 spawnPos = transform.position + new Vector3(0, positionIndex * logHeight, 0);
        GameObject newLog = Instantiate(logToSpawn, spawnPos, Quaternion.identity, transform);
        logsOnScreen.Add(newLog);
    }

    public string ChopBottomLog()
    {
        if (logsOnScreen.Count == 0) return "Empty";

        Destroy(logsOnScreen[0]); // Destruye el de abajo
        logsOnScreen.RemoveAt(0);

        // Bajar los demás
        foreach (var log in logsOnScreen) log.transform.position -= new Vector3(0, logHeight, 0);

        SpawnLog(logsOnScreen.Count, false); // Crea uno nuevo arriba

        // Retornar qué tipo de tronco quedó abajo para verificar colisión
        return logsOnScreen[0].tag;
    }
}