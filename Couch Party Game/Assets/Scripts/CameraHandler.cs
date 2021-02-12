using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] SpawnManager playerHandler;
    [SerializeField] Transform globalCamera;
    [SerializeField] float maxCameraDistance, distancePerUnitApart;
    [SerializeField] float minDistanceBeforeEffect;
    [SerializeField] float playerBoundsOffset;
    [SerializeField] float zoomSpeed = 1;

    bool inAllowedRange = true;
    Vector3 defaultLocation;
    Vector3 targetLocation;

    [SerializeField] bool handleTheCameras = true;
    // Start is called before the first frame update
    void Start()
    {
        defaultLocation = globalCamera.position;
        targetLocation = defaultLocation;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHandler.localPlayers.Count > 0 && handleTheCameras)
        {
            Vector3 center = Vector3.zero;

            foreach(Player player in playerHandler.localPlayers)
            {
                center += player.transform.position;
            }

            targetLocation.x = center.x;

            center /= playerHandler.localPlayers.Count;
            Bounds bounds = new Bounds(center, Vector3.zero);

            foreach (Player player in playerHandler.localPlayers)
            {
                bounds.Encapsulate(player.transform.position);
            }
            bounds.Expand(-playerBoundsOffset);

            Camera cam = globalCamera.GetComponent<Camera>();
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

            float zoomAmount = Time.deltaTime * zoomSpeed;
            Vector3 ogCameraPosition = globalCamera.position;
            if (GeometryUtility.TestPlanesAABB(planes, bounds))
            {
                Debug.Log("PLAYERS ARE IN BOUNDS");
                targetLocation += globalCamera.forward * zoomAmount;
                globalCamera.position = targetLocation;

                if (!GeometryUtility.TestPlanesAABB(planes, bounds))
                {
                    Debug.Log("PLAYERS ARE NOT IN BOUNDS ANYMORE, REVERT");

                    targetLocation += -globalCamera.forward * zoomAmount;
                }

                globalCamera.position = ogCameraPosition;
            }
            else
            {
                Debug.Log("PLAYERS ARE NOT IN BOUNDS");
                targetLocation += -globalCamera.forward * zoomAmount;
            }

            globalCamera.position = Vector3.Lerp(globalCamera.position, targetLocation, zoomSpeed);


        }
    }
}
