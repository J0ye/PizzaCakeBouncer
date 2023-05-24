using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public GameObject prefab;
    public bool writeDebug = false;
    public List<Color> colors = new List<Color>();
    public float duration = 1f;
    public float margin = 30f;

    protected List<GameObject> messages = new List<GameObject>();

    public void WriteMessage(string msgtext)
    {
        if (!writeDebug) return;
        GameObject msgObj = Instantiate(prefab, transform);
        Text text;
        if (msgObj.transform.childCount > 0)
        {
            if (msgObj.transform.GetChild(0).TryGetComponent<Text>(out text))
            {
                text.text = msgtext;
            }
        }
        messages.Add(msgObj);
        MoveMessages(margin);
        StartCoroutine(DestroyMessageAfter(msgObj, duration));
    }

    public void WriteMessage(string msgtext, int colorIndex)
    {
        if (!writeDebug) return;
        GameObject msgObj = Instantiate(prefab, transform);
        Text text;
        if (msgObj.transform.childCount > 0)
        {
            if (msgObj.transform.GetChild(0).TryGetComponent<Text>(out text))
            {
                text.text = msgtext;
            }
        }
        messages.Add(msgObj);
        MoveMessages(margin);
        StartCoroutine(DestroyMessageAfter(msgObj, duration));
        Image img;
        if (msgObj.TryGetComponent<Image>(out img) && colorIndex < colors.Count)
        {
            img.color = colors[colorIndex];
        }
    }

    public void WriteMessage(string msgtext, Color newColor)
    {
        if (!writeDebug) return;
        GameObject msgObj = Instantiate(prefab, transform);
        Text text;
        if (msgObj.transform.childCount > 0)
        {
            if (msgObj.transform.GetChild(0).TryGetComponent<Text>(out text))
            {
                text.text = msgtext;
            }
        }
        messages.Add(msgObj);
        MoveMessages(margin);
        StartCoroutine(DestroyMessageAfter(msgObj, duration));
        Image img;
        if(msgObj.TryGetComponent<Image>(out img))
        {
            img.color = newColor;
        }
    }

    protected void MoveMessages(float padding)
    {
        foreach(GameObject g in messages)
        {
            RectTransform rt;
            if(g.TryGetComponent<RectTransform>(out rt))
            {
                rt.position = new Vector3(rt.position.x, rt.position.y - padding, rt.position.z);
            }
        }
    }

    protected IEnumerator DestroyMessageAfter(GameObject msg, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        if (Contains(msg)) messages.Remove(msg);
        Destroy(msg);
    }

    protected bool Contains(GameObject target)
    {
        foreach(GameObject g in messages)
        {
            if(g == target)
            {
                return true;
            }
        }
        return false;
    }
}
