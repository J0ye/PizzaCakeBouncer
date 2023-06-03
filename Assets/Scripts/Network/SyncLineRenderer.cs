using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(LineRenderer))]
public class SyncLineRenderer : MonoBehaviourPun
{
    public int maxPositions = 100;
    public float punCoolDown = 10f;

    private LineRenderer lineRenderer;
    private Queue<Vector3> positions = new Queue<Vector3>();
    private float punTimer = 0;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        TouchControllScript.INSTANCE.onTouch.AddListener(AddPositionToQueue);
        TouchControllScript.INSTANCE.onNoTouch.AddListener(IdlePositionQueue);
    }

    public void AddPositionToQueue(Vector2 position)
    {
        if (!photonView.IsMine) return;

        // Add new position and maintain the maximum number of positions.
        if (positions.Count >= maxPositions)
        {
            positions.Dequeue();
        }
        positions.Enqueue(position);

        RenderLine();
    }

    public void IdlePositionQueue()
    {
        if (!photonView.IsMine) return;

        if (positions.Count > 0)
        {
            // Erase positions one by one while the left mouse button is not pushed down.
            positions.Dequeue();
        }
        RenderLine();
    }

    private void RenderLine()
    {
        // Update the line renderer with the new positions.
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

        // Convert positions to a serialized format to send over the network.
        float[] serializedPositions = new float[positions.Count * 3];
        int i = 0;
        foreach (Vector3 pos in positions)
        {
            serializedPositions[i++] = pos.x;
            serializedPositions[i++] = pos.y;
            serializedPositions[i++] = pos.z;
        }

        punTimer += Time.deltaTime;
        if(punTimer > punCoolDown)
        {
            // Call the RPC to sync the line renderer across the network.
            photonView.RPC("SyncLineRendererPositions", RpcTarget.Others, serializedPositions);
            punTimer = 0;
        }

    }

    [PunRPC]
    public void SyncLineRendererPositions(float[] serializedPositions)
    {
        // Convert serialized positions back to a queue of Vector3s.
        positions = new Queue<Vector3>();
        for (int i = 0; i < serializedPositions.Length; i += 3)
        {
            positions.Enqueue(new Vector3(serializedPositions[i], serializedPositions[i + 1], serializedPositions[i + 2]));
        }

        // Update the line renderer with the new positions.
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
