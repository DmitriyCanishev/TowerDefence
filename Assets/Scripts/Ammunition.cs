using UnityEngine;

public enum ammunitionType
{
    arrow, fireball, rock
};

public class Ammunition : MonoBehaviour
{
    [SerializeField]
    int attackDamage;

    [SerializeField]
    ammunitionType aType;

    public int AttackDamage
    {
        get
        {

            return attackDamage;
        }
    }

    public ammunitionType AmmunitionType
    {
        get
        {

            return aType;
        }
    }
}
