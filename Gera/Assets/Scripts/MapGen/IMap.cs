using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMap
{
    bool CanGo(int x, int y);

    bool IsDead(int x, int y);

    bool IsFinish(int x, int y);
}