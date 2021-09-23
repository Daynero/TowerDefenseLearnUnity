using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float maxPanSpeed = 0.1f;
    [SerializeField] private float minPanSpeed = 0.035f;
    [SerializeField] private float minFieldOfView = 25f;
    [SerializeField] private float maxFieldOfView = 65f;
    [SerializeField] private float maxZBoundsLimit = 20f;
    [SerializeField] private float maxXBoundsLimit = 30f;
    [SerializeField] private float pressingSpeed = 0.3f;
    [SerializeField] private Camera camera;

    private float _currentZBoundsLimit;
    private float _currentXBoundsLimit;
    private float _currentPanSpeed = 0.1f;
    private Vector3 _delta;
    private Vector3 _posStartCam;
    private Vector3 _initialTouchPosition;
    private Vector3 _initialCameraPosition;
    private Vector3 _initialTouch0Position;
    private Vector3 _initialTouch1Position;
    private Vector3 _initialMidPointScreen;
    private float _initialFieldOfView;
    private float _pressedTimer;

    [HideInInspector] public bool isDragging;
    [HideInInspector] public bool isZooming;

    private void Start()
    {
        _posStartCam = transform.position;
    }

    private void Update()
    {
        if (GameManager.GameIsOver)
        {
            enabled = false;
            return;
        }

        float fieldCamera = camera.fieldOfView;

        // Change PanSpeed proportionally FieldOfView
        _currentPanSpeed = (maxPanSpeed - minPanSpeed) * (fieldCamera - minFieldOfView)
            / (maxFieldOfView - minFieldOfView) + minPanSpeed;

        // Change limiting boundaries proportionally FieldOfView
        _currentXBoundsLimit = Mathf.Abs(fieldCamera - maxFieldOfView) * maxXBoundsLimit /
                              (maxFieldOfView - minFieldOfView);
        _currentZBoundsLimit = Mathf.Abs(fieldCamera - maxFieldOfView) * maxZBoundsLimit /
                              (maxFieldOfView - minFieldOfView);

        if (Input.touchCount == 1 || Input.touchCount == 2)
        {
            _pressedTimer += Time.deltaTime;
        }
        else
        {
            _pressedTimer = 0;
        }

        if (Input.touchCount == 1 && _pressedTimer >= pressingSpeed)
        {
            DragCamera();
        }

        if (Input.touchCount == 2 && _pressedTimer >= pressingSpeed)
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
                _initialTouchPosition = touch0.position;
                _initialCameraPosition = transform.position;

                isDragging = true;
            }
            else
            {
                _delta = (Vector3) touch0.position - _initialTouchPosition;
                Vector3 newPos = _initialCameraPosition;

                newPos.z += _delta.x * _currentPanSpeed;
                newPos.x -= _delta.y * _currentPanSpeed;

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
            _initialTouch0Position = touch0.position;
            _initialTouch1Position = touch1.position;
            _initialCameraPosition = transform.position;
            _initialFieldOfView = camera.fieldOfView;
            _initialMidPointScreen = (touch0.position + touch1.position) / 2;

            isZooming = true;
        }
        else
        {
            transform.position = _initialCameraPosition;
            camera.fieldOfView = _initialFieldOfView;

            float scaleFactor = GetScaleFactor(touch0.position,
                touch1.position,
                _initialTouch0Position,
                _initialTouch1Position);

            Vector2 currentMidPoint = (touch0.position + touch1.position) / 2;
            Vector3 initialPointWorldBeforeZoom = _initialMidPointScreen;

            camera.fieldOfView = Mathf.Clamp(_initialFieldOfView / scaleFactor, minFieldOfView, maxFieldOfView);

            Vector3 initialPointWorldAfterZoom = _initialMidPointScreen;
            Vector2 initialPointDelta = initialPointWorldBeforeZoom - initialPointWorldAfterZoom;

            Vector2 oldAndNewPointDelta = (Vector3) currentMidPoint - _initialMidPointScreen;

            Vector3 newPos = _initialCameraPosition;

            newPos.z += (oldAndNewPointDelta.x - initialPointDelta.x) * _currentPanSpeed;
            newPos.x -= (oldAndNewPointDelta.y - initialPointDelta.y) * _currentPanSpeed;

            transform.position = ClampNewCameraPosition(newPos);
        }
    }

    private Vector3 ClampNewCameraPosition(Vector3 newPos)
    {
        newPos.z = Mathf.Clamp(newPos.z, _posStartCam.z - _currentZBoundsLimit, _posStartCam.z + _currentZBoundsLimit);
        newPos.x = Mathf.Clamp(newPos.x, _posStartCam.x - _currentXBoundsLimit, _posStartCam.x + _currentXBoundsLimit);

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