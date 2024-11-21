using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
