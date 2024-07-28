using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.UI;
using Assets.Scripts.Vovkulaka;
using Game.Field;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        private const string PathTemplate = "Level_";

        public event Action OnWinning;
        public event Action OnFailInGame;

        private string CurLevelName => PathTemplate + (_curLevelIdx + 1);
        private Level CurLevel => _levels[_curLevelIdx];
        
        [SerializeField] private Level[] _levels;
        [SerializeField] private GameObject[] _menuObjectsToHide;

        private int _curLevelIdx;
        private FieldView _curGameField;

        private bool levelIsLoaded;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void StartLevel()
        {
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            if (levelIsLoaded)
            {
                SceneManager.UnloadSceneAsync(CurLevelName);
                Timer.Instance.OnAnimationChange -= VovkulakaAnimation.Instance.PlayNextAnimation;
            }
            var asyncLoad = SceneManager.LoadSceneAsync(CurLevelName, LoadSceneMode.Additive);
            
            Debug.Log("Loading progress:");
            while (!asyncLoad.isDone)
            {
                Debug.Log($"Loading progress: {asyncLoad.progress}");
                yield return null;
            }
            Debug.Log("Loading complete.");

            PrepareScene();
            BuildLevel();
            levelIsLoaded = true;

          Timer.Instance.OnAnimationChange += VovkulakaAnimation.Instance.PlayNextAnimation;
    }

        private void PrepareScene()
        {
            foreach (var menuObject in _menuObjectsToHide)
                menuObject.SetActive(false);
        }

        private void BuildLevel()
        {
            if (!SceneManager.GetSceneByName(CurLevelName)
                    .GetRootGameObjects()
                    .Any(go => go.TryGetComponent(out _curGameField)))
            {
                return;
            }

            _curGameField.Build(CurLevel);
            _curGameField.OnSelectFail += FailedSelection;
            _curGameField.OnPostitionChanged += CheckWin;
        }

    public void EnableMove()
    {
      _curGameField.EnableMove = true;
    }

    public void DisableMove()
    {
      _curGameField.EnableMove = false;
    }

    private void CheckWin()
    {
        var cells = _curGameField.GetGroup(CurLevel.Condition.GroupIdx);
        foreach (var position in CurLevel.Condition.Positions)
        {
            if (cells.All(c => c.CellPosition != position))
                return;
        }
      //Debug.Log("win GC");
        OnWinning?.Invoke();
    }

    private void FailedSelection()
    {
        OnFailInGame?.Invoke();
    }
  }
  

    [Serializable]
    public class Level
    {
        public Vector2int Size;
        public LevelCell[] Cells;
        public Group[] Groups;
        public Vector2int[] EmptyPositions;
        public WinCondition Condition;

        public bool IsEmptyPosition(int x, int y) => EmptyPositions.Any(i => i.X == x && i.Y == y);
    }

    [Serializable]
    public class LevelCell
    {
        public Vector2int Position;
        public CellView Prefab;
    }

    [Serializable]
    public struct WinCondition
    {
        public int GroupIdx;
        public Vector2int[] Positions;
    }
    
    [Serializable]
    public struct Group
    {
        public Vector2int[] Positions;
    }

    [Serializable]
    public struct Vector2int
    {
        public int X;
        public int Y;

        public Vector2int(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static Vector2int operator +(Vector2int a, Vector2int b) => new (a.X + b.X, a.Y + b.Y);
        public static Vector2int operator -(Vector2int a, Vector2int b) => new (a.X - b.X, a.Y - b.Y);
        public static bool operator ==(Vector2int a, Vector2int b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2int a, Vector2int b) => a.X != b.X || a.Y != b.Y;
    }
}
