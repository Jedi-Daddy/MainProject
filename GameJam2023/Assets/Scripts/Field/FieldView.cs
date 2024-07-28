using System;
using System.Collections.Generic;
using System.Linq;
using Game.Tools;
using UnityEngine;

namespace Game.Field
{
    [Flags]
    public enum Directions
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
    }
    
    public class FieldView : MonoBehaviour
    {
        public class Cell
        {
            public Directions Direction { get; set; }
            public bool IsFree { get; set; }
        
            public Cell(Directions direction, bool free)
            {
                Direction = direction;
                IsFree = free;
            }
        }
        
    [SerializeField] public bool UseSound = false;
    [SerializeField] private Vector2 _cellSize;
    [SerializeField] private Camera _camera;

    public bool EnableMove = true;

    public event Action OnSelectFail;
    public event Action OnPostitionChanged;

    private AudioSource _audioSource;

    private Cell[,] _field;
    private List<List<CellView>> _groups;

    private ICellMoveStrategy _moveStrategy;

    private int _failedMoveCount = 0;
    private float _moveLastTime = 0f;

        private void Start()
        {
          _moveStrategy = new CellMoveStrategyBase(_camera);
          if (UseSound)
            _audioSource = GetComponent<AudioSource>();

          EnableMove = true;
        }
        
        public List<CellView> GetGroup(int idx) => _groups[idx];    
    
        public void Build(Level level)
        {
            PrepareGroups(level);
            BuildCells(level);
            UpdateDirections();
        }
        
        private void PrepareGroups(Level level)
        {
            _groups = new List<List<CellView>>();
            for (var i = 0; i < level.Groups.Length; i++) 
                _groups.Add(new List<CellView>());
        }

        private void BuildCells(Level level)
        {
            _field = new Cell[level.Size.X, level.Size.Y];
            for (var i = 0; i < _field.GetLength(0); i++)
            {
                for (var j = 0; j < _field.GetLength(1); j++)
                {
                    _field[i, j] = new Cell(Directions.None, level.IsEmptyPosition(i, j));
                    if (_field[i, j].IsFree)
                        continue;
                    
                    var cellPrefab = level.Cells.First(x => x.Position.X == i && x.Position.Y == j).Prefab;
                    var cellView = Instantiate(cellPrefab, 
                        new Vector3(_cellSize.x * i, -_cellSize.y * j, 0f),
                        Quaternion.Euler(0, 90, 0));
                    cellView.transform.SetParent(transform, false);
                    cellView.Init(new Vector2int(i, j));
                    cellView.OnStartDrag += OnStartDrag;
                    cellView.OnDrag += OnDragCell;
                    cellView.OnDragComplete += OnDragComplete;
                    cellView.OnPositionChanged += CellPositionChanged;

                    var groupIdx = Array.FindIndex(level.Groups, g => g.Positions.Any(p => p.X == i && p.Y == j));
                    if (groupIdx != -1)
                        _groups[groupIdx].Add(cellView);
                }
            }
        }

        private void UpdateDirections()
        {
            Reset();
            UpdateFreeCellNeighbors();
            UpdateGroupNeighbors();
            
            void Reset()
            {
                for (var i = 0; i < _field.GetLength(0); i++)
                {
                    for (var j = 0; j < _field.GetLength(1); j++)
                    {
                        _field[i, j].Direction = Directions.None;
                    }
                }
            }

            void UpdateFreeCellNeighbors()
            {
                for (var i = 0; i < _field.GetLength(0); i++)
                {
                    for (var j = 0; j < _field.GetLength(1); j++)
                    {
                        if (!_field[i, j].IsFree)
                            continue;
                        
                        if (i - 1 >= 0)
                            _field[i - 1, j].Direction |= Directions.Right;
                        if (i + 1 < _field.GetLength(0))
                            _field[i + 1, j].Direction |= Directions.Left;
                        if (j - 1 >= 0)
                            _field[i, j - 1].Direction |= Directions.Down;
                        if (j + 1 < _field.GetLength(1))
                            _field[i, j + 1].Direction |= Directions.Up;
                    }
                }
            }

            void UpdateGroupNeighbors()
            {
                foreach (var group in _groups)
                {
                    foreach (var cell in group)
                    {
                        foreach (var cellCheck in group)
                        {
                            if (cell == cellCheck)
                                continue;

                            if (cell.CellPosition.Y == cellCheck.CellPosition.Y)
                            {
                                if (cell.CellPosition.X - cellCheck.CellPosition.X == 1)
                                    _field[cell.CellPosition.X, cell.CellPosition.Y].Direction |= Directions.Left;
                                if (cell.CellPosition.X - cellCheck.CellPosition.X == -1)
                                    _field[cell.CellPosition.X, cell.CellPosition.Y].Direction |= Directions.Right;
                            }

                            if (cell.CellPosition.X == cellCheck.CellPosition.X)
                            {
                                if (cell.CellPosition.Y - cellCheck.CellPosition.Y == 1)
                                    _field[cell.CellPosition.X, cell.CellPosition.Y].Direction |= Directions.Up;
                                if (cell.CellPosition.Y - cellCheck.CellPosition.Y == -1)
                                    _field[cell.CellPosition.X, cell.CellPosition.Y].Direction |= Directions.Down;
                            }
                        }
                    }
                }
            }
        }

        private void OnStartDrag(CellView view)
        {
          if (UseSound)
            _audioSource.Play();

          if (!EnableMove)
            return;

          _moveStrategy.StartMove(view.transform.position);
        }
        
        private void OnDragCell(CellView view)
        {
            if (!_moveStrategy.TryGetPositionOffsetFor(out var offset))
                return;
            
            var possibleDirection = _field[view.CellPosition.X, view.CellPosition.Y].Direction;
            if (possibleDirection == Directions.None)
                return;
            
            var groupIdx = FindGroup(view);
            if (groupIdx == -1)
            {
                Move(view);
            }
            else if (GroupMovePossible(_groups[groupIdx]))
            {
                foreach (var cellView in _groups[groupIdx])
                    Move(cellView);
            }

            void Move(CellView view)
            {
                _moveLastTime = Time.realtimeSinceStartup;
                view.DragInProgress = true;
                
                var moveTo = view.transform.position + offset;
                var clampXMin = view.WorldPosition.x - (possibleDirection.HasFlag(Directions.Left) ? _cellSize.x : 0);
                var clampXMax = view.WorldPosition.x + (possibleDirection.HasFlag(Directions.Right) ? _cellSize.x : 0);
                var clampYMin = view.WorldPosition.y - (possibleDirection.HasFlag(Directions.Down) ? _cellSize.y : 0);
                var clampYMax = view.WorldPosition.y + (possibleDirection.HasFlag(Directions.Up) ? _cellSize.y : 0);
            
                view.MoveTo = new Vector3(
                    Mathf.Clamp(moveTo.x, clampXMin, clampXMax),
                    Mathf.Clamp(moveTo.y, clampYMin, clampYMax),
                    moveTo.z);
            }
        }

        private bool GroupMovePossible(List<CellView> group)
        {
            var direction = _moveStrategy.Direction.Value.ToDirections();
            return group.All(view => _field[view.CellPosition.X, view.CellPosition.Y].Direction.HasFlag(direction));
        }

        private int FindGroup(CellView view)
        {
            for (var i = 0; i < _groups.Count; i++)
            {
                var idx = _groups[i].IndexOf(view);
                if (idx != -1)
                    return i;
            }

            return -1;
        }

        private void OnDragComplete(CellView view)
        {
            if (Time.realtimeSinceStartup - _moveLastTime < 0.5f)
                _failedMoveCount++;
            if (_failedMoveCount == 3)
            {
                _failedMoveCount = 0;
                //OnSelectFail?.Invoke();
            }

            if(UseSound)
              _audioSource.Stop();
            var groupIdx = FindGroup(view);
            if (groupIdx == -1)
            {
                CompleteMove(view);
            }
            else
            {
                foreach (var cellView in _groups[groupIdx])
                    CompleteMove(cellView);
            }
            
            void CompleteMove(CellView view)
            {
                view.DragInProgress = false;
                
                var possiblePosition = view.WorldPosition + (Vector3)(_moveStrategy.Direction * _cellSize);
                var startPosition = view.WorldPosition;
                var currentPosition = view.transform.position;

                if ((possiblePosition - currentPosition).sqrMagnitude > (startPosition - currentPosition).sqrMagnitude)
                    view.MoveTo = startPosition;
                else
                    view.MoveTo = possiblePosition;
            }
        }

        private void CellPositionChanged(CellView view)
        {
            var direction = _moveStrategy.Direction.Value.ToVector2Int();
            var occupatePosition = new Vector2int(view.CellPosition.X + direction.X, view.CellPosition.Y - direction.Y);
            _field[occupatePosition.X, occupatePosition.Y].IsFree = false;

            view.CellPosition = occupatePosition;
            
            SetFreePosition(view);

            var groupIdx = FindGroup(view);
            if (groupIdx == -1 || _groups[groupIdx].All(v => v.MoveCompleted))
            {
                UpdateDirections();
                OnPostitionChanged?.Invoke();
            }
        }

        private void SetFreePosition(CellView view)
       {
           var direction = _moveStrategy.Direction.Value.ToVector2Int();
           var prevPosition = new Vector2int(view.CellPosition.X - direction.X, view.CellPosition.Y + direction.Y);
           
           var groupIdx = FindGroup(view);
           if (groupIdx == -1)
           {
               _field[prevPosition.X, prevPosition.Y].IsFree = true;
               return;
           }
           
           while (CellExist(prevPosition.X, prevPosition.Y))
           {
               if (!CellExistInGroup(prevPosition, _groups[groupIdx]))
               {
                   _field[prevPosition.X, prevPosition.Y].IsFree = true;
                   return;
               }
               
               prevPosition = new Vector2int(prevPosition.X - direction.X, prevPosition.Y + direction.Y);
           }
       }

       bool CellExist(int x, int y) => 
           x >= 0 && x < _field.GetLength(0) && y >= 0 && y < _field.GetLength(1);
       
       bool CellExistInGroup(Vector2int position, List<CellView> group) => 
           group.Any(c => c.CellPosition == position);
    }
}
