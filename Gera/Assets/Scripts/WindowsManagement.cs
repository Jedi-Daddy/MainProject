using UnityEngine;
using UnityEngine.Video;
using Assets.Scripts;
using UnityEngine.UI;

public class WindowsManagement : MonoBehaviour
{
  public GameObject Level1;
  public GameObject Level2;
  public GameObject Level3;
  public GameObject Level4;
  public GameObject Level5;
  public GameObject Intro;
  public GameObject Loading;

  public GameObject MenuRestart;
  public GameObject MenuNext;


  public GameObject GameEndWin;
  public GameObject EndLevel1;
  public GameObject EndLevel2;
  public GameObject EndLevel3;

  public GameObject BeforeLevel2;
  public GameObject BeforeLevel3;
  public GameObject BeforeLevel4;
  public GameObject BeforeLevel5;

  public GameObject Logo;


  private int currentLevel = 0;

  void Start()
    {
    MenuRestart.GetComponentInChildren<ButtonScript>().ActionDelegate += ShowRestart;
    MenuRestart.GetComponentInChildren<ButtonScriptExit>().ActionDelegate += ExitGame;
    MenuNext.GetComponentInChildren<ButtonScript>().ActionDelegate += ShowRestart;
    MenuNext.GetComponentInChildren<ButtonScriptExit>().ActionDelegate += ExitGame;
    MenuNext.GetComponentInChildren<ButtonScriptNext>().ActionDelegate += NextLevel;

    Intro.active = true;
    Level1.active = false;
    Level2.active = false;
    Level3.active = false;
    Level4.active = false;
    Level5.active = false;
    Loading.active = false;
    MenuRestart.active = false;
    MenuNext.active = false;
    GameEndWin.active = false;
    EndLevel1.active = false;
    EndLevel2.active = false;
    EndLevel3.active = false;
    Logo.active = false;
    var introVideo = Intro.GetComponentInChildren<VideoPlayer>();
    introVideo.Play();
    introVideo.loopPointReached += ShowLogo;
  }
  public void ShowLogo(VideoPlayer vp)
  {
    vp.Stop();
    Intro.active = false;
    Logo.active = true;
    var loadingVideo = Logo.GetComponentInChildren<VideoPlayer>();
    var texture = new RenderTexture(1920, 1080, 1);
    loadingVideo.targetTexture = texture;
    loadingVideo.transform.parent.GetComponentInChildren<RawImage>().texture = texture;

    loadingVideo.Play();
    loadingVideo.loopPointReached += ShowLoading1;
  }

  public void ShowLoading1(VideoPlayer vp)
  {
    vp.Stop();
    Intro.active = false;
    Level1.active = false;
    Level2.active = false;
    Level3.active = false;
    Level4.active = false;
    Level5.active = false;
    Loading.active = true;
    Logo.active = false;
    var loadingVideo = Loading.GetComponentInChildren<VideoPlayer>();
    var texture = new RenderTexture(1920, 1080, 1);
    loadingVideo.targetTexture = texture;
    loadingVideo.transform.parent.GetComponentInChildren<RawImage>().texture = texture;

    loadingVideo.Play();
    loadingVideo.loopPointReached += StartLev1;
  }
  public void ShowIntroLevel2()
  {
    MenuNext.active = false;
    BeforeLevel2.active = true;
    var loadingVideo = BeforeLevel2.GetComponentInChildren<VideoPlayer>();
        var texture = new RenderTexture(1920, 1080, 1);
        loadingVideo.targetTexture = texture;
        loadingVideo.transform.parent.GetComponentInChildren<RawImage>().texture = texture;
        loadingVideo.Play();
    loadingVideo.loopPointReached += StartLev2;
  }
  public void ShowIntroLevel3()
  {
    MenuNext.active = false;
    BeforeLevel3.active = true;
    var loadingVideo = BeforeLevel3.GetComponentInChildren<VideoPlayer>();
        var texture = new RenderTexture(1920, 1080, 1);
        loadingVideo.targetTexture = texture;
        loadingVideo.transform.parent.GetComponentInChildren<RawImage>().texture = texture;
        loadingVideo.Play();
    loadingVideo.loopPointReached += StartLev3;
  }
  public void ShowIntroLevel4()
  {
    MenuNext.active = false;
    BeforeLevel4.active = true;
    var loadingVideo = BeforeLevel4.GetComponentInChildren<VideoPlayer>();
        var texture = new RenderTexture(1920, 1080, 1);
        loadingVideo.targetTexture = texture;
        loadingVideo.transform.parent.GetComponentInChildren<RawImage>().texture = texture;
        loadingVideo.Play();
    loadingVideo.loopPointReached += StartLev4;
  }
  public void ShowIntroLevel5()
  {
    MenuNext.active = false;
    BeforeLevel5.active = true;
    var loadingVideo = BeforeLevel5.GetComponentInChildren<VideoPlayer>();
        var texture = new RenderTexture(1920, 1080, 1);
        loadingVideo.targetTexture = texture;
        loadingVideo.transform.parent.GetComponentInChildren<RawImage>().texture = texture;
        loadingVideo.Play();
    loadingVideo.loopPointReached += StartLev5;
  }
  
