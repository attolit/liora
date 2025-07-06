using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class serihu : MonoBehaviour
{
    [SerializeField]
    private Text text = default;

    [SerializeField] public int givendamage;


    //kokodayo
    private List<string> serihus = new List<string>
{
    "どうも〜","Debug用のセリフです！","何か用？"
};

    private List<string> choose = new List<string>
{
    "調子どう？","エラー出てる？","なんでもない"
};
    public List<GameObject> tatie = new List<GameObject>();

    //kokodayo

    //atode button kamo
    public GameObject bunki1;
    public GameObject bunki2;
    public GameObject bunki3;
    public GameObject canlookmp;
    public GameObject go;

    private int currentIndex = 0;
    [SerializeField] public int mp;
    bool end = true;
    bool onlyone = true;
    bool scencearune = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // bunki1.onClick.AddListener(OnBunki1);
        
        mp = 10;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A)) // テスト用
        {
            sammon();
        }

        if (Input.GetKeyDown(KeyCode.Space) && currentIndex < serihus.Count && end && onlyone)
        {
            text.DOText("", 0); // 一度テキストをクリア
            end = false;

            text.DOText(serihus[currentIndex], 1).OnComplete(() =>
            {
                currentIndex++;
                end = true;

                // セリフが全て終わった直後に選択肢を出す
                if (currentIndex == serihus.Count && onlyone)
                {
                    sammon();
                    onlyone = false;
                    currentIndex = 0;
                }
            });
        }


        //if (Input.GetKeyDown(KeyCode.Space) && currentIndex < kotoba.Count)
        //{
        //    text.DOText("", 0); // 一度テキストをクリア
        //    end = false;
        //    text.DOText(kotoba[currentIndex], 1).OnComplete(() =>
        //    {
        //        end = true;
        //        currentIndex++; // インデックスを進める

        //    }); // 新しいセリフを表示

        //}


    }
    public void sammon()
    {
        bunki1.GetComponentInChildren<Text>().text = choose[0];
        bunki2.GetComponentInChildren<Text>().text = choose[1];
        bunki3.GetComponentInChildren<Text>().text = choose[2];
        bunki1.SetActive(true);
        bunki2.SetActive(true);
        bunki3.SetActive(true);

    }
    public void elase()
    {
        bunki1.SetActive(false);
        bunki2.SetActive(false);
        bunki3.SetActive(false);
        onlyone = true;
    }
    public void OnBunki1()
    {
        canlookmp.SetActive(true);
        mp += 10;
        elase();
        List<string> serihu01 = new List<string>
     {
        "絶好調だよ！","そっちは？"

     };
        tatie[0].SetActive(false);
        tatie[2].SetActive(true);
        serihus = serihu01;
        EventSystem.current.SetSelectedGameObject(null);
        List<string> choose01 = new List<string>
     {
        "DTMで音楽","メディアアート作品","Unityでゲーム"

     };
        tatie[0].SetActive(false);
        tatie[3].SetActive(true);
        choose = choose01;

    }
    public void OnBunki2()
    {
        elase();
        mp += 10;
        List<string> serihu02 = new List<string>
     {
        "今はエラー無いよ！","すごい！","開発頑張ってね！"
     };
        serihus = serihu02;
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnBunki3()
    {
        elase();
        List<string> serihu03 = new List<string>
     {
        "そっか..."
     };
        tatie[0].SetActive(false);
        tatie[1].SetActive(true);
        serihus = serihu03;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Scence()
    {
        
        SceneManager.LoadScene("ura");
        scencearune = true;
    }



}