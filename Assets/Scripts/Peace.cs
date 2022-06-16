using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 各パズルに落下操作を加える
 */
public class Peace : MonoBehaviour {

  public bool createFlg = true;
  Rigidbody2D rd;

  void Start() {
  }

  void Update() {
    if (transform.position.y <= 5f && createFlg && JigsawPuzzle.breakFlg) {
        
      rd = gameObject.AddComponent<Rigidbody2D>();
      rd = gameObject.GetComponent<Rigidbody2D>();

      float up = Random.Range (-300.0f, 300.0f);
      float right = Random.Range (-400.0f, 400.0f);
      float angular = Random.Range (-400.0f, 400.0f);
          
      rd.AddForce(transform.up * up);
      rd.AddForce(transform.right * right);
      rd.angularVelocity = angular;

      createFlg = false;
    }

    if (transform.position.y <= -13f) {
      Destroy(this.gameObject);
    }
  }
}