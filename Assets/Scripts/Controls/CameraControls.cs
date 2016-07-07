using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour
{
    public static CameraControls instance;

    public Vector3 upVector;
    public Vector3 rightVector;
    public float currentZoom;
    public float minZoom;
    public float maxZoom;
    public Camera cam;
    public AnimationCurve zoomDistance;
    public AnimationCurve zoomMoveScale;
    public float cameraSpeed;
    public float lerpSpeed;
    public float zoomSpeed;

    private Vector3 offset;
    private Vector3 targetPosition;
    

    void Awake()
    {
        instance = this;
        offset = cam.transform.localPosition.normalized;
        targetPosition = transform.position;
    }


	
	// Update is called once per frame
	void Update ()
    {
        cam.transform.localPosition = zoomDistance.Evaluate(currentZoom) * offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.05f)
            targetPosition += rightVector * Input.GetAxis("Horizontal") * zoomMoveScale.Evaluate(currentZoom) * Time.deltaTime * cameraSpeed ;

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.05f)
            targetPosition += upVector * Input.GetAxis("Vertical") * zoomMoveScale.Evaluate(currentZoom) * Time.deltaTime * cameraSpeed;

        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.05f)
        {
            currentZoom += Input.GetAxis("Mouse ScrollWheel")  * Time.deltaTime * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }
            

    }
}
