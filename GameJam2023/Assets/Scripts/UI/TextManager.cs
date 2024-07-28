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
    private AudioSource _audioSource;
    private string _currentText;

    private Coroutine _displayCoroutine;

    public Dictionary<int, string> Level1Text = new Dictionary<int, string>
    {
      { 0, " - Доброго дня. Що Вас турбує? З чим завiтали?"},
      { 1, " - Бачу Вас це бентежить."},
      { 2, " - Зробіть глибокий вдох."},
      { 3, " - Ви впевненi що саме це було вашим тригером?"},
      { 4, " - Що Ви відчуваєте, коли так дивитися на ситуацію?"},
      { 5, " - Чого боїтесь? Розкажіть, як саме Ви боїтесь?"},
      { 6, " - Розкажіть, як саме Ви боїтесь?"},
      { 7, " - Наскільки сильний страх, якщо 0 зовсім не страшно, 10 максимально страшно?"},
      { 8, " - Ви хочете контролювати те, чого не можете аби не відчувати тривогу?"},
      { 9, " - Ви відчуваєте зовнішню провину чи внутрішню?"},
      { 10, " - Як Ви інтерпретуєте для себе те, що сталося?"},
      { 11, " - Чому Ви згадали саме цей приклад?"},
      { 12, " - Як би Ви мали змогу щось змiнити у той час, щоб Ви зробили?"},
      { 13, " - Зробiть глибокий вдох i дорахуйте до десяти."},
      { 14, " - Помiркуйте про щось приємне."},
      { 15, " - Я впевнений, що разом ми знайдемо вихiд з цієї ситуації."},
      { 16, " - Я впевнений, що разом ми знайдемо вихiд з цієї ситуації."},
      { 17, " - Зараз я попрошу Вас вигадати метафору для цієї ситуації."},
      { 18, " - Якщо Ви чекаєте, що я відповім на запитання за Вас, то це не так."},
      { 19, " - Іноді час лікує психологічні травми."},
      { 20, " - Дякую що так довіряєте мені."},
      { 21, " - Зосередся, у тебе вийде."},
    };

    private int currentTextNumber = 0;

    void Awake()
    {
      Instance = this;
      _textMesh = GetComponent<TMP_Text>();
      _audioSource = GetComponent<AudioSource>();
    }

    public void PrintNextText()
    {
      currentTextNumber++;
      SetText(currentTextNumber);
    }

    public void SetText(int paintingNumber)
    {
      _textMesh.text = string.Empty;
      if (Level1Text.ContainsKey(paintingNumber))
      {
        _currentText = Level1Text[paintingNumber];
        currentTextNumber = paintingNumber;
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
        _audioSource.Play();
        _textMesh.text += c;
        _textMesh.text += leadingChar;
        if (NeedAStop(c))
          yield return new WaitForSeconds(deleyBtwSentance);
        else
          yield return new WaitForSeconds(timeBtwChars);
      }

      _audioSource.Stop();
      if (leadingChar != "")
      {
        _textMesh.text = _textMesh.text.Substring(0, _textMesh.text.Length - leadingChar.Length);
      }
    }

    public void ResetText()
    {
      SetText(21);
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
