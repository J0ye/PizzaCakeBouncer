using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Should be a component of the maincamera object. Sends and invokes events and methodes based on Input.touch. Handles OnTouchDown, -Up, -Stay and -Exit. Targets need TouchObjetScript for interaction.
/// This script is SINGELTON. Only one per scene.
/// </summary>
public class TouchControllScript : MonoBehaviour 
{
    public static TouchControllScript INSTANCE;

    public LayerMask touchInputMask;
    public float touchRange = 1f;

    public UnityEvent<Vector2> onTouch = new UnityEvent<Vector2>();
    public UnityEvent onNoTouch = new UnityEvent();

    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld;
    private RaycastHit rayHit;

    private void Awake()
    {
        if(INSTANCE)
        {
            Destroy(this);
        }
        else
        {
            INSTANCE = this;
        }
    }

    void Update () {
        // Handle editor input
        // Input in editor is made with the left mouse button but triggers same events as touch on device
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            // Process previous touches
            // To handle 'OnTouchExit' events
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();

            // 3D
            // Use ray to check for 3D objects in line with touch
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Vector2 touchPos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            onTouch.Invoke(touchPos);
            if (Physics.Raycast(ray, out rayHit, touchInputMask))
            {
                GameObject recipient = rayHit.transform.gameObject;

                touchList.Add(recipient);

                if (Input.GetMouseButtonDown(0))
                {
                    recipient.SendMessage("OnTouchDown", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    recipient.SendMessage("OnTouchUp", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }

                if (Input.GetMouseButton(0))
                {
                    recipient.SendMessage("OnTouchStay", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }
            }

            //2D
            // Use Circle cast to get 2D objects near touch
            RaycastHit2D hit2D = Physics2D.CircleCast(touchPos, touchRange, Vector2.one, touchInputMask);

            if(hit2D)
            {
                GameObject recipient = hit2D.collider.gameObject;

                touchList.Add(recipient);

                if (Input.GetMouseButtonDown(0))
                {
                    recipient.SendMessage("OnTouchDown", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    recipient.SendMessage("OnTouchUp", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }

                if (Input.GetMouseButton(0))
                {
                    recipient.SendMessage("OnTouchStay", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }
            }


            foreach (GameObject obj in touchesOld)
            {
                if(obj == null)
                {
                    // cards can be destroyed without being removed from this list
                    return;
                }

                if (!touchList.Contains(obj))
                {
                    obj.SendMessage("OnTouchTouchExit", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        else
        {
            onNoTouch?.Invoke();
        }
#endif
        // This code is actually executed on device
        if (Input.touchCount > 0)
        {
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();
            foreach (Touch touch in Input.touches)
            {
                // 3D
                Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out rayHit, touchInputMask))
                {
                    GameObject recipient = rayHit.transform.gameObject;

                    touchList.Add(recipient);

                    if (touch.phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        recipient.SendMessage("OnTouchStay", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (touch.phase == TouchPhase.Canceled)
                    {
                        recipient.SendMessage("OnTouchTouchExit", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
                Vector2 touchPos = GetComponent<Camera>().ScreenToWorldPoint(touch.position);
                onTouch.Invoke(touchPos);

                // 2D
                RaycastHit2D hit2D = Physics2D.CircleCast(touchPos, touchRange, Vector2.one, touchInputMask);

                if (hit2D)
                {
                    GameObject recipient = hit2D.collider.gameObject;

                    touchList.Add(recipient);

                    if (Input.GetMouseButtonDown(0))
                    {
                        recipient.SendMessage("OnTouchDown", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        recipient.SendMessage("OnTouchUp", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (Input.GetMouseButton(0))
                    {
                        recipient.SendMessage("OnTouchStay", rayHit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            foreach (GameObject obj in touchesOld)
            {
                if(!touchList.Contains(obj))
                {
                    obj.SendMessage("OnTouchTouchExit", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        else if(!Application.isEditor)
        {
            onNoTouch?.Invoke();
        }
	}
}
