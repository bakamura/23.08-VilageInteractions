using UnityEngine;

public abstract class CharBase : MonoBehaviour {

    public enum GenderT {
        Male,
        Female,
        Other
    }
    public enum RaceT {
        Human,
        Animal,
        Spirit,
        NonHuman
    }
    public enum MoneyT {
        Poor,
        Medium,
        Rich
    }
    public enum PersonalityT {
        Sadistic,
        Grumpy,
        Loud,
        Shy,
        Kind,
        Flirty
    }

    [SerializeField] protected GenderT gender;
    [SerializeField] protected uint age;
    [SerializeField] protected RaceT race;
    [SerializeField] protected MoneyT money;
    [SerializeField] protected float humor;
    [SerializeField] protected PersonalityT persona;

    public GenderT Gender { get { return gender; } }
    public uint Age { get { return age; } }
    public RaceT Race { get { return race; } }
    public MoneyT Money { get { return money; } }
    public float Humor { get { return humor; } }
    public PersonalityT Persona { get { return persona; } }

    public abstract void Interact(CharBase charInfo);

}
