using Abertay.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportHeatmap : MonoBehaviour
{
    public Color[] colourSelection;
    bool startTrigger;
    bool endTrigger;
    Vector3 startPos;
    Vector3 endPos;
    int teleportCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if (startTrigger && endTrigger)
        {
            startTrigger = false;
            endTrigger = false;
            Heatmap();
        }
    }

    // Update is called once per frame
    public void HeatmapStart(Vector3 position)
    {
        if (!startTrigger)
        {
            startTrigger = true;
            startPos = position;
        }
    }
    public void HeatmapEnd(Vector3 position)
    {
        if (!endTrigger)
        {
            endTrigger = true;
            endPos = position;
        }
    }
    void Heatmap()
    {
        //MAKE IT DO THIS AT THE END NOT EVERY TIME TELEPORT IS CALLED i.e MAKE IT AN ARRAY OF VALUES AND THEN PUT THEM ALL IN AT DA END
        // FIX COLOURS
        Color randomColour = colourSelection[Random.Range(0,5)];
        Color transparentRandomColour = new Color(randomColour.r, randomColour.g, randomColour.b, 0.5f);

       AnalyticsManager.LogHeatmapEvent("teleportStart" + teleportCount, startPos, transparentRandomColour);
       AnalyticsManager.LogHeatmapEvent("teleportEnd" + teleportCount, endPos, transparentRandomColour);
       teleportCount++;
    }
}
