using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of positions in a 2D space spaced a margin apart. Returns every position only once, unless to many positions requeted.
/// </summary>
public class PositionList
{
    private List<Vector3> positions = new List<Vector3>();
    private int margin;
    private Vector2 xRange;
    private Vector2 zRange;

    private void FillList()
    {
        for(float i = xRange.x; i <= xRange.y; i += margin)
        {
            for(float j = zRange.x; j <= zRange.y; j +=margin)
            {
                positions.Add(new Vector3(i, 0, j));
                //Debug.Log("Position (" + i + ",0," + j +") added.");
            }
        }
    }

    public PositionList(int newMargin, Vector2 newXRange, Vector2 newZRange)
    {
        margin = newMargin;
        xRange = newXRange;
        zRange = newZRange;

        FillList();
    }

    public List<Vector3> GetCopyPositionList()
    {
        return positions;
    }

    /// <summary>
    /// Use this to get positions from the list. Deletes the position afterwards so positions are not assigned TWICE.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomPosition()
    {
        if(positions.Count <= 0)
        {
            FillList();
        }
        int rand = UnityEngine.Random.Range(0, positions.Count -1);
        Vector3 pos = positions[rand];
        positions.RemoveAt(rand);

        return pos;        
    }

    public Vector2 GetRandomPositionIn2D()
    {
        if (positions.Count <= 0)
        {
            FillList();
        }
        int rand = UnityEngine.Random.Range(0, positions.Count - 1);
        Vector3 pos = positions[rand];
        positions.RemoveAt(rand);
        Vector2 ret = new Vector2(pos.x, pos.z);
        return ret;
    }

    public int GetPositionCount()
    {
        return positions.Count;
    }
}
