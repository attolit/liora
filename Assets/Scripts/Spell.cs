using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Magic/Spell")]
public class Spell : ScriptableObject
{
    public string spellName;
    public int baseDamage;
    public damage.BookType type;

    public void Cast(damage.BookType targetType, out int finalDamage)
    {
        float typeMultiplier = damage.TypeEffectiveness.GetEffectiveness(type, targetType);
        finalDamage = Mathf.FloorToInt(baseDamage * typeMultiplier);
    }
}
