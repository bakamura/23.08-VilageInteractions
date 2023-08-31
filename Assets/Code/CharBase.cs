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

    private GenderT gender;
    private uint age;
    private RaceT race;
    private MoneyT money;
    private float humor;
    private PersonalityT persona;

    public GenderT Gender { get { return gender; } }
    public uint Age {  get { return age; } }
    public RaceT Race { get { return race; } }
    public MoneyT Money { get { return money; } }
    public PersonalityT Persona { get { return persona; } }

    public abstract void Interact(CharBase charInfo);

}
