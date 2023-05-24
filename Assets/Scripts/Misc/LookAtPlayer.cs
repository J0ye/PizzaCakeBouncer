using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public string lookFor = "Player";
    public GameObject player;
    public bool lookAway = false;

    // Start is called before the first frame update
    protected void Awake()
    {
        if (player == null && !string.IsNullOrEmpty(lookFor))
        {
            player = GameObject.FindGameObjectWithTag(lookFor);
        }
    }

    // Update is called once per frame
    protected void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogError("There is no " + lookFor + " object in the scene. Help me here: " + gameObject.name);
            if(GameObject.FindGameObjectWithTag(lookFor) != null)
            {
                player = GameObject.FindGameObjectWithTag(lookFor);
            }
        }
        else if(lookAway)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            Vector3 directionAwayFromPlayer = -directionToPlayer;
            transform.rotation = Quaternion.LookRotation(directionAwayFromPlayer);
        }
        else
        {
            transform.LookAt(player.transform.position);
        }
    }
}
