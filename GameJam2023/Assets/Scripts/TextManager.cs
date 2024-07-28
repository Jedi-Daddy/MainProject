using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
  public class TextManager : MonoBehaviour
  {
    [SerializeField] float deleyBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar = "";
    [SerializeField] float deleyBtwSentance = 0.2f;

    public static TextManager Instance;
    private TMP_Text _textMesh;
    private string _currentText;

    private Coroutine _displayCoroutine;

    //public Dictionary<int, string> Level1Text = new Dictionary<int, string>
    //{
    //  { 0, ""},
    //  { 1, ""},
    //  { 2, ""},
    //  { 3, ""},
    //  { 4, ""},
    //  { 5, ""},
    //  { 6, ""},
    //  { 7, ""},
    //  { 8, ""},
    //  { 9, ""}
    //};

    public Dictionary<int, string> Level1Text = new Dictionary<int, string>
    {
      { 0, " - Що Вас турбує? З чим пришли?"},
      { 1, " - О, бачу вас це бентежить"},
      { 2, " - Зробіть глобокий вдох"},
      { 3, " - Подумайте про щось приємне"},
      { 4, ""},
      { 5, ""},
      { 6, ""},
      { 7, ""},
      { 8, ""},
      { 9, ""}
    };

    void Awake()
    {
      Instance = this;
      _textMesh = GetComponent<TMP_Text>();
    }

    public void SetText(int paintingNumber)
    {
      _textMesh.text = string.Empty;
      if (Level1Text.ContainsKey(paintingNumber))
      {
        _currentText = Level1Text[paintingNumber];
        if (_displayCoroutine != null)
        {
          StopCoroutine(_displayCoroutine);
        }
        _displayCoroutine = StartCoroutine("TypeWriter");
      }
      else
      _currentText = string.Empty;
    }

    public IEnumerator TypeWriter()
    {
      _textMesh.text = leadingChar;
      yield return new WaitForSeconds(deleyBeforeStart);
      foreach (char c in _currentText)
      {
        if (_textMesh.text.Length > 0)
        {
          _textMesh.text = _textMesh.text.Substring(0, _textMesh.text.Length - leadingChar.Length);
        }
        _textMesh.text += c;
        _textMesh.text += leadingChar;
        if(NeedAStop(c))
          yield return new WaitForSeconds(deleyBtwSentance);
        else
        yield return new WaitForSeconds(timeBtwChars);
      }

      if (leadingChar != "")
      {
        _textMesh.text = _textMesh.text.Substring(0, _textMesh.text.Length - leadingChar.Length);
      }
    }

    private bool NeedAStop(char c)
    {
      if (c == '-')
        return false;
      if (char.IsLetterOrDigit(c) || c == ' ')
        return false;
      return true;
    }
  }
}
