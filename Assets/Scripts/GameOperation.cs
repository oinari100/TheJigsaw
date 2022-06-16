using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOperation : MonoBehaviour {

  // 現在のステージ
  public static int stageNow = 1;

  // 一番最新のステージ
  public static int stageLatest = 1;

  // 最終のステージ
  public static int stageLast = 35;

  // ステージ名
  public static List<string> stageList = new List<string>(){ "-", "apple", "cat", "pink cocktail", "hamburger", "ferris wheel", "seaside beach", "avocado", "jellyfish", "white owl", "milk", "heart", "banana", "moon", "cherry blossoms", "donuts", "goldfish", "yacht", "pyramid", "butterfly", "fox", "sunflower", "dog", "snow scene", "aurora", "sushi", "macaron", "whisky", "ice cream", "accoustic guitar", "pizza", "zebra", "grapefruit", "egg", "firework", "bicycle sign" };

  // 写真家 
  public static List<string> photographerList = new List<string>(){ "-", "Sami Ahmed", "Alvan Nee", "Great Cocktails", "Or Hakim", "Wesley Tingey", "Derick McKinney", "Irene Kredenets", "Jank Sandoval", "Simon", "Jagoda Kondratiuk", "elCarito", "Giorgio Trovato", "Thula Na", "TOMOKO UJI", "Lore Schodts", "Cici Hung", "Emma Dau", "Alex Azabache", "Andra C Taylor Jr", "Jonatan Pie", "Mike Marrah", "charlesdeluvio", "Magdalena Mastej", "Johny Goerend", "Mahmoud Fawzy", "Heather Barnes", "Charles \"Duck\" Unitas", "Dovile Ramoskaite", "Annie Spratt", "Shourav Sheikh", "redcharlie", "Amy Shamblen", "Toa Heftiba", "Ray Hennessy", "Takehiro Tomiyama" };  

  // ピース
  [SerializeField] private GameObject piece1_1, piece1_2, piece2_1, piece2_2, piece3_1, piece3_2, 
    piece4_1, piece4_2, piece5_1, piece5_2, piece6_1, piece6_2, piece6_3, piece7_1, piece7_2, piece7_3, 
    piece8_1, piece8_2, piece8_3, piece9_1, piece9_2, piece9_3, piece10_1, piece10_2, piece10_3,
    piece11_1, piece11_2, piece11_3, piece12_1, piece12_2, piece12_3, piece13_1, piece13_2, piece13_3, 
    piece14_1, piece14_2, piece14_3, piece15_1, piece15_2, piece15_3, piece16_1, piece16_2, piece16_3, 
    piece17_1, piece17_2, piece17_3, piece18_1, piece18_2, piece18_3, piece19_1, piece19_2, piece19_3, 
    piece20_1, piece20_2, piece20_3, piece21_1, piece21_2, piece21_3, piece21_4, 
    piece22_1, piece22_2, piece22_3, piece22_4, piece23_1, piece23_2, piece23_3, piece23_4,
    piece24_1, piece24_2, piece24_3, piece24_4, piece25_1, piece25_2, piece25_3, piece25_4,
    piece26_1, piece26_2, piece26_3, piece26_4, piece27_1, piece27_2, piece27_3, piece27_4,
    piece28_1, piece28_2, piece28_3, piece28_4, piece29_1, piece29_2, piece29_3, piece29_4,
    piece30_1, piece30_2, piece30_3, piece30_4, piece31_1, piece31_2, piece31_3, piece31_4,
    piece32_1, piece32_2, piece32_3, piece32_4, piece33_1, piece33_2, piece33_3, piece33_4,
    piece34_1, piece34_2, piece34_3, piece34_4, piece35_1, piece35_2, piece35_3, piece35_4;
  
  // ピース生成する位置
  [SerializeField] private float pieceX = 0f;
  [SerializeField] private float pieceY = 12f;
  
  //[SerializeField] public static float stopY = 6.48f;

  // ステージ
  [SerializeField] private GameObject stage1, stage2, stage3, stage4, stage5, stage6, stage7, stage8, stage9, stage10, 
    stage11, stage12, stage13, stage14, stage15, stage16, stage17, stage18, stage19, stage20, 
    stage21, stage22, stage23, stage24, stage25, stage26, stage27, stage28, stage29, stage30, 
    stage31, stage32, stage33, stage34, stage35, stage36, stage37, stage38, stage39, stage40, 
    stage41, stage42, stage43, stage44, stage45, stage46, stage47, stage48, stage49, stage50; 

  // ステージ生成する位置
  [SerializeField] private float stageX = 0f;
  [SerializeField] private float stageY = 0f;

  // ステージNo.、ステージ名
  [SerializeField] private Text stageNum, stageName, gameTitle;
  
  // 作成するピース、作成したピース
  GameObject targetPiece1, targetPiece2, targetPiece3, targetPiece4, createPiece1, createPiece2, createPiece3, createPiece4;

  // 作成するステージ、作成したステージ
  GameObject targetStage, createStage;

  // ステージ毎のピース作成フラグ
  public static bool createFlg1;
  public static bool createFlg2;
  public static bool createFlg3;

  // 音
  public AudioClip collapse;
  public AudioClip move;
  public AudioClip rotate;
  public AudioClip correct;
  AudioSource audioSource;

  // 広告表示エラー回数
  public static int adErrorCount = 0;

  /**
   * ゲーム起動時処理
   */ 
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void Initialize() {
    // 最新のステージをセット
    stageLatest = PlayerPrefs.GetInt("STAGE", 1);
    stageNow = stageLatest;
    
    if (stageNow > stageLast) {
      stageNow = stageLast;
    }
  }

  /**
   * ゲーム開始時処理
   */ 
  void Start() {

    InitializeValue();
    
    // タイトル設定
    stageNum.text = "#" + stageNow + " |";
    stageName.text = "- " + stageList[stageNow] + " -";

    // ステージ選択
    SelectStage();

    // ピース作成フラグは元に戻す
    createFlg1 = false;
    createFlg2 = false;

    audioSource = GetComponent<AudioSource>();
    
  }

  void Update() { 
    // スタート時にテキストを非表示
    if (StartGame.startFlg) {
      stageNum.enabled = false;
      stageName.enabled = false;
      gameTitle.enabled = false;
    }

    if (JigsawPuzzle.moveFlg) {
      audioSource.PlayOneShot(move);
      JigsawPuzzle.moveFlg = false;
    }
    
    if (JigsawPuzzle.rotateFlg) {
      audioSource.PlayOneShot(rotate);
      JigsawPuzzle.rotateFlg = false;
    }

    if (JigsawPuzzle.correctFlg) {
      audioSource.PlayOneShot(correct);
      JigsawPuzzle.correctFlg = false;
    }
  }

  private IEnumerator CreatePiece() {
    JigsawPuzzle.breakFlg = false;
    createFlg1 = false;
    createFlg2 = false;
    createFlg3 = false;
    Hand.outFlg = false;
    Hand.wait = true;
    audioSource.PlayOneShot(collapse);
    // 1秒間待つ
    yield return new WaitForSeconds(2f);
    if (createPiece1 != null) {Destroy(createPiece1);}
    if (createPiece2 != null) {Destroy(createPiece2);}
    if (createPiece3 != null) {Destroy(createPiece3);}
    if (createPiece4 != null) {Destroy(createPiece4);}
    createPiece1 = Instantiate(targetPiece1, new Vector3(pieceX, pieceY, 0), Quaternion.identity);
    createPiece1.AddComponent<JigsawPuzzle>();
    Hand.wait = false;
  }

  /**
   * ゲーム中処理
   */ 
  void FixedUpdate() { 
    //if (createPiece.transform.position.y <= -13.5f) {
    if (JigsawPuzzle.breakFlg) {
      Handheld.Vibrate();
      adErrorCount += 1;
      StartCoroutine(CreatePiece());
    } else if (createFlg1) {
      InitializeValue();
      switch(stageNow) {
        case 1:
          targetPiece2 = piece1_2;
        break;

        case 2:
          targetPiece2 = piece2_2;
        break;

        case 3:
          targetPiece2 = piece3_2;
        break;

        case 4:
          targetPiece2 = piece4_2;
        break;

        case 5:
          targetPiece2 = piece5_2;
        break;

        case 6:
          targetPiece2 = piece6_2;
        break;

        case 7:
          targetPiece2 = piece7_2;
        break;

        case 8:
          targetPiece2 = piece8_2;
        break;

        case 9:
          targetPiece2 = piece9_2;
        break;

        case 10:
          targetPiece2 = piece10_2;
        break;

        case 11:
          targetPiece2 = piece11_2;
        break;

        case 12:
          targetPiece2 = piece12_2;
        break;

        case 13:
          targetPiece2 = piece13_2;
        break;

        case 14:
          targetPiece2 = piece14_2;
        break;

        case 15:
          targetPiece2 = piece15_2;
        break;

        case 16:
          targetPiece2 = piece16_2;
        break;

        case 17:
          targetPiece2 = piece17_2;
        break;

        case 18:
          targetPiece2 = piece18_2;
        break;

        case 19:
          targetPiece2 = piece19_2;
        break;

        case 20:
          targetPiece2 = piece20_2;
        break;

        case 21:
          targetPiece2 = piece21_2;
        break;

        case 22:
          targetPiece2 = piece22_2;
        break;

        case 23:
          targetPiece2 = piece23_2;
        break;

        case 24:
          targetPiece2 = piece24_2;
        break;

        case 25:
          targetPiece2 = piece25_2;
        break;

        case 26:
          targetPiece2 = piece26_2;
        break;

        case 27:
          targetPiece2 = piece27_2;
        break;

        case 28:
          targetPiece2 = piece28_2;
        break;

        case 29:
          targetPiece2 = piece29_2;
        break;

        case 30:
          targetPiece2 = piece30_2;
        break;

        case 31:
          targetPiece2 = piece31_2;
        break;

        case 32:
          targetPiece2 = piece32_2;
        break;

        case 33:
          targetPiece2 = piece33_2;
        break;

        case 34:
          targetPiece2 = piece34_2;
        break;

        case 35:
          targetPiece2 = piece35_2;
        break;
      }
      Hand.outFlg = false;
      createFlg1 = false;
      createPiece2 = Instantiate(targetPiece2, new Vector3(0f, 12f, 0), Quaternion.identity);
      createPiece2.AddComponent<JigsawPuzzle>();
    } else if (createFlg2) {
      InitializeValue();
      switch(stageNow) {
        case 6:
          targetPiece3 = piece6_3;
        break;

        case 7:
          targetPiece3 = piece7_3;
        break;

        case 8:
          targetPiece3 = piece8_3;
        break;

        case 9:
          targetPiece3 = piece9_3;
        break;

        case 10:
          targetPiece3 = piece10_3;
        break;

        case 11:
          targetPiece3 = piece11_3;
        break;

        case 12:
          targetPiece3 = piece12_3;
        break;

        case 13:
          targetPiece3 = piece13_3;
        break;

        case 14:
          targetPiece3 = piece14_3;
        break;

        case 15:
          targetPiece3 = piece15_3;
        break;

        case 16:
          targetPiece3 = piece16_3;
        break;

        case 17:
          targetPiece3 = piece17_3;
        break;

        case 18:
          targetPiece3 = piece18_3;
        break;

        case 19:
          targetPiece3 = piece19_3;
        break;

        case 20:
          targetPiece3 = piece20_3;
        break;

        case 21:
          targetPiece3 = piece21_3;
        break;

        case 22:
          targetPiece3 = piece22_3;
        break;

        case 23:
          targetPiece3 = piece23_3;
        break;

        case 24:
          targetPiece3 = piece24_3;
        break;

        case 25:
          targetPiece3 = piece25_3;
        break;

        case 26:
          targetPiece3 = piece26_3;
        break;

        case 27:
          targetPiece3 = piece27_3;
        break;

        case 28:
          targetPiece3 = piece28_3;
        break;

        case 29:
          targetPiece3 = piece29_3;
        break;

        case 30:
          targetPiece3 = piece30_3;
        break;

        case 31:
          targetPiece3 = piece31_3;
        break;

        case 32:
          targetPiece3 = piece32_3;
        break;

        case 33:
          targetPiece3 = piece33_3;
        break;

        case 34:
          targetPiece3 = piece34_3;
        break;

        case 35:
          targetPiece3 = piece35_3;
        break;
      }
      Hand.outFlg = false;
      createFlg2 = false;
      createPiece3 = Instantiate(targetPiece3, new Vector3(0f, 12f, 0), Quaternion.identity);
      createPiece3.AddComponent<JigsawPuzzle>();
    }else if (createFlg3) {
      InitializeValue();
      switch(stageNow) {
        case 21:
          targetPiece4 = piece21_4;
        break;

        case 22:
          targetPiece4 = piece22_4;
        break;

        case 23:
          targetPiece4 = piece23_4;
        break;

        case 24:
          targetPiece4 = piece24_4;
        break;

        case 25:
          targetPiece4 = piece25_4;
        break;

        case 26:
          targetPiece4 = piece26_4;
        break;

        case 27:
          targetPiece4 = piece27_4;
        break;

        case 28:
          targetPiece4 = piece28_4;
        break;

        case 29:
          targetPiece4 = piece29_4;
        break;

        case 30:
          targetPiece4 = piece30_4;
        break;

        case 31:
          targetPiece4 = piece31_4;
        break;

        case 32:
          targetPiece4 = piece32_4;
        break;

        case 33:
          targetPiece4 = piece33_4;
        break;

        case 34:
          targetPiece4 = piece34_4;
        break;

        case 35:
          targetPiece4 = piece35_4;
        break;
      }
      Hand.outFlg = false;
      createFlg3 = false;
      createPiece4 = Instantiate(targetPiece4, new Vector3(0f, 12f, 0), Quaternion.identity);
      createPiece4.AddComponent<JigsawPuzzle>();
    }
  }

  /**
   * ゲーム終了時処理
   */ 
  void OnApplicationQuit(){
    Save();
  }

  /**
   * セーブ処理
   */ 
  public static void Save(){
    // ステージをセットする
    PlayerPrefs.SetInt ("STAGE", stageLatest);
    PlayerPrefs.Save();
  }

  /**
   * ステージ選択時処理
   */ 
  void SelectStage() {

    // 開始フラグを初期化
    StartGame.startFlg = false;

    switch(stageNow) {
      case 1:
        targetPiece1 = piece1_1;
        targetStage = stage1;
        stageY = -7f;
        break; 
        
      case 2:
        targetPiece1 = piece2_1;
        targetStage = stage2;
        stageY = -9f;
        break;

      case 3:
        targetPiece1 = piece3_1;
        targetStage = stage3;
        stageY = -7f;
        break;

      case 4:
        targetPiece1 = piece4_1;
        targetStage = stage4;
        stageY = -4f;
        break;

      case 5:
        targetPiece1 = piece5_1;
        targetStage = stage5;
        stageY = -2f;
        break;

      case 6:
        targetPiece1 = piece6_1;
        targetStage = stage6;
        stageY = -4f;
        break;

      case 7:
        targetPiece1 = piece7_1;
        targetStage = stage7;
        stageY = -7f;
        break;

      case 8:
        targetPiece1 = piece8_1;
        targetStage = stage8;
        stageY = -4f;
        break;

      case 9:
        targetPiece1 = piece9_1;
        targetStage = stage9;
        stageY = -11f;
        break;

      case 10:
        targetPiece1 = piece10_1;
        targetStage = stage10;
        stageY = -6f;
        break;

      case 11:
        targetPiece1 = piece11_1;
        targetStage = stage11;
        stageY = -9f;
        break;

      case 12:
        targetPiece1 = piece12_1;
        targetStage = stage12;
        stageY = -5f;
        break;

      case 13:
        targetPiece1 = piece13_1;
        targetStage = stage13;
        stageY = -9f;
        break;

      case 14:
        targetPiece1 = piece14_1;
        targetStage = stage14;
        stageY = -6f;
        break;

      case 15:
        targetPiece1 = piece15_1;
        targetStage = stage15;
        stageY = -4f;
        break;

      case 16:
        targetPiece1 = piece16_1;
        targetStage = stage16;
        stageY = -7f;
        break;

      case 17:
        targetPiece1 = piece17_1;
        targetStage = stage17;
        stageY = -2f;
        break;

      case 18:
        targetPiece1 = piece18_1;
        targetStage = stage18;
        stageY = -8f;
        break;

      case 19:
        targetPiece1 = piece19_1;
        targetStage = stage19;
        stageY = -5f;
        break;

      case 20:
        targetPiece1 = piece20_1;
        targetStage = stage20;
        stageY = -6f;
        break;

      case 21:
        targetPiece1 = piece21_1;
        targetStage = stage21;
        stageY = -4f;
        break;

      case 22:
        targetPiece1 = piece22_1;
        targetStage = stage22;
        stageY = -9f;
        break;

      case 23:
        targetPiece1 = piece23_1;
        targetStage = stage23;
        stageY = -6f;
        break;

      case 24:
        targetPiece1 = piece24_1;
        targetStage = stage24;
        stageY = -7f;
        break;

      case 25:
        targetPiece1 = piece25_1;
        targetStage = stage25;
        stageY = -5f;
        break;

      case 26:
        targetPiece1 = piece26_1;
        targetStage = stage26;
        stageY = -6f;
        break;

      case 27:
        targetPiece1 = piece27_1;
        targetStage = stage27;
        stageY = -1f;
        break;

      case 28:
        targetPiece1 = piece28_1;
        targetStage = stage28;
        stageY = -7f;
        break;

      case 29:
        targetPiece1 = piece29_1;
        targetStage = stage29;
        stageY = -10f;
        break;

      case 30:
        targetPiece1 = piece30_1;
        targetStage = stage30;
        stageY = -5f;
        break;

      case 31:
        targetPiece1 = piece31_1;
        targetStage = stage31;
        stageY = -8f;
        break;

      case 32:
        targetPiece1 = piece32_1;
        targetStage = stage32;
        stageY = -3f;
        break;

      case 33:
        targetPiece1 = piece33_1;
        targetStage = stage33;
        stageY = -4f;
        break;

      case 34:
        targetPiece1 = piece34_1;
        targetStage = stage34;
        stageY = -10f;
        break;

      case 35:
        targetPiece1 = piece35_1;
        targetStage = stage35;
        stageY = -2f;
        break;
    }

    // ピースを作成する
    createPiece1 = Instantiate(targetPiece1, new Vector3(pieceX, pieceY, 0), Quaternion.identity);
    createPiece1.AddComponent<JigsawPuzzle>();
 
    // ステージを作成する
    createStage = Instantiate(targetStage, new Vector3(stageX, stageY, 0), Quaternion.identity);
  }

  void InitializeValue() {
    // 値を初期化
    pieceX = 0f;
    pieceY = 12f;
    stageX = 0f;
    stageY = 0f;
  }
}
