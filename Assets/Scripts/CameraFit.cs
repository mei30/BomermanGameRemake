using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraFitToMap : MonoBehaviour
{
    public Vector2Int mapSize;   // e.g., (31, 13)
    public float tileSize = 1f;  // size of each tile in Unity units

    private CinemachineCamera vcam;
    private CinemachinePositionComposer composer;

    private float baseXOffset = 0;
    private float baseYOffset = 0;
    private float mapWidth;
    float mapHeight;

    public Transform target;       // The player to follow

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        composer = GetComponent<CinemachinePositionComposer>();

        float aspect = (float)Screen.width / Screen.height;

        // world size in units
        mapWidth = mapSize.x * tileSize;
        mapHeight = mapSize.y * tileSize;

        // candidate sizes
        float verticalSize = mapHeight / 2f;

        vcam.Lens.OrthographicSize = verticalSize;

        float visibleWidth = (verticalSize * 2) * (aspect);

        baseXOffset = visibleWidth / 2f - 0.5f;
        baseYOffset = mapHeight / 2f - 0.5f;

        // Offset from tracking target
        //composer.TargetOffset = new Vector3(baseXOffset - 1, baseYOffset - 1);

        Debug.Log("Visible width:" + visibleWidth + "     apsect: " + aspect + "    vertical:" + verticalSize);

        // center the camera on the map
        vcam.transform.position = new Vector3(baseXOffset, baseYOffset, -10f);
    }

    void LateUpdate()
    {
        if (target == null)
            return;
        
        Debug.Log("vcam\n");
        Vector3 newPosition = target.position;
        newPosition.z = vcam.transform.position.z;

        float minX = baseXOffset;
        float maxX = mapWidth - 1;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, minX + (maxX - (baseXOffset * 2)));
        newPosition.y = baseYOffset;

        vcam.transform.position = newPosition;

    }
}
