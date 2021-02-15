using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] SpawnManager playerHandler;
    [SerializeField] Transform globalCamera;
    [SerializeField] float maxCameraDistance;
    [SerializeField] float minCameraDistance;
    [SerializeField] float playerBoundsOffset;
    [SerializeField] float distanceSpeedModifier = 1;
    [SerializeField] float smoothZoomModifier = 1;

    bool inAllowedRange = true;
    Vector3 defaultLocation;
    Vector3 targetLocation;

    Bounds currentBounds;

    [SerializeField] bool handleTheCameras = true;
    // Start is called before the first frame update
    void Start()
    {
        currentBounds = new Bounds();
        defaultLocation = globalCamera.position;
        targetLocation = defaultLocation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerHandler.localPlayers.Count > 0 && handleTheCameras)
        {
            Vector3 center = Vector3.zero;

            foreach(Player player in playerHandler.localPlayers)
            {
                center += player.transform.position;
            }

            center /= playerHandler.localPlayers.Count;

            targetLocation.x = center.x;

            Vector3 ogCameraPosition = globalCamera.position;
            globalCamera.position = targetLocation;

            float zoomAmount = Time.deltaTime * distanceSpeedModifier;

            if (IsEveryoneVisible())
            {
                Debug.Log("VISIBLE");
                targetLocation += globalCamera.forward * zoomAmount;

                globalCamera.position = targetLocation;

                if (!IsEveryoneVisible())
                {
                    targetLocation += -globalCamera.forward * zoomAmount;
                }
            }
            else
            {
                targetLocation += -globalCamera.forward * zoomAmount;
            }

            globalCamera.position = ogCameraPosition;

            globalCamera.position = Vector3.Lerp(globalCamera.position, targetLocation, smoothZoomModifier);


        }
    }

    bool IsEveryoneVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(globalCamera.GetComponent<Camera>());

        foreach(Player player in playerHandler.localPlayers)
        {
            Bounds bounds = player.GetComponent<Collider>().bounds;
            bounds.Expand(-playerBoundsOffset);

            if (!GeometryUtility.TestPlanesAABB(planes, bounds))
            {
                return false;
            }
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(currentBounds.center, currentBounds.size);
    }
}
