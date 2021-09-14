using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float maxPanSpeed = 0.1f;
    [SerializeField] private float minPanSpeed = 0.035f;
    [SerializeField] private float minFieldOfView = 25f;
    [SerializeField] private float maxFieldOfView = 65f;
    [SerializeField] private float maxZBoundsLimit = 20f;
    [SerializeField] private float maxXBoundsLimit  = 30f;
    [SerializeField] private Camera camera;

    private float currentZBoundsLimit;
    private float currentXBoundsLimit;
    private Vector3 posStartCam;
    private bool isDragging = false;
    private bool isZooming = false;
    private float currentPanSpeed = 0.1f;
    private Vector3 delta;
    private Vector3 startPos;
    private Vector3 initialTouchPosition;
    private Vector3 initialCameraPosition;
    private Vector3 initialTouch0Position;
    private Vector3 initialTouch1Position;
    private Vector3 initialMidPointScreen;
    private float initialFieldOfView;

    private void Start()
    {
        posStartCam = transform.position;
    }

    private void Update()
    {
        if (GameManager.gameIsOver)
        {
            enabled = false;
            return;
        }

        float fieldCamera = camera.fieldOfView;
        
        // Change PanSpeed proportionally FieldOfView
        currentPanSpeed = (maxPanSpeed - minPanSpeed) * (fieldCamera - minFieldOfView) 
            / (maxFieldOfView - minFieldOfView) + minPanSpeed;
        
        // Change limiting boundaries proportionally FieldOfView
        currentXBoundsLimit =  Mathf.Abs(fieldCamera - maxFieldOfView) * maxXBoundsLimit  / (maxFieldOfView - minFieldOfView) ;
        currentZBoundsLimit =  Mathf.Abs(fieldCamera - maxFieldOfView) * maxZBoundsLimit  / (maxFieldOfView - minFieldOfView) ;
        
        if (Input.touchCount == 1)
        {
            DragCamera();
        }

        if (Input.touchCount == 2)
        {
            ZoomAndDragCamera();
        }
        else
        {
            isZooming = false;
        }
    }

    private void DragCamera()
    {
        Touch touch0 = Input.GetTouch(0);

        if (IsTouching(touch0))
        {
            if (!isDragging)
            {
                initialTouchPosition = touch0.position;
                initialCameraPosition = transform.position;

                isDragging = true;
            }
            else
            {
                delta = (Vector3) touch0.position - initialTouchPosition;
                Vector3 newPos = initialCameraPosition;

                newPos.z += delta.x * currentPanSpeed;
                newPos.x -= delta.y * currentPanSpeed;
                
                transform.position = ClampNewCameraPosition(newPos);
            }
        }
            
        if (!IsTouching(touch0))
        {
            isDragging = false;
        }
    }

    private void ZoomAndDragCamera()
    {
        isDragging = false;

            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (!isZooming)
            {
                initialTouch0Position = touch0.position;
                initialTouch1Position = touch1.position;
                initialCameraPosition = transform.position;
                initialFieldOfView = camera.fieldOfView;
                initialMidPointScreen = (touch0.position + touch1.position) / 2;

                isZooming = true;
            }
            else
            {
                transform.position = initialCameraPosition;
                camera.fieldOfView = initialFieldOfView;

                float scaleFactor = GetScaleFactor(touch0.position,
                                                   touch1.position,
                                                   initialTouch0Position,
                                                   initialTouch1Position);
                
                Vector2 currentMidPoint = (touch0.position + touch1.position) / 2;
                Vector3 initialPointWorldBeforeZoom = initialMidPointScreen;

                camera.fieldOfView = Mathf.Clamp(initialFieldOfView / scaleFactor, minFieldOfView, maxFieldOfView);

                Vector3 initialPointWorldAfterZoom = initialMidPointScreen;
                Vector2 initialPointDelta = initialPointWorldBeforeZoom - initialPointWorldAfterZoom;

                Vector2 oldAndNewPointDelta = (Vector3)currentMidPoint - initialMidPointScreen;

                Vector3 newPos = initialCameraPosition;
                
                newPos.z += (oldAndNewPointDelta.x - initialPointDelta.x) * currentPanSpeed;
                newPos.x -= (oldAndNewPointDelta.y - initialPointDelta.y) * currentPanSpeed;

                transform.position = ClampNewCameraPosition(newPos);
            }
    }

    private Vector3 ClampNewCameraPosition(Vector3 newPos)
    {
        newPos.z = Mathf.Clamp(newPos.z,  posStartCam.z - currentZBoundsLimit, posStartCam.z + currentZBoundsLimit);
        newPos.x = Mathf.Clamp(newPos.x,  posStartCam.x - currentXBoundsLimit, posStartCam.x + currentXBoundsLimit);

        return newPos;
    }

    private bool IsTouching(Touch touch)
    {
        return touch.phase == TouchPhase.Began ||
               touch.phase == TouchPhase.Moved ||
               touch.phase == TouchPhase.Stationary;
    }
    
    private float GetScaleFactor(Vector2 position1, Vector2 position2, Vector2 oldPosition1, Vector2 oldPosition2)
    {
        float distance = Vector2.Distance(position1, position2);
        float oldDistance = Vector2.Distance(oldPosition1, oldPosition2);

        if (oldDistance == 0 || distance == 0)
        {
            return 1.0f;
        }

        return distance / oldDistance;
    }
}