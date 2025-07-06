using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class roleofscene : MonoBehaviour
{
    [System.Serializable]
    public class SceneData
    {
        public List<JumpData> jumpAfterSerihu;

        public List<string> serihus;
        public List<string> choose;
        public List<int> nextSceneIndexes;
        public List<int> nextSerihuIndexes; // 各選択肢の飛び先セリフ番号（scene内）

        public int nextSceneAfterFinish = -1;　//-1 is serihutobanai
    }



    [System.Serializable]
    public class JumpData
    {
        public int fromSerihuIndex; // このセリフのあとに
        public int toSceneIndex;    // どのsceneへ
        public int toSerihuIndex;   // そのsceneの中の何番目のserihuへ
    }

    [SerializeField] private Text text;
    [SerializeField] private List<SceneData> scenes = new List<SceneData>();

    public GameObject bunki1;
    public GameObject bunki2;
    public GameObject bunki3;
    public GameObject canlookmp;
    public GameObject enemyController;
    //public OtherScript otherScript;

    private int currentSceneIndex = 0;//scene kazoeru
    private int currentSerihuIndex = 0;//serihu bangou
    public int ransu;//ransu shori you
    private bool end = true;//serihu zenbu deteruka
    private bool isChoosing = false;//sentakusi deruka denaika

    public System.Action onBattleEnd;//sentou shuuryou

    [SerializeField] damage damageScript;// messe okuru script
    // private bool hasShownChoices = false;


    void Start()
    {
        ShowNextSerihu();

    }

    void Update()// umaku itteruyo
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log( "isChoosing" + isChoosing);
            Debug.Log( "end" + end);
            if (end && !isChoosing) {
                ShowNextSerihu();
                Debug.Log(isChoosing);
            }

        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlaySerihuBySceneIndex(5);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlaySerihuBySceneIndex(6);
        }

        //debug
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlaySerihuBySceneIndex(1);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlaySerihuBySceneIndex(2);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlaySerihuBySceneIndex(3);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlaySerihuBySceneIndex(4);
        }
    }

    void ShowNextSerihu()
    {
        if (currentSceneIndex >= scenes.Count) return;

        var scene = scenes[currentSceneIndex];

        if (currentSerihuIndex < scene.serihus.Count)//一旦そのシーンで出したいセリフを全部出す
        {
            text.DOText("", 0);
            end = false;
            string currentLine = scene.serihus[currentSerihuIndex];

            text.DOText(currentLine, 1).OnComplete(() =>
            {
                end = true;

                // 1. ジャンプ処理
                foreach (var jump in scene.jumpAfterSerihu)
                {
                    if (jump.fromSerihuIndex == currentSerihuIndex)
                    {
                        currentSceneIndex = jump.toSceneIndex;
                        currentSerihuIndex = jump.toSerihuIndex;
                        ShowNextSerihu();
                        return;
                    }
                }

                // 2. セリフインデックス進める
                currentSerihuIndex++;

                // 3. 分岐が必要ならコルーチン（分岐が最終セリフの時用）
                if (currentSerihuIndex >= scene.serihus.Count && scene.choose.Count > 0)
                {
                    StartCoroutine(WaitAndShowChoices());
                }
            });

        }
        else
        {
            if (scene.choose.Count > 0)
            {
                Debug.Log("ShowChoices");
                //   isChoosing = true;//detekuru kedo false yori hayai.
                StartCoroutine(WaitAndShowChoices());
            }

            else
            {
                if (scene.nextSceneAfterFinish != -1)
                {
                    currentSceneIndex = scene.nextSceneAfterFinish;
                    currentSerihuIndex = 0;
                    ShowNextSerihu();
                }
                else
                {
                    currentSceneIndex++;
                    currentSerihuIndex = 0;

                    if (currentSceneIndex < scenes.Count)
                    {
                        ShowNextSerihu();

                    }
                }
            }
        }
    }


    private IEnumerator WaitAndShowChoices()
    {
       // Debug.Log(isChoosing);
        isChoosing = true; //ごめん、全然必要だった
        Debug.Log(isChoosing);
        yield return new WaitForSeconds(.0f);
        ShowChoices();
    }




    public void PlaySerihuBySceneIndex(int index)
    {
        //hasShownChoices = false;
        if (index >= 0 && index < scenes.Count)
        {
            currentSceneIndex = index;
            currentSerihuIndex = 0;
            elase(); // 選択肢が表示されてる場合は消す
            isChoosing = false;
            ShowNextSerihu();
        }
    }




    void ShowChoices()
    {
        isChoosing = true;
        Debug.Log("1");

        var choices = scenes[currentSceneIndex].choose;
        if (choices.Count >= 3)
        {
            bunki1.GetComponentInChildren<Text>().text = choices[0];
            bunki2.GetComponentInChildren<Text>().text = choices[1];
            bunki3.GetComponentInChildren<Text>().text = choices[2];
        }

        bunki1.SetActive(true);
        bunki2.SetActive(true);
        bunki3.SetActive(true);
    }


    public void elase()//bunki kesu
    {
        bunki1.SetActive(false);
        bunki2.SetActive(false);
        bunki3.SetActive(false);
    }


    public List<string> GetFormattedSerihus(SceneData data, string bookTitle)//bunseki de tukau
    {
        List<string> result = new List<string>();
        foreach (string line in data.serihus)
        {
            string replaced = line.Replace("{TITLE}", bookTitle);
            result.Add(replaced);
        }
        return result;
    }

    public void OnBunki1() => JumpToSceneByChoice(0);
    public void OnBunki2() => JumpToSceneByChoice(1);
    public void OnBunki3() => JumpToSceneByChoice(2);


    void JumpToSceneByChoice(int choiceIndex)//bunki
    {
        //hasShownChoices = false;
        Debug.Log("bunkibunki~~");
        elase();
        Debug.Log("2");
        isChoosing = false;
        Debug.Log("3");


        var scene = scenes[currentSceneIndex];
        //kokorani message kaku
        if (currentSceneIndex == 1 && choiceIndex == 0)
        {
            Debug.Log("シーン1の分岐1を選択したよ！");//sentou
                                         // otherScript.ReceiveMessage("シーン1の分岐1が選ばれた");
                                         // other shori is hear
        }
        else if (currentSceneIndex == 1 && choiceIndex == 1)//bunseki
        {
            Debug.Log("シーン1の分岐2を選択したよ！");
        }
        else if (currentSceneIndex == 1 && choiceIndex == 2)
        {
            Debug.Log("シーン1の分岐3を選択したよ！");//nigeru
            ransu = Random.Range(1, 3);
            if (ransu <= 2)
            {
                Debug.Log("seikou");
                enemyController.GetComponent<enemyatack>().onBattleEnd.Invoke();//戦闘終了したらこれこれ。全スクリプト共通でいける。enemyattack ni aruyo.

            }
            else
            {
                Debug.Log("sippai");
                //敵にターン渡す処理を余裕あったら
            }
        }
        else if (currentSceneIndex == 8 && choiceIndex == 1)//本拾うか選べるよ
        {
            Debug.Log("シーン8の分岐1を選択したよ！");
        }
        else if (currentSceneIndex == 2 && choiceIndex == 3)//本を拾うやつ
        {
            Debug.Log("シーン2の分岐3を選択したよ！");
            damageScript.OnBookPicked();
            //message okuruyo
        }


        if (choiceIndex < scene.nextSceneIndexes.Count)//セリフ終わった後に移るセリフ
        {
            currentSceneIndex = scene.nextSceneIndexes[choiceIndex];

            if (scene.nextSerihuIndexes != null && choiceIndex < scene.nextSerihuIndexes.Count)
            {
                currentSerihuIndex = scene.nextSerihuIndexes[choiceIndex];
            }
            else
            {
                currentSerihuIndex = 0; // ← 保険
            }

            ShowNextSerihu();//koko ayasii
        }
    }




}