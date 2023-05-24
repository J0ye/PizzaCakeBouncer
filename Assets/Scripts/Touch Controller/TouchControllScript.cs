using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControllScript : MonoBehaviour {

    public LayerMask touchInputMask;
    public float touchRange = 1f;

    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld;
    private RaycastHit rayHit;

    // Update is called once per frame
    void Update () {

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();


            
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Vector2 touchPos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

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
                if (!touchList.Contains(obj))
                {
                    obj.SendMessage("OnTouchTouchExit", rayHit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
#endif

        if (Input.touchCount > 0)
        {
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();
            foreach (Touch touch in Input.touches)
            {
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
	}
}
