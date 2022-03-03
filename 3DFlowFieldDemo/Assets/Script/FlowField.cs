using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    enum GridLayers
    {
        IMPASSABLE = 6,
        SMOOTH_TERRAIN = 7,
        ROUGH_TERRAIN = 8
    }

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
        int layerMask = LayerMask.GetMask("Impassable", "Terrian", "RoughTerrian");
        foreach(FlowCell cell in mFlowGrid)
        {
            Collider[] obstacle = Physics.OverlapBox(cell.mWorldPos, halfExtend, Quaternion.identity, layerMask);
            bool hasCostIncreased = false;
            
            foreach(Collider col in obstacle)
            {
                if (col.gameObject.layer == (int)GridLayers.IMPASSABLE)
                {
                    cell.IncCost(1000);
                    continue;
                }
                else if (!hasCostIncreased && col.gameObject.layer == (int)GridLayers.ROUGH_TERRAIN)
                {
                    cell.IncCost(4);
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
                if (currentNeighbor.mCost + currentFlowCell.mBestCost < currentNeighbor.mBestCost)
                {
                    currentNeighbor.mBestCost = currentNeighbor.mCost + currentFlowCell.mBestCost;
                    openQueue.Enqueue(currentNeighbor);
                }
            }
        }
    }

    public void InitFlowField()
    {
        foreach(FlowCell cell in mFlowGrid)
        {
            List<FlowCell> neighbors = GetNeighbors(cell.mIndex, FlowCellDirection.DirectionList);
            int currentBestCost = cell.mBestCost;

            foreach(FlowCell neighbor in neighbors)
            {
                if (neighbor.mBestCost < currentBestCost)
                {
                    currentBestCost = neighbor.mBestCost;
                    cell.mBestDir = FlowCellDirection.GetDirection(neighbor.mIndex - cell.mIndex);
                }
            }
        }
    }

    private List<FlowCell> GetNeighbors(Vector2Int index, List<FlowCellDirection> flowDir)
    {
        List<FlowCell> neighbors = new List<FlowCell>();
        foreach(FlowCellDirection dir in flowDir)
        {
            Vector2Int newPos = index + dir.mDirVector;
            if (!(newPos.x < 0 || newPos.x >= mSize.x || newPos.y < 0 || newPos.y >= mSize.y)) // make sure we are in range
            {
                neighbors.Add(mFlowGrid[newPos.x, newPos.y]);
            }
        }


        return neighbors;
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
