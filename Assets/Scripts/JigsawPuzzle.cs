using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class JigsawPuzzle : MonoBehaviour {
  public static bool breakFlg;
  public static bool moveFlg;
  public static bool rotateFlg;
  public static bool correctFlg;
  private bool stopFlg;
  private bool separateFlg = true;
  public Rigidbody2D rb;
  private Vector3 touchStartPos;
  private Vector3 touchEndPos;
  List<string> notTouch = new List<string> { "Next", "Back", "Pause", "PauseFront", "Menu" };
  const float downSpeedo = -3;
  const float power = 20;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    Application.targetFrameRate = 90;
  }

  void Update() {

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

    if (stopFlg) {
      return;
    }

    if (Menu.open) {
      return;
    }
/*
    // ボタンクリックと区別
#if UNITY_EDITOR
    if(EventSystem.current.IsPointerOverGameObject()){
        return;
    }
#else 
    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
        return;
    }
#endif
*/
    // 手から離れた時
    if (Hand.outFlg && !stopFlg) {

      // クリック時（押下）
      if (Input.GetKeyDown(KeyCode.Mouse0)){
        touchStartPos = new Vector3(Input.mousePosition.x ,Input.mousePosition.y ,Input.mousePosition.z);
      }

      // クリック時（離れた）
      if (Input.GetKeyUp(KeyCode.Mouse0)){
        touchEndPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        if (separateFlg) {
          GetDirection();
        } else {
          separateFlg = true;
        }
      }
    }
  }

  IEnumerator RotateObject() {
    rotateFlg = true;
    var wait = new WaitForSeconds(0.0001f);
    for (int num = 0; num < 18; num++) {
      transform.Rotate(0,0,5);
      yield return wait;
    }
  }

  IEnumerator MoveObject(float num) {
    moveFlg = true;
    var wait = new WaitForSeconds(0.0001f);
    for (int turn=0; turn < 5; turn++) {
      transform.position += new Vector3(num, 0, 0);
      yield return wait;
    }
  }

  void FixedUpdate() { 
    
    if (StartGame.startFlg) {
      if (transform.position.y >= 6.48f) {//GameOperation.stopY ) {
        transform.position += new Vector3(0, -0.05f, 0);

        //rb.AddForce(Vector2.up * ((downSpeedo - rb.velocity.y) * power));
      }

      if (Hand.outFlg && !stopFlg) {
        //rb.AddForce(Vector2.up * ((downSpeedo - rb.velocity.y) * power));
        transform.position += new Vector3(0, -0.05f, 0);
      
        if (transform.position.y <= -13.5f) {
          if (gameObject != null) {
            //Destroy(gameObject);
          }
        }
      }
    }
  }

  void GetDirection(){
    float directionX = touchEndPos.x - touchStartPos.x;
    float directionY = touchEndPos.y - touchStartPos.y;

    if (Mathf.Abs(directionY) < Mathf.Abs(directionX)){
      if (30 < directionX){
        //右向きにフリック -0.1f
        StartCoroutine(MoveObject(0.2f));
      }else if (-30 > directionX){
        //左向きにフリック
        StartCoroutine(MoveObject(-0.2f));
      }
    }else{
      //タッチを検出
      StartCoroutine("RotateObject");
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {   
    
    if (Hand.wait) {
      return;
    }
    
    separateFlg = false;
    
    if (collision.gameObject.name == "Break") {
      // バラバラにする
      breakFlg = true;
      return;
    }

    // 角度取得
    float z = transform.root.gameObject.transform.localEulerAngles.z;
    
    // 位置取得
    float x = transform.position.x;

    switch(transform.name) {

      /*
       * ステージ1
       */
      case "Piece1_1(Clone)":
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece1_2(Clone)":
        if(269f < z && z < 271f && -0.9f > x && x > -1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ2
       */
      case "Piece2_1(Clone)":
        if(179f < z && z < 181f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece2_2(Clone)":
        if(((359f < z && z < 361f ) || (-1f < z && z < 1f)) && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ3
       */
      case "Piece3_1(Clone)":
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece3_2(Clone)":
        if(89f < z && z < 91f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ4
       */
      case "Piece4_1(Clone)":
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece4_2(Clone)":
        if(89f < z && z < 91f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ5
       */
      case "Piece5_1(Clone)":
        if (collision.gameObject.name == "Piece5_2(Clone)") {return;}
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece5_2(Clone)":
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ6
       */
      case "Piece6_1(Clone)":
        if (collision.gameObject.name == "Piece6_2(Clone)") {return;}
        if (collision.gameObject.name == "Piece6_3(Clone)") {return;}
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece6_2(Clone)":
        if (collision.gameObject.name == "Piece6_3(Clone)") {return;}
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece6_3(Clone)":
        if(89f < z && z < 91f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ7
       */
      case "Piece7_1(Clone)":
        if (collision.gameObject.name == "Piece7_2(Clone)") {return;}
        if (collision.gameObject.name == "Piece7_3(Clone)") {return;}
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece7_2(Clone)":
        if (collision.gameObject.name == "Piece7_3(Clone)") {return;}
        if(89f < z && z < 91f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece7_3(Clone)":
        if (collision.gameObject.name == "Piece7_2(Clone)") {return;}
        if(89f < z && z < 91f && 2.9f < x && x < 3.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ8
       */
      case "Piece8_1(Clone)":
        if (collision.gameObject.name == "Piece8_2(Clone)") {return;}
        if (collision.gameObject.name == "Piece8_3(Clone)") {return;}
        if(269f < z && z < 271f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece8_2(Clone)":
        if (collision.gameObject.name == "Piece8_3(Clone)") {return;}
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece8_3(Clone)":
        if(89f < z && z < 91f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ9
       */
      case "Piece9_1(Clone)":
        if (collision.gameObject.name == "Piece9_2(Clone)") {return;}
        if (collision.gameObject.name == "Piece9_3(Clone)") {return;}
        if(179f < z && z < 181f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece9_2(Clone)":
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece9_3(Clone)":
        if(-1f < z && z < 1f && -3.1f < x && x < -2.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ10
       */
      case "Piece10_1(Clone)":
        if(179f < z && z < 181f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece10_2(Clone)":
        if(269f < z && z < 271f && 2.9f < x && x < 3.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece10_3(Clone)":
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ11
       */
      case "Piece11_1(Clone)":
        if (collision.gameObject.name == "Piece11_2(Clone)") {return;}
        if (collision.gameObject.name == "Piece11_3(Clone)") {return;}
        if(269f < z && z < 271f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece11_2(Clone)":
        if(179f < z && z < 181f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece11_3(Clone)":
        if(-1f < z && z < 1f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ12
       */
      case "Piece12_1(Clone)":
        if (collision.gameObject.name == "Piece12_2(Clone)") {return;}
        if (collision.gameObject.name == "Piece12_3(Clone)") {return;}
        if(269f < z && z < 271f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece12_2(Clone)":
        if(179f < z && z < 181f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece12_3(Clone)":
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ13
       */
      case "Piece13_1(Clone)":
        if (collision.gameObject.name == "Piece13_2(Clone)") {return;}
        if (collision.gameObject.name == "Piece13_3(Clone)") {return;}
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece13_2(Clone)":
        if(-1f < z && z < 1f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece13_3(Clone)":
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ14
       */
      case "Piece14_1(Clone)":
        if (collision.gameObject.name == "Piece14_3(Clone)") {return;}
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece14_2(Clone)":
        if(269f < z && z < 271f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece14_3(Clone)":
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ15
       */
      case "Piece15_1(Clone)":
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece15_2(Clone)":
        if(179f < z && z < 181f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece15_3(Clone)":
        if(179f < z && z < 181f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ16
       */
      case "Piece16_1(Clone)":
        if(89f < z && z < 91f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece16_2(Clone)":
        if(269f < z && z < 271f && -3.1f < x && x < -2.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece16_3(Clone)":
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ17
       */
      case "Piece17_1(Clone)":
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece17_2(Clone)":
        if(269f < z && z < 271f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece17_3(Clone)":
        if(-1f < z && z < 1f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ18
       */
      case "Piece18_1(Clone)":
        if (collision.gameObject.name == "Piece18_3(Clone)") {return;}
        if(179f < z && z < 181f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece18_2(Clone)":
        if (collision.gameObject.name == "Piece18_3(Clone)") {return;}
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece18_3(Clone)":
        if(269f < z && z < 271f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ19
       */
      case "Piece19_1(Clone)":
        if(89f < z && z < 91f && 2.9f < x && x < 3.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece19_2(Clone)":
        if(179f < z && z < 181f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece19_3(Clone)":
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ20
       */
      case "Piece20_1(Clone)":
        if(179f < z && z < 181f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece20_2(Clone)":
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece20_3(Clone)":
        if(179f < z && z < 181f && -3.1f < x && x < -2.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ21
       */
      case "Piece21_1(Clone)":
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece21_2(Clone)":
        if(269f < z && z < 271f && -3.1f < x && x < -2.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece21_3(Clone)":
        if(269f < z && z < 271f && 2.9f < x && x < 3.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece21_4(Clone)":
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ22
       */
      case "Piece22_1(Clone)":
        if (collision.gameObject.name == "Piece22_3(Clone)") {return;}
        if (collision.gameObject.name == "Piece22_4(Clone)") {return;}
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece22_2(Clone)":
        if (collision.gameObject.name == "Piece22_4(Clone)") {return;}
        if(269f < z && z < 271f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece22_3(Clone)":
        if(269f < z && z < 271f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece22_4(Clone)":
        if(89f < z && z < 91f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ23
       */
      case "Piece23_1(Clone)":
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece23_2(Clone)":
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece23_3(Clone)":
        if(269f < z && z < 271f && -3.1f < x && x < -2.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece23_4(Clone)":
        if(269f < z && z < 271f && 2.9f < x && x < 3.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ24
       */
      case "Piece24_1(Clone)":
        if (collision.gameObject.name == "Piece24_3(Clone)") {return;}
        if(269f < z && z < 271f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece24_2(Clone)":
        if (collision.gameObject.name == "Piece24_4(Clone)") {return;}
        if(179f < z && z < 181f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece24_3(Clone)":
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece24_4(Clone)":
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ25
       */
      case "Piece25_1(Clone)":
        if (collision.gameObject.name == "Piece25_2(Clone)") {return;}
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece25_2(Clone)":
        if (collision.gameObject.name == "Piece25_3(Clone)") {return;}
        if (collision.gameObject.name == "Piece25_4(Clone)") {return;}
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece25_3(Clone)":
        if(179f < z && z < 181f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece25_4(Clone)":
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ26
       */
      case "Piece26_1(Clone)":
        if(269f < z && z < 271f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece26_2(Clone)":
        if(89f < z && z < 91f && 2.9f < x && x < 3.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece26_3(Clone)":
        if(179f < z && z < 181f && -3.1f < x && x < -2.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece26_4(Clone)":
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ27
       */
      case "Piece27_1(Clone)":
        if (collision.gameObject.name == "Piece27_4(Clone)") {return;}
        if(-1f < z && z < 1f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece27_2(Clone)":
        if(269f < z && z < 271f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece27_3(Clone)":
        if(269f < z && z < 271f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece27_4(Clone)":
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ28
       */
      case "Piece28_1(Clone)":
        if (collision.gameObject.name == "Piece28_2(Clone)") {return;}
        if (collision.gameObject.name == "Piece28_4(Clone)") {return;}
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece28_2(Clone)":
        if(((359f < z && z < 361f ) || (-1f < z && z < 1f)) && 2.9f < x && x < 3.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece28_3(Clone)":
        if (collision.gameObject.name == "Piece28_4(Clone)") {return;}
        if(179f < z && z < 181f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece28_4(Clone)":
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ29
       */
      case "Piece29_1(Clone)":
        if (collision.gameObject.name == "Piece29_2(Clone)") {return;}
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece29_2(Clone)":
        if(269f < z && z < 271f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece29_3(Clone)":
        if (collision.gameObject.name == "Piece29_4(Clone)") {return;}
        if(((359f < z && z < 361f ) || (-1f < z && z < 1f)) && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece29_4(Clone)":
        if(179f < z && z < 181f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ30
       */
      case "Piece30_1(Clone)":
        if (collision.gameObject.name == "Piece30_3(Clone)") {return;}
        if(179f < z && z < 181f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece30_2(Clone)":
        if (collision.gameObject.name == "Piece30_4(Clone)") {return;}
        if(((359f < z && z < 361f ) || (-1f < z && z < 1f)) && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece30_3(Clone)":
        if(179f < z && z < 181f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece30_4(Clone)":
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ31
       */
      case "Piece31_1(Clone)":
        if (collision.gameObject.name == "Piece31_2(Clone)") {return;}
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece31_2(Clone)":
        if (collision.gameObject.name == "Piece31_3(Clone)") {return;}
        if (collision.gameObject.name == "Piece31_4(Clone)") {return;}
        if(((359f < z && z < 361f ) || (-1f < z && z < 1f)) && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece31_3(Clone)":
        if(179f < z && z < 181f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece31_4(Clone)":
        if(89f < z && z < 91f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ32
       */
      case "Piece32_1(Clone)":
        if (collision.gameObject.name == "Piece32_4(Clone)") {return;}
        if(89f < z && z < 91f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece32_2(Clone)":
        if (collision.gameObject.name == "Piece32_3(Clone)") {return;}
        if(179f < z && z < 181f && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece32_3(Clone)":
        if(((359f < z && z < 361f ) || (-1f < z && z < 1f)) && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece32_4(Clone)":
        if(269f < z && z < 271f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ33
       */
      case "Piece33_1(Clone)":
        if (collision.gameObject.name == "Piece33_3(Clone)") {return;}
        if(179f < z && z < 181f && -3.1f < x && x < -2.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece33_2(Clone)":
        if (collision.gameObject.name == "Piece33_4(Clone)") {return;}
        if(269f < z && z < 271f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece33_3(Clone)":
        if(89f < z && z < 91f && -1.1f < x && x < -0.9f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece33_4(Clone)":
        if(89f < z && z < 91f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ34
       */
      case "Piece34_1(Clone)":
        if (collision.gameObject.name == "Piece34_2(Clone)") {return;}
        if (collision.gameObject.name == "Piece34_3(Clone)") {return;}
        if(179f < z && z < 181f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece34_2(Clone)":
        if (collision.gameObject.name == "Piece34_3(Clone)") {return;}
        if (collision.gameObject.name == "Piece34_4(Clone)") {return;}
        if(269f < z && z < 271f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece34_3(Clone)":
        if(269f < z && z < 271f && 1.9f < x && x < 2.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece34_4(Clone)":
        if(((359f < z && z < 361f ) || (-1f < z && z < 1f)) && -2.1f < x && x < -1.9f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      /*
       * ステージ35
       */
      case "Piece35_1(Clone)":
        if (collision.gameObject.name == "Piece35_3(Clone)") {return;}
        if(179f < z && z < 181f && 2.9f < x && x < 3.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg1 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece35_2(Clone)":
        if (collision.gameObject.name == "Piece35_4(Clone)") {return;}
        if(179f < z && z < 181f && -0.1f < x && x < 0.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg2 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece35_3(Clone)":
        if(269f < z && z < 271f && 3.9f < x && x < 4.1f) { 
          correctFlg = true;
          stopFlg = true;
          GameOperation.createFlg3 = true;
        } else {
          breakFlg = true;
        }
      break;

      case "Piece35_4(Clone)":
        if(269f < z && z < 271f && 0.9f < x && x < 1.1f) { 
          correctFlg = true;
          stopFlg = true;
          SendResult();
        } else {
          breakFlg = true;
        }
      break;

      default:
        breakFlg = true;
        break;
    }
  }

  void SendResult() {
    // 最新ステージをセット
    if (GameOperation.stageNow == GameOperation.stageLatest) {
      GameOperation.stageLatest += 1;
      GameOperation.Save();
    }

    // 画面遷移（完了画面） 
    Invoke("ChangeScene", 0.5f);
  }

  void ChangeScene() {
    SceneManager.LoadScene("Result");
  }
}
