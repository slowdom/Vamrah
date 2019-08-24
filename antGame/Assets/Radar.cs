using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RadarObject
{
    public Image icon;
    public GameObject owner;
}

public class Radar : MonoBehaviour
{
    public Transform player;
    public float mapScale = 2;

    public List<RadarObject> radObjects = new List<RadarObject>();

    void Update()
    {
        DrawRadarDots();
    }

    void DrawRadarDots()
    {
        foreach(RadarObject ro in radObjects)
        {
            Vector3 radorPos = (ro.owner.transform.position - player.position);
            float distToObject = Vector3.Distance(player.position, ro.owner.transform.position) * mapScale;
            float deltay = Mathf.Atan2(radorPos.x, radorPos.z) * Mathf.Rad2Deg - 270 - player.eulerAngles.y;
            radorPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            radorPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

            ro.icon.transform.SetParent(transform);
            ro.icon.transform.position = new Vector3(radorPos.x, radorPos.z, 0) + transform.position;
        }
    }
}
