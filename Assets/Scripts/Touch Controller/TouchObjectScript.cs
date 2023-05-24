using UnityEngine;
using UnityEngine.Events;

public class TouchObjectScript : MonoBehaviour {

    public UnityEvent onDown = new UnityEvent();
    public UnityEvent onUp = new UnityEvent();
    public UnityEvent onStay = new UnityEvent();
    public UnityEvent onExit = new UnityEvent();

	private void OnTouchDown()
    {
        onDown.Invoke();
    }

    private void OnTouchUp()
    {
        onUp.Invoke();
    }

    private void OnTouchStay()
    {
        onStay.Invoke();           
    }

    private void OnTouchExit()
    {
        onExit.Invoke();
    }
}
