using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpell : MonoBehaviour
{
    public string spellName;
    public int baseDamage;
    public damage.BookType type;
    public System.Action<GameObject, GameObject> effect; // 発動効果（プレイヤーと敵に対して）

    public MagicSpell(string name, int damage, damage.BookType t, System.Action<GameObject, GameObject> fx)
    {
        spellName = name;
        baseDamage = damage;
        type = t;
        effect = fx;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
