using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public FlowCell[,] mFlowGrid;
    public Vector2Int mSize;
    public float mCellRadius;

    public FlowField(float cellRadius, Vector2Int size)
    {
        mCellRadius = cellRadius;
        mSize = size;
    }

    public void InitFlowGrid()
    {
        mFlowGrid = new FlowCell[mSize.x, mSize.y];

        for (int i = 0; i < mSize.x; ++i)
        {
            for (int j = 0; j < mSize.y; ++j)
            {
                Vector3 currentWorldPos = new Vector3((mCellRadius * 2.0f) * i + mCellRadius, 0, (mCellRadius * 2.0f) * j + mCellRadius);
                mFlowGrid[i, j] = new FlowCell(currentWorldPos, new Vector2Int(i, j));
            }
        }
    }

    public void InitCost()
    {
        Vector3 halfExtend = Vector3.one * mCellRadius;
        int layerMask = LayerMask.GetMask("Impassable", "Terrian");
        foreach(FlowCell cell in mFlowGrid)
        {
            Collider[] obstacle = Physics.OverlapBox(cell.mWorldPos, halfExtend, Quaternion.identity, layerMask);
            bool hasCostIncreased = false;
            
            foreach(Collider col in obstacle)
            {
                if (col.gameObject.layer == 6) //Impassable Layer
                {
                    cell.IncCost(255);
                    continue;
                }
                else if (!hasCostIncreased && col.gameObject.layer == 7) //passable
                {
                    cell.IncCost(1);
                    hasCostIncreased = true;
                }
            }
        }
    }
}