  public void ShowEnd()
  {
    Level1.active = false;
    Level2.active = false;
    Level3.active = false;
    Level4.active = false;
    Level5.active = false;
    MenuRestart.active = true;
  }
  public void ShowRestart()
  {
    MenuRestart.active = false;
    MenuNext.active = false;
    switch (currentLevel)
    {
      case 1:
        var map1 = Level1.GetComponentInChildren<Map1>();
        map1.SetStartPosition();
        StartLev1();
        return;
      case 2:
        var map21 = Level2.GetComponentInChildren<Map2>();
        map21.SetStartPosition();
        var map22 = Level2.GetComponentInChildren<Map2_Invert>();
        map22.SetStartPosition();
        ShowIntroLevel2();
        return;
      case 3:
        var map31 = Level3.GetComponentInChildren<Map3>();
        map31.SetStartPosition();
        var map32 = Level3.GetComponentInChildren<Map3_Invert2>();
        map32.SetStartPosition();
        ShowIntroLevel3();
        return;
      case 4:
        var map41 = Level4.GetComponentInChildren<Map4>();
        map41.SetStartPosition();
        var map42 = Level4.GetComponentInChildren<Map4_Invert>();
        map42.SetStartPosition();
        ShowIntroLevel4();
        return;
      case 5:
        var map5_Human = Level5.GetComponentInChildren<Map5_Human>();
        map5_Human.SetStartPosition();
        var map5_Marsian = Level5.GetComponentInChildren<Map5_Marsian>();
        map5_Marsian.SetStartPosition();
        var map5_Barsuk = Level5.GetComponentInChildren<Map5_Barsuk>();
        map5_Barsuk.SetStartPosition();
        var map5_Jam = Level5.GetComponentInChildren<Map5_Jam>();
        map5_Jam.SetStartPosition();
        ShowIntroLevel5();
        return;
    }
  }

  public void NextLevel()
  {
    switch (currentLevel)
    {
      case 1:
        ShowIntroLevel2();
        return;
      case 2:
        ShowIntroLevel3();
        return;
      case 3:
        ShowIntroLevel4();
        return;
      case 4:
        ShowIntroLevel5();
        return;
      case 5:
        ShowEnd();
        return;
    }
  }

  public void ExitGame()
  {
    Application.Quit();
  }

  public void StartLev1(VideoPlayer vp)
  {
    vp.Stop();
    MenuRestart.active = false;
    MenuNext.active = false;
    Intro.active = false;
    Level1.active = true;
    Level2.active = false;
    Level3.active = false;
    Level4.active = false;
    Level5.active = false;
    Loading.active = false;
    currentLevel = 1;

    SoundSystem.Instance.Play("Main Theme");
    PreloaderAnimator.Instance.Play("Start_Level");
  }
  public void StartLev1()
  {
    MenuRestart.active = false;
    MenuNext.active = false;
    Intro.active = false;
    Level1.active = true;
    Level2.active = false;
    Level3.active = false;
    Level4.active = false;
    Level5.active = false;
    Loading.active = false;
    currentLevel = 1;

    SoundSystem.Instance.Play("Main Theme");
    PreloaderAnimator.Instance.Play("Start_Level");
  }

  //public void StartLev2()
  //{
  //  MenuRestart.active = false;
  //  MenuNext.active = false;
  //  Intro.active = false;
  //  Level1.active = false;
  //  Level2.active = true;
  //  Level3.active = false;
  //  Level4.active = false;
  //  Loading.active = false;
  //  currentLevel = 2;

  //      SoundSystem.Instance.Play("Main Theme");
  //      PreloaderAnimator.Instance.Play("Start_Level");
  //  }

  public void StartLev2(VideoPlayer vp)
  {
    vp.Stop();
    BeforeLevel2.active = false;
    MenuRestart.active = false;
    MenuNext.active = false;
    Intro.active = false;
    Level1.active = false;
    Level2.active = true;
    Level3.active = false;
    Level4.active = false;
    Level5.active = false;
    Loading.active = false;
    currentLevel = 2;

    SoundSystem.Instance.Play("Main Theme");
    //PreloaderAnimator.Instance.Play("Start_Level");
  }

  //public void StartLev3()
  //{
  //  MenuRestart.active = false;
  //  MenuNext.active = false;
  //  Intro.active = false;
  //  Level1.active = false;
  //  Level2.active = false;
  //  Level3.active = true;
  //  Level4.active = false;
  //  Loading.active = false;
  //  currentLevel = 3;

