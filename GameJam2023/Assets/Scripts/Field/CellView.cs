using System;
using Game.Tools;
using UnityEngine;

namespace Game.Field
{
  public class CellView : MonoBehaviour
  {
    public Vector2int CellPosition { get; set; }
    public Vector3 WorldPosition { get; private set; }
    public bool DragInProgress { get; set; }
    public bool MoveCompleted => !MoveTo.HasValue;
    public Vector3? MoveTo { get; set; }

    public event Action<CellView> OnStartDrag;
    public event Action<CellView> OnDrag;
    public event Action<CellView> OnDragComplete;
    public event Action<CellView> OnPositionChanged;

    public void Init(Vector2int cellPosition)
    {
      CellPosition = cellPosition;
      WorldPosition = transform.position;
    }

    private void Update()
    {
      if (!MoveTo.HasValue)
        return;
      
      if (MoveTo.Value.IsEqual(transform.position))
      {
        if (!DragInProgress && !MoveTo.Value.IsEqual(WorldPosition))
        {
          WorldPosition = transform.position;
          MoveTo = null;
          
          OnPositionChanged?.Invoke(this);
        }

        MoveTo = null;
        return; 
      }

      transform.position = Vector3.Lerp(transform.position, MoveTo.Value, Time.deltaTime * 80f);
    }

    private void OnMouseDown()
    {
      OnStartDrag?.Invoke(this);
    }
    
    private void OnMouseDrag()
    {
      OnDrag?.Invoke(this);
    }

    private void OnMouseUp()
    {
      if (!DragInProgress)
        return;
      
      OnDragComplete?.Invoke(this);
    }
  }
}