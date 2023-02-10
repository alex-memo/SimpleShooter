using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(0)]
public class WayPointManager : MonoBehaviour
{
    public static WayPointManager Instance;
    public List<Transform> Waypoints = new List<Transform>();
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { DestroyImmediate(gameObject); }
        foreach(Transform _child in transform)
        {
            Waypoints.Add(_child);
        }
    }
}
