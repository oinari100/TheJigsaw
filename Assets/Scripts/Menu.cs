using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

  public static bool open; 

  // メニュー画面（パネル）
  [SerializeField] private GameObject pausePanel;

  // メニュー遷移ボタン
  [SerializeField] private GameObject next, back;

  // ステージNo.、ステージ名
  [SerializeField] private Text stageNum, stageName;

  // 選択するステージ
  private int stageTarget;

  // 音
  public AudioClip button;
  AudioSource audioSource;

  void Start() {
    // メニュー画面非表示
    pausePanel.SetActive(false);

    audioSource = GetComponent<AudioSource>();
  }

  /**
   * メニュー画面クリック時処理
   **/
  public void ClickMenu() {

    // フラグをセット
    open = true;

    // メニュー画面用の値をセット
    stageTarget = GameOperation.stageNow;

    // メニュー画面を現在のステージにセット
    stageNum.text = "#" + stageTarget;
    stageName.text = "- " + GameOperation.stageList[stageTarget] + " -";

    // ゲーム停止
    Time.timeScale = 0;  
    pausePanel.SetActive(true);

    // ステージ1の時は、Back非表示
    if (1 == stageTarget) {
      back.SetActive(false);
    }

    // ステージが最新の時は、Next非表示
    if (GameOperation.stageLatest == stageTarget) {
      next.SetActive(false);
    }
  
    // ステージが最新の時は、Next非表示
    if (GameOperation.stageLast == stageTarget) {
      next.SetActive(false);
    }
  }

  /**
   * 戻るボタンクリック時処理
   **/
  public void ClickBack() {

    stageTarget -= 1;

    // 音
    audioSource.PlayOneShot(button);

    stageNum.text = "#" + stageTarget;
    stageName.text = "- " + GameOperation.stageList[stageTarget] + " -";
    
    // ステージ1の時は、Back非表示
    if (1 == stageTarget) {
      back.SetActive(false);
    }

    // ステージが最新ではない時は、Next表示
    if (GameOperation.stageLatest != stageTarget) {
      next.SetActive(true);
    }
  }

  /**
   * 次ボタンクリック時処理
   **/
  public void ClickNext() {
    back.SetActive(true);

    stageTarget += 1;

    // ステージが最新の時は、Next非表示
    if (GameOperation.stageLast < stageTarget) {
      next.SetActive(false);
      stageTarget -= 1;
      return;
    }

    if (GameOperation.stageLatest == stageTarget) {
      next.SetActive(false);
    }

    // ステージが最新の時は、Next非表示
    if (GameOperation.stageLatest < stageTarget) {
      next.SetActive(false);
      stageTarget -= 1;
      return;
    }

    // 音
    audioSource.PlayOneShot(button);

    stageNum.text = "#" + stageTarget;
    stageName.text = "- " + GameOperation.stageList[stageTarget] + " -";
    
    // ステージ1の時は、Back非表示
    if (1 != stageTarget) {
      back.SetActive(true);
    }
  }

  /**
   * 背景クリック時処理
   **/
  public void ClickBackground() {

    // ゲーム再開
    Time.timeScale = 1;
    pausePanel.SetActive(false);

    // 別のステージを選択した場合
    if (GameOperation.stageNow != stageTarget) {
      GameOperation.stageNow = stageTarget;
      Hand.wait = false;
      SceneManager.LoadScene("Stage");
    }

    // フラグをセット
    open = false;
  }
  /**
   * メニュー画面クリック時処理
   
  public void Click() {

    // メニュー画面用の値をセット
    stageTarget = GameOperation.stageNow;

    // クリックした画像名を取得
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
Debug.Log(hit2d.transform.gameObject.name);
Debug.Log(stageTarget);

    switch(hit2d.transform.gameObject.name) {
      case "Menu":
        
      break;

      case "Pause":
        // ゲーム再開
        Time.timeScale = 1;  
        pausePanel.SetActive(false);
      break;

      case "Next":
        back.SetActive(true);

        stageTarget += 1;

        stageNum.text = "#" + stageTarget;
        stageName.text = "- " + GameOperation.stageList[stageTarget] + " -";
      break;

      case "Back":
        if (1 == stageTarget) {
          back.SetActive(false);
        }

        stageTarget -= 1;

        stageNum.text = "#" + stageTarget;
        stageName.text = "- " + GameOperation.stageList[stageTarget] + " -";
      break;
    }
  }*/ 
}