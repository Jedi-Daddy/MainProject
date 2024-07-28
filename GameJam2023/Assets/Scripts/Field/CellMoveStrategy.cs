using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Game.Field
{
  public class CellMoveStrategyBase : ICellMoveStrategy
  {
    public Vector3? Direction { get; private set; }

    private Camera _camera;
    
    private Vector3 _startScreenPosition;
    private Vector3 _startMousePosition;

    public CellMoveStrategyBase(Camera camera)
    {
      _camera = camera;
    }

    public void StartMove(Vector3 position)
    {
      _startScreenPosition = _camera.WorldToScreenPoint(position);
      _startMousePosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _startScreenPosition.z));
      Direction = null;
    }

    public bool TryGetPositionOffsetFor(out Vector3 offset)
    {
      var curMousePosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _startScreenPosition.z));
      offset = curMousePosition - _startMousePosition;
      if (!Direction.HasValue)
      {
        if (Mathf.Abs(offset.x - offset.y) < 0.0000001f)
          return false;

        Direction = Mathf.Abs(offset.x) > Mathf.Abs(offset.y) 
          ? new Vector3(Mathf.Sign(offset.x), 0f, 0f) 
          : new Vector3(0f, Mathf.Sign(offset.y), 0f);
      }
      
      _startMousePosition = curMousePosition;
      
      if (Direction == Vector3.up || Direction == Vector3.down)
        offset = new Vector3(0f, offset.y, 0f);
      if (Direction == Vector3.right || Direction == Vector3.left)
        offset = new Vector3(offset.x, 0f, 0f);
      
      return true;
    }
  }
}