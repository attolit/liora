using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class enemyatack : MonoBehaviour
{
    public  int decreacedamage = 30;
    public float cantime = 10.0f;
    public float abletime = 10.0f;
    public int decede;
    GameObject clickedGameObject;

    public Text timeText;
    public Text tellText;
    public Text teachText;

    public GameObject d1;
    public GameObject d2;
    [SerializeField] public int givendamage;

    [SerializeField]bool sutan = false;

    public UnityEvent onBattleEnd;

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
            new Dictionary<(BookType, BookType), float>
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void tarn()
    {
        if (sutan) // sutan hantei
        {

            gameObject.SendMessage("notmove");
        }
        else // rannsuu ni sitoita or AI
        {
            int rnd = Random.Range(1, 3);

            if (decede == 1)
            {
                StartCoroutine(attack1());
            }
            else if (decede == 2 && decede == 3)
            {
                StartCoroutine(attack2());
            }

        }
    }

    public void OnRecieve(int value)
    {
        sutan = true;
    }


    private IEnumerator attack1() //1
    {
        tellText.enabled = true;
        cantime -= Time.deltaTime;
        timeText.text = cantime.ToString("f1");


        if (Input.GetKey(KeyCode.Space)) //renda renda
        {
            decreacedamage--;

            if(cantime <= 0) // time limit
            {
                if (decreacedamage == 0) //renda seikou
                {
                    gameObject.SendMessage("nodamage");
                    sutan = true;
                }
                else // sippai
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    givendamage = 90;
                    //damage
                }
                tellText.enabled = false;
                gameObject.SendMessage("endshori");

            }
        }
    }

    private IEnumerator attack2() //2
    {
        teachText.enabled = true;
        d1.SetActive(true);
        d2.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        
        if(abletime < 0)　//I love time limit
        {
            cantime -= Time.deltaTime;
            timeText.text = cantime.ToString("f1");
        }
        else{ //クリックしたら消えるやつ
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                if (hit2d)
                {
                    clickedGameObject = hit2d.transform.gameObject;
                    Debug.Log(clickedGameObject.name);//ゲームオブジェクトの名前を出力
                    Destroy(clickedGameObject);//ゲームオブジェクトを破壊
                }
            }
        }

        teachText.enabled = false;
        gameObject.SendMessage("endshori");
    }
}
 