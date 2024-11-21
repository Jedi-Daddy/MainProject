using System.Linq;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    private Player[] _players;

    void Start()
    {
        _players = transform.parent.GetComponentsInChildren<Player>().Where(x => x.isActiveAndEnabled).ToArray();
    }

    void Update()
    {
#if !UNITY_ANDROID || UNITY_EDITOR
        var horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            var distance = new Vector2(horizontal > 0 ? 1 : -1, 0);
            if (_players.Any(x => !x.CanGo(distance)))
                return;

            foreach (var player in _players)
                player.Move(distance);
        }

        var vertical = Input.GetAxis("Vertical");
        if (vertical != 0)
        {
            var distance = new Vector2(0, vertical > 0 ? 1 : -1);
            if (_players.Any(x => !x.CanGo(distance)))
                return;

            foreach (var player in _players)
                player.Move(distance);
        }

#endif
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundSystem.Instance.Stop();
            PreloaderAnimator.Instance.Play("Game_Over");
        }
    }
}
