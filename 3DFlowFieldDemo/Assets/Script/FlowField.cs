using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public FlowCell[,] mFlowGrid;
    public Vector2Int mSize;
    public float mCellRadius;
    public FlowCell mCurrentEndCell;

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

    public void InitIntergration(FlowCell end)
    {
        mCurrentEndCell = end;

        mCurrentEndCell.mCost = 0;
        mCurrentEndCell.mBestCost = 0;

        Queue<FlowCell> openQueue = new Queue<FlowCell>();
        openQueue.Enqueue(mCurrentEndCell);

        while (openQueue.Count > 0)
        {
            FlowCell currentFlowCell = openQueue.Dequeue();
            List<FlowCell> neighbors = GetNeighbors(currentFlowCell.mIndex, FlowCellDirection.DirectionList);

            foreach(FlowCell currentNeighbor in neighbors)
            {
                if (currentNeighbor.mCost == int.MaxValue)
                {
                    continue;
                }
                else if (currentNeighbor.mCost + currentFlowCell.mBestCost < currentFlowCell.mBestCost)
                {
                    currentNeighbor.mBestCost = currentNeighbor.mCost + currentFlowCell.mBestCost;
                    openQueue.Enqueue(currentNeighbor);
                }
            }
        }
    }

    private List<FlowCell> GetNeighbors(Vector2Int index, List<FlowCellDirection> flowDir)
    {
        List<FlowCell> neighbors = new List<FlowCell>();

        foreach(Vector2Int dir in flowDir)
        {
            FlowCell currentNeighbor = GetCellNeighbor(index, dir);
            if (currentNeighbor != null)
            {
                neighbors.Add(currentNeighbor);
            }
        }


        return neighbors;
    }

    private FlowCell GetCellNeighbor(Vector2Int origin, Vector2Int pos)
    {
        Vector2Int newPos = origin + pos;

        if (newPos.x < 0 || newPos.x >= mSize.x || newPos.y < 0 || newPos.y >= mSize.y) // make sure we are in range
        {
            return null;
        }
        else
        {
            return mFlowGrid[newPos.x, newPos.y];
        }
    }

    public FlowCell ConvertWorldToCellPos(Vector3 worldPos)
    {
        float x = worldPos.x / (mSize.x * (mCellRadius * 2));
        float y = worldPos.y / (mSize.y * (mCellRadius * 2));

        x = Mathf.Clamp(x, 0, 1);
        y = Mathf.Clamp(y, 0, 1);

        int flowCellX = Mathf.Clamp(Mathf.FloorToInt((mSize.x) * x), 0, mSize.x - 1);
        int flowCellY = Mathf.Clamp(Mathf.FloorToInt((mSize.y) * y), 0, mSize.y - 1);

        return mFlowGrid[flowCellX, flowCellY];
    }
}
