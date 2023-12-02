using UnityEngine;

public class DragIndicatorScript : MonoBehaviour
{
    public const float LINE_Z_POS = -0.5f;
    
    public LineRenderer lr;
    public bool startPosSet, endPosSet;


    public void UpdateLine()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform != null)
            {
                if (hit.transform == this.transform.root)
                {
                    lr.enabled = true;
                    SetLinePos(0, transform.position);
                    startPosSet = true;
                }
            }
        }
        if (Input.GetMouseButton(0) && !endPosSet) 
        {
            if (!startPosSet)
            {
                return;
            }
            SetLinePos(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (Input.GetMouseButtonUp(0) && !endPosSet)
        {
            lr.enabled = false;
            startPosSet = false;
        }
    }

    public void SetLinePos(int index, Vector3 pos)
    {
        Vector3 newPos = pos;
        newPos.z = LINE_Z_POS;
        lr.SetPosition(index, newPos);
    }
}
