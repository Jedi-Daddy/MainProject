using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
  public class ButtonScript : MonoBehaviour, IPointerClickHandler
  {
    public Action ActionDelegate;

    public void OnPointerClick(PointerEventData eventData)
    {
      if (ActionDelegate != null)
      {
        ActionDelegate();
      }
    }
  }
}
