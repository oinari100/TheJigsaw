using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NatSuite.Sharing;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour {

  // 生成された完成図
  [SerializeField] private GameObject resultPeace;

  // 対象の完成図
  [SerializeField] private GameObject targetPeace;

  // 完成図
  [SerializeField] private GameObject result1, result2, result3, result4, result5, result6, result7, result8, result9, result10,
    result11, result12, result13, result14, result15, result16, result17, result18, result19, result20,
    result21, result22, result23, result24, result25, result26, result27, result28, result29, result30,
    result31, result32, result33, result34, result35, result36, result37, result38, result39, result40,
    result41, result42, result43, result44, result45, result46, result47, result48, result49, result50;

  // ステージNo.、ステージ名、写真家
  [SerializeField] private Text stageNum, stageName, photographer;

  // 画面エフェクト
  [SerializeField] private Image effect;

  // 音
  public AudioClip complete;
  AudioSource audioSource;
  
  void Start() {

    effect.color = new Color(1, 1, 1, 1);

    // 現在のステージを取得（描写のキーとなる）
    int stage = GameOperation.stageNow;
    switch(stage) {
      case 1:
        targetPeace = result1;
        break; 
   
      case 2:
        targetPeace = result2;
        break;
   
      case 3:
        targetPeace = result3;
        break;
   
      case 4:
        targetPeace = result4;
        break;
   
      case 5:
        targetPeace = result5;
        break;
   
      case 6:
        targetPeace = result6;
        break;

      case 7:
        targetPeace = result7;
        break;

      case 8:
        targetPeace = result8;
        break;

      case 9:
        targetPeace = result9;
        break;

      case 10:
        targetPeace = result10;
        break;

      case 11:
        targetPeace = result11;
        break;

      case 12:
        targetPeace = result12;
        break;

      case 13:
        targetPeace = result13;
        break;

      case 14:
        targetPeace = result14;
        break;

      case 15:
        targetPeace = result15;
        break;

      case 16:
        targetPeace = result16;
        break;

      case 17:
        targetPeace = result17;
        break;

      case 18:
        targetPeace = result18;
        break;

      case 19:
        targetPeace = result19;
        break;

      case 20:
        targetPeace = result20;
        break;

      case 21:
        targetPeace = result21;
        break;

      case 22:
        targetPeace = result22;
        break;

      case 23:
        targetPeace = result23;
        break;

      case 24:
        targetPeace = result24;
        break;

      case 25:
        targetPeace = result25;
        break;

      case 26:
        targetPeace = result26;
        break;

      case 27:
        targetPeace = result27;
        break;

      case 28:
        targetPeace = result28;
        break;

      case 29:
        targetPeace = result29;
        break;

      case 30:
        targetPeace = result30;
        break;

      case 31:
        targetPeace = result31;
        break;

      case 32:
        targetPeace = result32;
        break;

      case 33:
        targetPeace = result33;
        break;

      case 34:
        targetPeace = result34;
        break;

      case 35:
        targetPeace = result35;
        break;
    }
    // パズル全体を描写
    resultPeace = Instantiate(targetPeace, new Vector3(0, 1, 0), Quaternion.identity);
    
    // ステージ数セット
    stageNum.text = "#" + stage;
    
    // ステージ名セット
    stageName.text = "- " + GameOperation.stageList[stage] + " -";
    
    // 写真家の名前をセット
    photographer.text = "photo by " + GameOperation.photographerList[stage];

    // 音を流す
    audioSource = GetComponent<AudioSource>();
    audioSource.PlayOneShot(complete);

    // アプリ評価を促す
    if (GameOperation.stageLatest == 11 && GameOperation.stageNow == 10) {
      #if UNITY_IOS
        UnityEngine.iOS.Device.RequestStoreReview();
      #endif
    }
  }

  void Update() {
    // 遷移時のエフェクト
    effect.color = Color.Lerp(effect.color, new Color(1, 1, 1, 0), Time.deltaTime * 3f);
  }
  
  // 次ボタン押下時
  public void ClickNext() {
    if (GameOperation.stageNow != GameOperation.stageLast) {
      GameOperation.stageNow += 1;
    }
    SceneManager.LoadScene("Stage");
  }

  // シェアボタン押下時
  public void ClickShare() {
    //  SNSへのシェア
    var payload = new SharePayload();
    payload.AddText("The Jigsaw" + " " + "#" + GameOperation.stageNow + " " + "- " + GameOperation.stageList[GameOperation.stageNow] + " -" + " completed!");
    Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
    payload.AddImage(tex);
    var success = payload.Commit();
  }
}
