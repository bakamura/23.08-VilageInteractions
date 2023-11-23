using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharBase;
using Random = UnityEngine.Random;

[Serializable]
public struct IdentityStats
{
    public GenderT realGender;
    public uint realAge;
    public RaceT realRace;
    public MoneyT realMoney;
    [Range(-3, 3)] public float realHumor;
    public PersonalityT realPersona;
}
public class CharMori : CharBase
{
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 1f;
    private void Update()
    {
        //Nao precisa mexer
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed / 10 * Time.deltaTime);
    }
    /*
    Nome dos lugares no mapa:
    TownSquare
    Bakery
    Bar
    Library
    Hospital
    ?
    */
    public enum Identity
    {
        ShyKid,
        CrazyWoman,
        AngryOldMan
    }
    public Identity currentIdentity;
    [Header("-----Identity Values-----"), Space]
    public IdentityStats shyKidStats;
    public IdentityStats crazyWomanStats;
    public IdentityStats angryOldManStats;

    private float IdentityDistortion;
    public float identityDistortion
    {
        get { return IdentityDistortion; }
        set
        {
            if (value > identityDistortion)
            {
                OnIdentityDistortionChange();
            }
            if (value > 1)
            {
                IdentityDistortion = 1;
            }
            else IdentityDistortion = value;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Nao mexer nessa funcao ate a implementacao de interacoes especificas com alguns personagens
        if (collision.gameObject.tag == "Char")
        {
            if (collision.TryGetComponent<CharBase>(out CharBase charBase))
            {
                charBase.Interact(this);
            }
            else
            {
                Debug.Log("Erro em pegar informacoes de" + collision.gameObject.name);
            }
        }
    }

    public IdentityStats GetCurrentIdentityStats()
    {
        if (Random.value < identityDistortion)
        {
            switch (currentIdentity)
            {
                case Identity.ShyKid:
                default:
                    return shyKidStats;

                case Identity.CrazyWoman:
                    return crazyWomanStats;

                case Identity.AngryOldMan:
                    return angryOldManStats;
            }
        }
        else
        {
            IdentityStats identityStats = new IdentityStats();
            //Gender
            switch (Random.Range(0, 2))
            {
                case 0:
                    identityStats.realGender = shyKidStats.realGender;
                    break;
                case 1:
                    identityStats.realGender = crazyWomanStats.realGender;
                    break;
                case 2:
                    identityStats.realGender = angryOldManStats.realGender;
                    break;
            }
            //Persona
            switch (Random.Range(0, 2))
            {
                case 0:
                    identityStats.realPersona = shyKidStats.realPersona;
                    break;
                case 1:
                    identityStats.realPersona = crazyWomanStats.realPersona;
                    break;
                case 2:
                    identityStats.realPersona = angryOldManStats.realPersona;
                    break;
            }
            //Race
            switch (Random.Range(0, 2))
            {
                case 0:
                    identityStats.realRace = shyKidStats.realRace;
                    break;
                case 1:
                    identityStats.realRace = crazyWomanStats.realRace;
                    break;
                case 2:
                    identityStats.realRace = angryOldManStats.realRace;
                    break;
            }
            //Age
            switch (Random.Range(0, 2))
            {
                case 0:
                    identityStats.realAge = shyKidStats.realAge;
                    break;
                case 1:
                    identityStats.realAge = crazyWomanStats.realAge;
                    break;
                case 2:
                    identityStats.realAge = angryOldManStats.realAge;
                    break;
            }
            //Money
            switch (Random.Range(0, 2))
            {
                case 0:
                    identityStats.realMoney = shyKidStats.realMoney;
                    break;
                case 1:
                    identityStats.realMoney = crazyWomanStats.realMoney;
                    break;
                case 2:
                    identityStats.realMoney = angryOldManStats.realMoney;
                    break;
            }
            //Humor
            switch (Random.Range(0, 2))
            {
                case 0:
                    identityStats.realHumor = shyKidStats.realHumor;
                    break;
                case 1:
                    identityStats.realHumor = crazyWomanStats.realHumor;
                    break;
                case 2:
                    identityStats.realHumor = angryOldManStats.realHumor;
                    break;
            }
            return identityStats;
        }
    }
    public ref IdentityStats GetCurrentIdentityStatsRef()
    {
        switch (currentIdentity)
        {
            case Identity.ShyKid:
            default:
                return ref shyKidStats;

            case Identity.CrazyWoman:
                return ref crazyWomanStats;

            case Identity.AngryOldMan:
                return ref angryOldManStats;
        }
    }
    public string GetRoutinePosition(int period)
    {
        Identity identity = currentIdentity;
        if (identityDistortion > 0.75) identity = (Identity)Random.Range(0, 2);
        switch (identity)
        {
            case Identity.ShyKid:
            default:
                switch (period)
                {
                    case 0:
                        return "Bakery";
                    case 1:
                        return "TownSquare";
                    case 2:
                    case 3:
                        return "Library";
                    case 4:
                        return "TownSquare";
                    case 5:
                    case 6:
                    default:
                        return "Library";
                }
            case Identity.CrazyWoman:
                switch (period)
                {
                    case 0:
                        return "TownSquare";
                    case 1:
                        return "Bakery";
                    case 2:
                    case 3:
                        return "?";
                    case 4:
                        return "Library";
                    case 5:
                        return "Hospital";
                    case 6:
                    default:
                        return "Bar";

                }
            case Identity.AngryOldMan:
                switch (period)
                {
                    case 0:
                    case 1:
                        return "TownSquare";
                    case 2:
                    case 3:
                        return "Library";
                    case 4:
                    case 5:
                    case 6:
                    default:
                        return "Hospital";
                }

        }
    }

    public void OnIdentityDistortionChange()
    {
        currentIdentity = (Identity)Random.Range(0, 2);
        IdentityStats identityStats = GetCurrentIdentityStats();
        humor = identityStats.realHumor;
        age = identityStats.realAge;
        money = identityStats.realMoney;
        race = identityStats.realRace;
        gender = identityStats.realGender;
        persona = identityStats.realPersona;
    }
    void Start()
    {
        currentIdentity = (Identity)Random.Range(0, 2);
        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        identityDistortion += 0.05f;
        ReactToOtherNpc(charInfo);
    }
    public void ReactToOtherNpc(CharBase charInfo)
    {

        switch (currentIdentity)
        {
            case Identity.ShyKid:
                //Persona
                switch (charInfo.Persona)
                {
                    case PersonalityT.Kind:
                    case PersonalityT.Shy:
                        shyKidStats.realHumor += 1;
                        break;
                    default:
                        shyKidStats.realHumor -= 1;
                        break;
                }
                //Age
                if (charInfo.Age < 15 || charInfo.Age > 25)
                {
                    shyKidStats.realHumor += 1;
                }
                else
                {
                    shyKidStats.realHumor -= 1;
                }
                //Money dont change
                //Race
                switch (charInfo.Race)
                {
                    case RaceT.Animal:
                        shyKidStats.realHumor += 1;
                        break;
                    case RaceT.Spirit:
                    case RaceT.NonHuman:
                        shyKidStats.realPersona = PersonalityT.Loud;
                        shyKidStats.realHumor -= 1;
                        break;
                }
                //Gender
                if (shyKidStats.realGender != charInfo.Gender)
                {
                    if (charInfo.Humor >= 2)
                    {
                        shyKidStats.realPersona = PersonalityT.Kind;
                    }
                    else shyKidStats.realPersona = PersonalityT.Shy;
                }
                //Humor
                if (charInfo.Humor < -1)
                {
                    identityDistortion += 0.1f;
                    shyKidStats.realPersona = PersonalityT.Loud;
                    shyKidStats.realHumor -= 1;
                }
                else if (charInfo.Humor < 2)
                {
                    shyKidStats.realPersona = PersonalityT.Shy;
                    shyKidStats.realHumor += 1;
                }
                else
                {
                    shyKidStats.realPersona = PersonalityT.Kind;
                    shyKidStats.realHumor += 2;
                    identityDistortion -= 0.1f;
                }
                break;
            case Identity.CrazyWoman:
                //Persona
                switch (charInfo.Persona)
                {
                    case PersonalityT.Loud:
                    case PersonalityT.Sadistic:
                    case PersonalityT.Shy:
                        crazyWomanStats.realHumor -= 1;
                        break;
                }
                //Race
                switch (charInfo.Race)
                {
                    case RaceT.Human:
                        if (charInfo.Gender == GenderT.Male)
                        {
                            crazyWomanStats.realPersona = PersonalityT.Flirty;
                            crazyWomanStats.realHumor += 1;
                        }
                        break;
                }
                //Humor
                if (crazyWomanStats.realHumor < -2 || crazyWomanStats.realHumor > 2)
                {
                    crazyWomanStats.realHumor += 1;
                    crazyWomanStats.realPersona = PersonalityT.Loud;
                }
                else
                {
                    crazyWomanStats.realPersona = PersonalityT.Grumpy;
                }
                //Gender dont change
                //Age
                if (charInfo.Age - crazyWomanStats.realAge < 10 && charInfo.Age > 18)
                {
                    crazyWomanStats.realPersona = PersonalityT.Flirty;
                    if (charInfo.Humor > 2)
                    {
                        crazyWomanStats.realHumor += 1;
                    }
                }
                //Money dont change
                break;
            case Identity.AngryOldMan:
                //Age
                if (charInfo.Age >= 65)
                {
                    angryOldManStats.realHumor += 1;
                }
                //Gender
                if (charInfo.Gender == GenderT.Other)
                {
                    angryOldManStats.realPersona = PersonalityT.Grumpy;
                    angryOldManStats.realHumor -= 1;
                }
                //Persona
                switch (charInfo.Persona)
                {
                    case PersonalityT.Flirty:
                    case PersonalityT.Loud:
                    case PersonalityT.Shy:
                        angryOldManStats.realHumor -= 1;
                        break;
                    case PersonalityT.Grumpy:
                    case PersonalityT.Sadistic:
                        angryOldManStats.realPersona = PersonalityT.Grumpy;
                        break;
                    case PersonalityT.Kind:
                        angryOldManStats.realHumor += 1;
                        break;
                }
                //Humor dont change
                //Money dont change
                //Race
                switch (charInfo.Race)
                {
                    case RaceT.Animal:
                    case RaceT.Human:
                        angryOldManStats.realHumor += 1;
                        break;
                    case RaceT.Spirit:
                        angryOldManStats.realPersona = PersonalityT.Loud;
                        angryOldManStats.realHumor -= 2;
                        break;
                    case RaceT.NonHuman:
                        angryOldManStats.realPersona = PersonalityT.Grumpy;
                        angryOldManStats.realHumor -= 1;
                        break;
                }
                break;
        }

    }

    public void OnChangePeriod(int periodo)
    {
        currentIdentity = (Identity)Random.Range(0, 2);
        identityDistortion += 0.1f;

        Vector3 locationObject = GameManager._placePosition[GetRoutinePosition(periodo)];

        if (locationObject != null)
        {
            targetPosition = locationObject;
        }

    }
}
