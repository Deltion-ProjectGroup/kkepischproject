using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] List<Camera> allCameras = new List<Camera>();
    [SerializeField] List<Camera> activeCameras = new List<Camera>();
    bool handleCameras = true;

    [SerializeField] float maximalDistanceBetweenPlayers = 10;
    [SerializeField] float cameraDistancePerExtraUnit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleCameras(bool value)
    {
        handleCameras = value;
    }
}
