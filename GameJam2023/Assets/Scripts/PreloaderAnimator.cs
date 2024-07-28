using UnityEngine;

namespace Assets.Scripts
{
  public class PreloaderAnimator : MonoBehaviour
  {
    public static PreloaderAnimator Instance;
    private Animator _animator;

    void Awake()
    {
      Instance = this;
      _animator = GetComponentInChildren<Animator>();
    }

    public void Play(string name)
    {
      _animator.SetTrigger(name);
    }

    //  private bool _isVictory;
    //  public void Play(string name, bool isVictory)
    //  {
    //      _isVictory = isVictory;
    //      _animator.SetTrigger(name);
    //  }

    //public void Restart()
    //{
    //  if (_isVictory)
    //  {
    //    var windowsManager = FindObjectOfType<WindowsManager>();
    //    windowsManager.ToNextState();
    //  }
    //  else
    //  {
    //    var windowsManager = FindObjectOfType<WindowsManager>();
    //    windowsManager.ShowEnd();
    //  }
    //}
  }
}