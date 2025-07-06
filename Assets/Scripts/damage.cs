using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class damage : MonoBehaviour
{


    public int hp = 100;
    public int ehp = 100;
    [SerializeField]public int nowhp;
    public int takedamage;
    public int finalDamage;
    public int nowehp;
    [SerializeField] GameObject e1_my;
    [SerializeField] GameObject e2_te;
    //zokusei huetara add siyou

    bool canfinish = false;

    public GameObject bunki1;
    public GameObject bunki2;
    public GameObject bunki3;
    public GameObject sutan;
    public GameObject kattoin;
    public GameObject tatie;
    public GameObject sitazi;
    public GameObject hudasi;
    public GameObject bunki;
    public GameObject kakutei;
    public List<GameObject> siori = new List<GameObject>(); //siori
    [SerializeField] public int mp;
    [SerializeField] public int givendamage;

    public List<Spell> allSpells; // 全部の魔法一覧
    public List<int> sioriIndexes = new List<int>(); // 登録したしおり番号

    //catin you
    public RectTransform panel;       // CutInPanelのRectTransform（親）
    public CanvasGroup canvasGroup;   // CutInPanelに付けたCanvasGroup

    public float slideDuration = 0.4f;
    public float visibleTime = 1.5f;
    public float fadeOutDuration = 0.3f;
    public Vector2 startPos = new Vector2(-1000f, 0f);
    public Vector2 centerPos = new Vector2(0f, 0f);
    public Vector2 exitPos = new Vector2(1000f, 0f);





    public enum BookType
    {
        Mystic,
        Tech,
        Action,
        Drama,
        Verse
    }

    public static class TypeEffectiveness
    {
        private static readonly Dictionary<(BookType attacker, BookType defender), float> effectivenessTable =
            new Dictionary<(BookType, BookType), float>//type bairitu
            {
            {(BookType.Mystic, BookType.Drama), 1.5f},
            {(BookType.Drama, BookType.Action), 1.5f},
            {(BookType.Action, BookType.Tech), 1.5f},
            {(BookType.Tech, BookType.Mystic), 1.5f},

            {(BookType.Drama, BookType.Mystic), 0.5f},
            {(BookType.Action, BookType.Drama), 0.5f},
            {(BookType.Tech, BookType.Action), 0.5f},
            {(BookType.Mystic, BookType.Tech), 0.5f},

            {(BookType.Verse, BookType.Mystic), 1.0f},
            {(BookType.Verse, BookType.Tech), 1.0f},
            {(BookType.Verse, BookType.Action), 1.0f},
            {(BookType.Verse, BookType.Drama), 1.0f},
            {(BookType.Verse, BookType.Verse), 1.0f},
            };

        public static float GetEffectiveness(BookType attacker, BookType defender)
        {
            if (effectivenessTable.TryGetValue((attacker, defender), out float multiplier))
            {
                return multiplier;
            }
            return 1.0f; // 相性がない場合は通常倍率
        }
        public static float CalculateTypeDamage(BookType attackerType, BookType defenderType, float baseDamage)
        {
            float typeMultiplier = GetEffectiveness(attackerType, defenderType);
            return baseDamage * typeMultiplier;
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        attack();
        panel.anchoredPosition = startPos;
        canvasGroup.alpha = 0f;
    }

    // Update is called once per frame
    void Update()//kokononakami messageni kaeru
    {

        if(canfinish)
        {
            StartCoroutine(finishattack());
        }
        if(ehp < 0)
        {
            
            gameObject.SendMessage("OnRecieve");
           //break dayo
        }

        if(Input.GetKeyDown(KeyCode.C)) // to debug
        {
            StartCoroutine(finishattack());
        }
        if(Input.GetKeyDown(KeyCode.X))//to debug
        {
            PlayCutIn();
        }

    }

    public void UseSiori(int sioriSlot)//sin kinou
    {
        if (sioriSlot < 0 || sioriSlot >= sioriIndexes.Count) return;

        int spellIndex = sioriIndexes[sioriSlot];
        Spell selectedSpell = allSpells[spellIndex];

        selectedSpell.Cast(damage.BookType.Mystic, out int damageResult); // 例: 敵がMysticタイプ
        ehp -= damageResult;
        Debug.Log($"使った魔法: {selectedSpell.spellName} / ダメージ: {damageResult}");
    }

    public void AddSiori(int spellIndex)//ue no tuduki
    {
        if (!sioriIndexes.Contains(spellIndex))
        {
            sioriIndexes.Add(spellIndex);
        }
    }

    public void OnBookPicked()//messe hatudou
    {
        StartCoroutine(PickUpBookAndCast());
    }



    private IEnumerator finishattack()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //tatie hosiina
            
            yield return new WaitForSecondsRealtime(1.5f);
            
            StartCoroutine(attack());

            canfinish = false;

        }
    }
    public void endshori(int value)
    {
        
        nowhp = hp - givendamage;

        hp = nowhp;

        //damage shori

    }

    public List<MagicSpell> spellLibrary = new List<MagicSpell>();

    void Awake()
    {
        spellLibrary.Add(new MagicSpell("name", 50, BookType.Action, (user, enemy) =>
        {
            Debug.Log("hatudou");
            // damage effect etc...
        }));//other majics will be add at under
    }

    private IEnumerator attack() //effect & damage
    {
        //all kinds of effects

        e1_my = GameObject.Find("e2");
        e2_te = GameObject.Find("e3");


        if (canfinish)//hissatu
        {
            //effect appear and recognize type

            e1_my.SetActive(true);
            e2_te.SetActive(true);
            yield return new WaitForSecondsRealtime(1.0f);
            e1_my.SetActive(false);
            e2_te.SetActive(false);
            takedamage = 70;
        }
        else//normal
        {
            takedamage = 30;//実験!倍率調整 sita

            
            BookType attacker = BookType.Tech;//type aishou
            BookType defender = BookType.Mystic;
            float finalDamage = TypeEffectiveness.CalculateTypeDamage(attacker, defender, 30f);//damage keisan

            //type kokomade

            //if is here
            e1_my.SetActive(true);
            e2_te.SetActive(true);
            yield return new WaitForSecondsRealtime(1.0f);

            e1_my.SetActive(false);
            e2_te.SetActive(false);

            takedamage = 70; //base damage
            takedamage = Mathf.FloorToInt(finalDamage * takedamage);//type damage

        }
        //finally damage　(to enemy)

        nowehp = ehp - takedamage;
        ehp = nowehp;
        Debug.Log(ehp);
        
        
        
    }

    public IEnumerator PickUpBookAndCast()// pick shori
    {
        var randomSpell = spellLibrary[Random.Range(0, spellLibrary.Count)];
        Debug.Log("name：" + randomSpell.spellName);

        yield return new WaitForSeconds(1f); // ちょっと演出待ち

       // randomSpell.effect(gameObject, enemyObject);

    }

    public void PlayCutIn()
    {
        Sequence seq = DOTween.Sequence();

        // 登場：スライド＋フェードイン
        seq.Append(canvasGroup.DOFade(1f, 0.2f));
        seq.Join(panel.DOAnchorPos(centerPos, slideDuration).SetEase(Ease.OutBack));

        // 表示キープ
        seq.AppendInterval(visibleTime);

        // 退場：フェードアウト＋スライドアウト
        seq.Append(canvasGroup.DOFade(0f, fadeOutDuration));
        seq.Join(panel.DOAnchorPos(exitPos, 0.5f).SetEase(Ease.InBack));

        seq.OnComplete(() =>
        {
            Debug.Log("カットイン終わったよ！");
        });
    }
}

