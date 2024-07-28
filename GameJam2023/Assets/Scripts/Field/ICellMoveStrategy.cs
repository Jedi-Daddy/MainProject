using UnityEngine;

namespace Game.Field
{
  public interface ICellMoveStrategy
  {
    Vector3? Direction { get; }

    void StartMove(Vector3 position);
    bool TryGetPositionOffsetFor(out Vector3 offset);
  }
}