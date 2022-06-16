using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultClick : MonoBehaviour {
    
  void Start() {
        
  }

  void Update() {
        
  }

  public void ClickNext() {
    // シーン移行
    Invoke("ChangeScene", 1.5f);
  }

  void ChangeScene() {
    // 次のステージを指定する
    GameOperation.stageNow += 1;
    SceneManager.LoadScene("Stage");
  }
}
