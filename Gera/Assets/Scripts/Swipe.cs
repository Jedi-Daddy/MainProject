using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Swipe : MonoBehaviour, IEndDragHandler, IDragHandler
{
    public float MinVelocityThreshold = 30f;
    public float MinDistanceForSwipe = 80f;

    private Vector2 _distance;
    private Vector2 _prevPosition;
    private Vector2 _currentPosition;

    private Player[] _players;
    private void Start()
    {
        _players = transform.parent.GetComponentsInChildren<Player>().Where(x => x.isActiveAndEnabled).ToArray();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Drag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag(eventData);
    }

    private void Drag(PointerEventData eventData)
    {
        _distance += eventData.delta;
        _currentPosition = eventData.position;
    }

    private void LateUpdate()
    {
        if ((Math.Abs(_distance.x) < float.Epsilon) && (Math.Abs(_distance.y) < float.Epsilon))
            return;

        var velocity = (_currentPosition - _prevPosition).magnitude / Time.unscaledDeltaTime;
        if (velocity < MinVelocityThreshold)
        {
            if (Math.Abs(_distance.x) < MinDistanceForSwipe && Math.Abs(_distance.y) < MinDistanceForSwipe)
            {
                _distance = Vector2.zero;
                return;
            }

            if (_players.Any(x => !x.CanGo(_distance)))
            {
                _distance = Vector2.zero;
                return;
            }

            foreach (var player in _players)
                player.Move(_distance);

            _distance = Vector2.zero;
        }

        _prevPosition = _currentPosition;
    }
}
