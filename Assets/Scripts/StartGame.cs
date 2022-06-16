using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {
  public static bool startFlg = false;
  Animator animator;

  void Start() {
    animator = GetComponent<Animator>();
  }
 
  void Update() {
    if (Input.GetMouseButtonDown(0)) {
      animator.SetBool("startFlg", true);
      startFlg = true;
    }
  }
}