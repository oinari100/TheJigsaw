using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour {
  public Sprite imgDefault;
  public Sprite imgOut;
  SpriteRenderer img;
  public bool defaultFlg = false;
  public static bool outFlg = false;
  public static bool wait = false;

  void Start() {
    img = gameObject.GetComponent<SpriteRenderer>();
    img.sprite = imgDefault;
    outFlg = false;
  }

  void Update() {

    // 画面クリック時と手がデフォルト位置の場合
    if (defaultFlg) {
    //if (defaultFlg) {

      // 画像変更
      img.sprite = imgOut;

      // 変更した画像位置修正
      Transform myTransform = this.transform;
      Vector3 pos = myTransform.position;
      pos.x -= 0.12f;
      myTransform.position = pos;

      outFlg = true;
      defaultFlg = false;
    }
  }

  void FixedUpdate() {
    // 手を離す
    if (outFlg) {
      if (transform.position.y >= 14.3f) {
        // 画像変更
        img.sprite = imgDefault;

        // 変更した画像位置修正
        Transform myTransform = this.transform;
        Vector3 pos = myTransform.position;
        pos.x = 0.578f;
        myTransform.position = pos;
      } else {
        // 手を上に移動
        transform.position += new Vector3(0, 0.05f, 0);
      }
    // 手を下げる
    } else if (StartGame.startFlg) {

      if (wait) {
        return;
      }
      if (transform.position.y >= 8.8f) {
        // 手を下に移動
        transform.position += new Vector3(0, -0.05f, 0);
      } else {
        // 画面クリックをOnにする
        defaultFlg = true;
      }
    }
  }
}