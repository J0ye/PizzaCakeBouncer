using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{
    [Range(0.1f, 50f)]
    public float speed = 10f;
    [Range(2f, 50f)]
    public float sprintMultiplyer = 10f;
    [Range(1f, 10f)]
    public float sensitivity = 3f;
    [Range(1f, 10f)]
    public float smooth = 2f;

    private Vector2 mouseOrientation;
    private Vector2 smoothedVector;
    private Vector2 mouseLook;
    private bool active = true;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            // Activate sprint by pressing left shift
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed *= sprintMultiplyer;
            }
            
            // Deactivate sprint by releasing left shift
            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed /= sprintMultiplyer;
            }

            // Horizontal movement
            Vector3 direction = new Vector3(((Input.GetAxis("Horizontal") * speed) * Time.deltaTime), 0,(Input.GetAxis("Vertical") * speed) * Time.deltaTime);

            // Vertical movement
            if(Input.GetKey(KeyCode.Space))
            {
                direction.y = (1 *speed) * Time.deltaTime;
            }else if(Input.GetKey(KeyCode.LeftControl))
            {
                direction.y = ((1 *speed) * Time.deltaTime) * -1;
            }

            // Translation of positon to target
            if(direction != Vector3.zero)
            {
                transform.Translate(direction);
            }

            // Calculation of camera orientation
            mouseOrientation = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            mouseOrientation = Vector2.Scale(mouseOrientation, new Vector2(sensitivity * smooth, sensitivity * smooth));
            smoothedVector = new Vector2(Mathf.Lerp(smoothedVector.x, mouseOrientation.x, 1.0f / smooth), 
                                            Mathf.Lerp(smoothedVector.y, mouseOrientation.y, 1.0f / smooth));
            mouseLook += smoothedVector;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90);

            // Translation of camera orientation
            transform.rotation = Quaternion.Euler(-mouseLook.y, mouseLook.x, 0);   
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                active = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                active = true;
            }
        }
    }
}
