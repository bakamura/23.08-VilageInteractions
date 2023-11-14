using UnityEngine;

public abstract class CharBase : MonoBehaviour
{
    public enum GenderT
    {
        Male,
        Female,
        Other
    }
    public enum RaceT
    {
        Human,
        Animal,
        Spirit,
        NonHuman
    }
    public enum MoneyT
    {
        Poor,
        Medium,
        Rich
    }
    public enum PersonalityT
    {
        Sadistic,
        Grumpy,
        Loud,
        Shy,
        Kind,
        Flirty
    }

    [SerializeField] protected GenderT gender;
    [SerializeField,] protected uint age;
    [SerializeField] protected RaceT race;
    [SerializeField] protected MoneyT money;
    [SerializeField, Range(-3, 3)] protected float humor;
    [SerializeField] protected PersonalityT persona;

    public GenderT Gender { get { return gender; } set { gender = value; } }
    public uint Age { get { return age; } set { age = value; } }
    public RaceT Race { get { return race; } set { race = value; } }
    public MoneyT Money { get { return money; } set { money = value; } }
    public float Humor { get { return humor; } set { humor = value; } }
    public PersonalityT Persona { get { return persona; } set { persona = value; } }

    public abstract void Interact(CharBase charInfo);
}
