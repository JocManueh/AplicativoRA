using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public GameObject logEmpty, logLeft, logRight;
    public float logHeight = 1.2f; // Altura LOCAL de un tronco dentro de GameRoot
    private List<GameObject> logsOnScreen = new List<GameObject>();

    void Start()
    {
        ResetTree();
    }

    // Reinicia el arbol completo (usado al reiniciar la partida)
    public void ResetTree()
    {
        foreach (var log in logsOnScreen)
        {
            if (log != null) Destroy(log);
        }
        logsOnScreen.Clear();

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

        GameObject newLog = Instantiate(logToSpawn, transform);

        newLog.transform.localPosition = new Vector3(0, positionIndex * logHeight, 0);
        newLog.transform.localRotation = Quaternion.identity;

        logsOnScreen.Add(newLog);
    }

    // NUEVO: consulta el tag del tronco de abajo SIN talarlo.
    // Se usa para decidir si el leñador esquivo la rama antes de talar.
    public string GetBottomLogTag()
    {
        if (logsOnScreen.Count == 0) return "Empty";
        return logsOnScreen[0].tag;
    }

    public void ChopBottomLog()
    {
        if (logsOnScreen.Count == 0) return;

        Destroy(logsOnScreen[0]);
        logsOnScreen.RemoveAt(0);

        foreach (var log in logsOnScreen)
        {
            log.transform.localPosition -= new Vector3(0, logHeight, 0);
        }

        SpawnLog(logsOnScreen.Count, false);
    }
}