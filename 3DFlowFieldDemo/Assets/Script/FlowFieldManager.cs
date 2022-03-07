using UnityEditor;
using UnityEngine;

public class FlowFieldManager : MonoBehaviour
{
    //Enum for debug view
    public enum DebugDisplayType
    {
        OFF,
        COST_FIELD,
        INTEGRATION_FIELD,
        FLOW_FIELD
    };

    [Header("Flow Field Data")]
    public Vector2Int mSize;
    public float mCellRadius = 0.5f;
    public FlowField mFlowField;

    [Header("Debug")]
    public bool mShouldDisplayGrid;
    public DebugDisplayType mDebugType;

    [Header("DebugSprites")]
    public Sprite mArrowSprite;
    public Sprite mXSprite;
    public Sprite mSmileSprite;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitFlowField();
            mFlowField.InitCost();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                FlowCell end = mFlowField.ConvertWorldToCellPos(hit.point);
                mFlowField.InitIntergration(end);
                mFlowField.InitFlowField();
            }
        }
    }

    private void InitFlowField()
    {
        mFlowField = new FlowField(mCellRadius, mSize);
        mFlowField.InitFlowGrid();
    }

    private void OnDrawGizmos()
    {
        if (mShouldDisplayGrid && mFlowField == null && mDebugType != DebugDisplayType.OFF) //Visualize grid before being made
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

                    ClearDisplay();
                    foreach (FlowCell cell in mFlowField.mFlowGrid)
                    {
                        Handles.Label(cell.mWorldPos, cell.mCost.ToString());
                    }

                    break;

                case DebugDisplayType.INTEGRATION_FIELD:

                    ClearDisplay();
                    foreach (FlowCell cell in mFlowField.mFlowGrid)
                    {
                        Handles.Label(cell.mWorldPos, cell.mBestCost.ToString());
                    }

                    break;

                case DebugDisplayType.FLOW_FIELD:

                    ClearDisplay();
                    foreach (FlowCell cell in mFlowField.mFlowGrid)
                    {
                        GameObject icon = new GameObject();
                        SpriteRenderer sr = icon.AddComponent<SpriteRenderer>();
                        icon.transform.parent = transform;
                        icon.transform.position = cell.mWorldPos + new Vector3(0.0f, 0.1f, 0.0f);
                        icon.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

                        if (cell.mCost == 0)
                        {
                            sr.sprite = mSmileSprite;
                            Quaternion newRot = Quaternion.Euler(90, 0, 0);
                            icon.transform.rotation = newRot;
                        }
                        else if (cell.mCost == cell.mMaxCost)
                        {
                            sr.sprite = mXSprite;
                            Quaternion newRot = Quaternion.Euler(90, 0, 0);
                            icon.transform.rotation = newRot;
                        }
                        else if (cell.mBestDir == FlowCellDirection.UpDir)
                        {
                            sr.sprite = mArrowSprite;
                            Quaternion newRot = Quaternion.Euler(90, 0, 0);
                            icon.transform.rotation = newRot;
                        }
                        else if (cell.mBestDir == FlowCellDirection.DownDir)
                        {
                            sr.sprite = mArrowSprite;
                            Quaternion newRot = Quaternion.Euler(90, 180, 0);
                            icon.transform.rotation = newRot;
                        }
                        else if (cell.mBestDir == FlowCellDirection.RightDir)
                        {
                            sr.sprite = mArrowSprite;
                            Quaternion newRot = Quaternion.Euler(90, 90, 0);
                            icon.transform.rotation = newRot;
                        }
                        else if (cell.mBestDir == FlowCellDirection.LeftDir)
                        {
                            sr.sprite = mArrowSprite;
                            Quaternion newRot = Quaternion.Euler(90, 270, 0);
                            icon.transform.rotation = newRot;
                        }
                        else if (cell.mBestDir == FlowCellDirection.TopRightDir)
                        {
                            sr.sprite = mArrowSprite;
                            Quaternion newRot = Quaternion.Euler(90, 45, 0);
                            icon.transform.rotation = newRot;
                        }
                        else if (cell.mBestDir == FlowCellDirection.TopLeftDir)
                        {
                            sr.sprite = mArrowSprite;
                            Quaternion newRot = Quaternion.Euler(90, 315, 0);
                            icon.transform.rotation = newRot;
                        }
                        else if (cell.mBestDir == FlowCellDirection.BottomLeftDir)
                        {
                            sr.sprite = mArrowSprite;
                            Quaternion newRot = Quaternion.Euler(90, 225, 0);
                            icon.transform.rotation = newRot;
                        }
                        else if (cell.mBestDir == FlowCellDirection.BottomRightDir)
                        {
                            sr.sprite = mArrowSprite;
                            Quaternion newRot = Quaternion.Euler(90, 135, 0);
                            icon.transform.rotation = newRot;
                        }
                    }
                    break;

                case DebugDisplayType.OFF:

                    ClearDisplay();
                    Debug.Log("Debug Now Off");
                    break;

                default:

                    Debug.LogError("ERROR: Invalid Debug Display Type");
                    break;
            }
        }
    }

    public void ClearDisplay()
    {
        foreach(Transform trans in transform)
        {
            GameObject.Destroy(trans.gameObject);
        }
    }
}
