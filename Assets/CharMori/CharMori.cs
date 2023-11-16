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
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

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
    public Sprite originalSprite;
    [Range(0, 10)] public int socialMaskingEnergy = 10;
    readonly private int maxSocialMaskingEnergyCount = 10;

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
    [HideInInspector] public IdentityStats fakeStats;

    private IdentityStats finalStats
    {
        get { return finalStats; }
        set
        {
            humor = finalStats.realHumor;
            age = finalStats.realAge;
            race = finalStats.realRace;
            persona = finalStats.realPersona;
            money = finalStats.realMoney;
            gender = finalStats.realGender;
        }
    }

    public float identityDistortion
    {
        get { return identityDistortion; }
        set
        {
            if (value > 1)
            {
                identityDistortion = 1;
            }
            else identityDistortion = value;
        }
    }

    [HideInInspector] public SpriteRenderer spriteRenderer;

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
                    case 1:
                        return "TownSquare";
                    case 2:
                    case 3:
                        return "Bakery";
                    case 4:
                    case 5:
                    case 6:
                    default:
                        return "Library";
                }
            case Identity.CrazyWoman:
                switch (period)
                {
                    case 0:
                    case 1:
                        return "Bakery";
                    case 2:
                    case 3:
                        return "?";
                    case 4:
                    case 5:
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

    public ref IdentityStats GetCurrentIdentityStats()
    {
        if (Random.value < identityDistortion)
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
        else
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                default:
                    return ref shyKidStats;

                case 1:
                    return ref crazyWomanStats;

                case 2:
                    return ref angryOldManStats;
                case 3:
                    return ref fakeStats;
            }
        }
    }

    void Start()
    {
        currentIdentity = (Identity)Random.Range(0, 2);
        finalStats = GetCurrentIdentityStats();
        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        if (socialMaskingEnergy > 0)
        {
            DefineCharValues(charInfo);
            identityDistortion += 0.1f;
            socialMaskingEnergy--;
        }
        CheckIfAbilityIsLowCharge();
        ReactToOtherNpc(charInfo);
    }
    public void ReactToOtherNpc(CharBase charInfo)
    {
        if (identityDistortion < 0.75f)
        {
            switch (currentIdentity)
            {
                case Identity.ShyKid:
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
                    if (charInfo.Age < 15 || charInfo.Age > 25)
                    {
                        shyKidStats.realHumor += 1;
                    }
                    else
                    {
                        shyKidStats.realHumor -= 1;
                    }
                    break;
                case Identity.CrazyWoman:
                    switch (charInfo.Persona)
                    {
                        case PersonalityT.Loud:
                        case PersonalityT.Sadistic:
                        case PersonalityT.Shy:
                            crazyWomanStats.realHumor -= 1;
                            break;
                    }
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
                    break;
                case Identity.AngryOldMan:
                    if (charInfo.Age >= 65)
                    {
                        angryOldManStats.realHumor += 1;
                    }
                    if (charInfo.Gender == GenderT.Other)
                    {
                        angryOldManStats.realPersona = PersonalityT.Grumpy;
                        angryOldManStats.realHumor -= 1;
                    }
                    break;
            }

        }
    }

    private bool RollForSuccessChance()
    {
        return Random.value > socialMaskingEnergy / maxSocialMaskingEnergyCount;
    }
    private void DefineCharValues(CharBase otherChar)
    {
        if (RollForSuccessChance()) fakeStats.realHumor = otherChar.Humor;
        else fakeStats.realHumor = humor;

        if (RollForSuccessChance()) fakeStats.realGender = otherChar.Gender;
        else fakeStats.realGender = gender;

        if (RollForSuccessChance()) fakeStats.realPersona = otherChar.Persona;
        else fakeStats.realPersona = persona;

        if (RollForSuccessChance()) fakeStats.realAge = otherChar.Age;
        else fakeStats.realAge = age;

        if (RollForSuccessChance()) fakeStats.realMoney = otherChar.Money;
        else fakeStats.realMoney = money;
        //Try get other sprite 
        if (RollForSuccessChance())
        {
            spriteRenderer.sprite = otherChar.GetComponent<SpriteRenderer>().sprite;
            spriteRenderer.color = new Color(0.5f, 0, 1);
        }
        else
        {
            spriteRenderer.sprite = originalSprite;
            spriteRenderer.color = Color.white;
        }
        fakeStats.realHumor = GetCurrentIdentityStats().realHumor;
        fakeStats.realGender = GetCurrentIdentityStats().realGender;
        fakeStats.realPersona = GetCurrentIdentityStats().realPersona;
        fakeStats.realAge = GetCurrentIdentityStats().realAge;
        fakeStats.realMoney = GetCurrentIdentityStats().realMoney;
    }
    private void CheckIfAbilityIsLowCharge()
    {
        if (socialMaskingEnergy <= 0)
        {
            if (RestoreAbilityCoroutine == null) RestoreAbilityCoroutine = StartCoroutine(RestoreAbility());
        }
    }
    Coroutine RestoreAbilityCoroutine;
    private IEnumerator RestoreAbility()
    {
        yield return new WaitForSeconds(10f);
        socialMaskingEnergy = maxSocialMaskingEnergyCount;
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
