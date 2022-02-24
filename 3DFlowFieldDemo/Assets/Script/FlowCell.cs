using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowCell
{
    public Vector3 mWorldPos;
    public Vector2Int mIndex;
    public int mCost;

    const int MAX_COST = 100;

    public FlowCell(Vector3 worldPos, Vector2Int index)
    {
        mWorldPos = worldPos;
        mIndex = index;
    }

    public void IncCost(int incAmout)
    {
        if (mCost == MAX_COST)
        {
            return;
        }
        else if (incAmout + mCost >= MAX_COST)
        {
            mCost = MAX_COST;
        }
        else
        {
            mCost += incAmout;
        }
    }
}
