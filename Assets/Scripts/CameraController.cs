using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float panSpeed = 3f;
    [SerializeField] private float panBorderThickness = 10f;
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float minY = 10f;
    [SerializeField] private float maxY = 80f;
    [SerializeField] private Camera camera;

    private bool drag = false;
    private Vector3 delta;
    private Vector3 startPos;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 startCamPos = new Vector3(camera.transform.position.z, camera.transform.position.x, 0);
            startPos = Input.mousePosition;
            Debug.Log("startCamPos = " + startCamPos + "   startPos = " + startPos);
            delta = startPos + startCamPos;

        } 
        else
        if (Input.GetMouseButton(0))
        {
            Debug.Log("delta = " + delta + "   startPos = " + startPos);
            Vector3 pos = (delta - Input.mousePosition);
            camera.transform.position = new Vector3(pos.y,camera.transform.position.y, pos.x);
        }
        
        
       
        
        // if (GameManager.gameIsOver)
        // {
        //     enabled = false;
        //     return;
        // }
        
        

        // if (Input.GetKey("w") )
        // {
        //     transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        // }
        //
        // if (Input.GetKey("s") )
        // {
        //     transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        // }
        //
        // if (Input.GetKey("d") )
        // {
        //     transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        // }
        //
        // if (Input.GetKey("a") )
        // {
        //     transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        // }
        //
        // float scroll = Input.GetAxis("Mouse ScrollWheel");
        //
        // Vector3 pos = transform.position;
        //
        // pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        // pos.y = Mathf.Clamp(pos.y, minY, maxY);
        //
        // transform.position = pos;
    }

    
    
}