  //      SoundSystem.Instance.Play("Main Theme");
  //      PreloaderAnimator.Instance.Play("Start_Level");
  //  }
  public void StartLev3(VideoPlayer vp)
  {
    vp.Stop();
    BeforeLevel3.active = false;
    MenuRestart.active = false;
    MenuNext.active = false;
    Intro.active = false;
    Level1.active = false;
    Level2.active = false;
    Level3.active = true;
    Level4.active = false;
    Level5.active = false;
    Loading.active = false;
    currentLevel = 3;

    SoundSystem.Instance.Play("Main Theme");
  }

  //public void StartLev4()
  //{
  //  MenuRestart.active = false;
  //  MenuNext.active = false;
  //  Intro.active = false;
  //  Level1.active = false;
  //  Level2.active = false;
  //  Level3.active = false;
  //  Level4.active = true;
  //  Loading.active = false;
  //  currentLevel = 4;

  //      SoundSystem.Instance.Play("Main Theme");
  //      PreloaderAnimator.Instance.Play("Start_Level");
  //  }
  public void StartLev4(VideoPlayer vp)
  {
    vp.Stop();
    BeforeLevel4.active = false;
    MenuRestart.active = false;
    MenuNext.active = false;
    Intro.active = false;
    Level1.active = false;
    Level2.active = false;
    Level3.active = false;
    Level4.active = true;
    Level5.active = false;
    Loading.active = false;
    currentLevel = 4;

    SoundSystem.Instance.Play("Main Theme");
  }
  public void StartLev5(VideoPlayer vp)
  {
    vp.Stop();
    GameEndWin.active = false;
    BeforeLevel5.active = false;
    MenuRestart.active = false;
    MenuNext.active = false;
    Intro.active = false;
    Level1.active = false;
    Level2.active = false;
    Level3.active = false;
    Level4.active = false;
    Level5.active = true;
    Loading.active = false;
    currentLevel = 5;

    SoundSystem.Instance.Play("Main Theme");
  }

  public void ToNextLevel()
  {
    switch (currentLevel)
    {
      case 1:
        Level1Complete();
        return;
      case 2:
        Level2Complete();
        return;
      case 3:
        Level3Complete();
        return;
      case 4:
        Level4Complete();
        return;
      case 5:
        GameEndWinShow();
        return;
    }
  }

  public void GameEndWinShow()
  {
        Level5.active = false;
        GameEndWin.active = true;
    var video = GameEndWin.GetComponentInChildren<VideoPlayer>();
        var texture = new RenderTexture(1920, 1080, 1);
        video.targetTexture = texture;
        video.transform.parent.GetComponentInChildren<RawImage>().texture = texture;
        video.Play();
  }

  public void Level1Complete()
  {
    Level1.active = false;
    EndLevel1.active = true;
    var video = EndLevel1.GetComponentInChildren<VideoPlayer>();

        var texture = new RenderTexture(1920, 1080, 1);
        video.targetTexture = texture;
        video.transform.parent.GetComponentInChildren<RawImage>().texture = texture;

        video.Play();
    video.loopPointReached += ShowMenu;
  }

  public void ShowMenu(VideoPlayer vp)
  {
    vp.Stop();
    MenuNext.active = true;
    EndLevel1.active = false;
    EndLevel2.active = false;
    EndLevel3.active = false;
    GameEndWin.active = false;
    PreloaderAnimator.Instance.Play("Start_Level");
  }
  public void Level2Complete()
  {
    Level2.active = false;
    EndLevel2.active = true;
    var video = EndLevel2.GetComponentInChildren<VideoPlayer>();

        var texture = new RenderTexture(1920, 1080, 1);
        video.targetTexture = texture;
        video.transform.parent.GetComponentInChildren<RawImage>().texture = texture;
        video.Play();
    video.loopPointReached += ShowMenu;
  }
  public void Level3Complete()
  {
    Level3.active = false;
    EndLevel3.active = true;
    var video = EndLevel3.GetComponentInChildren<VideoPlayer>();
        var texture = new RenderTexture(1920, 1080, 1);
        video.targetTexture = texture;
        video.transform.parent.GetComponentInChildren<RawImage>().texture = texture;
        video.Play();
    video.loopPointReached += ShowMenu;
  }
  public void Level4Complete()
    {
        Level4.active = false;
        GameEndWin.active = true;
        var video = GameEndWin.GetComponentInChildren<VideoPlayer>();
        var texture = new RenderTexture(1920, 1080, 1);
        video.targetTexture = texture;
        video.transform.parent.GetComponentInChildren<RawImage>().texture = texture;
        video.Play();
        video.loopPointReached += ShowMenu;
    }
}
