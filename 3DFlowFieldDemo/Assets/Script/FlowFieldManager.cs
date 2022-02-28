using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlowFieldManager : MonoBehaviour
{
    //Enum for debug view
    public enum DebugDisplayType
    {
        OFF,
        COST_FIELD,
        INTEGRATION_FIELD
    };

    [Header("Flow Field Data")]
    public Vector2Int mSize;
    public float mCellRadius = 0.5f;
    public FlowField mFlowField;

    [Header("Debug")]
    public bool mShouldDisplayGrid;
    public DebugDisplayType mDebugType;
    

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitFlowField();
            mFlowField.InitCost();

            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            
            FlowCell end = mFlowField.ConvertWorldToCellPos(worldMousePos);
            mFlowField.InitIntergration(end);
        }
    }

    private void InitFlowField()
    {
        mFlowField = new FlowField(mCellRadius, mSize);
        mFlowField.InitFlowGrid();
    }

    private void OnDrawGizmos()
    {
        if (mShouldDisplayGrid && mFlowField == null) //Visualize grid before being made
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < mSize.x; ++i)
            {
                for (int j = 0; j < mSize.y; ++j)
                {
                    Vector3 center = new Vector3((mCellRadius * 2) * i + mCellRadius, 0, (mCellRadius * 2) * j + mCellRadius);
                    Gizmos.DrawCube(center, (Vector3.one * mCellRadius * 2));
                }
            }
        }
        else if (mShouldDisplayGrid && mFlowField != null) //Visualize Cost Field for created grid
        {
            switch (mDebugType)
            {
                case DebugDisplayType.COST_FIELD:

                    foreach (FlowCell cell in mFlowField.mFlowGrid)
                    {
                        Handles.Label(cell.mWorldPos, cell.mCost.ToString());
                    }
                    
                    break;

                case DebugDisplayType.INTEGRATION_FIELD:

                    foreach (FlowCell cell in mFlowField.mFlowGrid)
                    {
                        Handles.Label(cell.mWorldPos, cell.mBestCost.ToString());
                    }

                    break;

                case DebugDisplayType.OFF:

                    Debug.Log("Debug Now Off");
                    break;

                default:

                    Debug.LogError("ERROR: Invalid Debug Display Type");
                    break;
            }
        }
    }
}
