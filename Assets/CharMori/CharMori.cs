using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMori : CharBase
{
    private Dictionary<int, string> periodToLocation = new Dictionary<int, string>();
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 1f;
    private void Update()
    {
        //Nao precisa mexer
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed / 10 * Time.deltaTime);
    }
    private void AdicionarARotina(int periodoDoDia, string lugar)
    {
        periodToLocation.Add(periodoDoDia, lugar);
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

    private enum Identity
    {
        ShyKid,
        CrazyWoman,
        AngryOldMan
    }
    private Identity currentIdentity;

    [Header("-----Real Values-----"), Space]
    [Header("-----Personality Shy Kid-----")]
    public GenderT realShyGender;
    public uint realShyAge;
    public RaceT realShyRace;
    public MoneyT realShyMoney;
    [Range(-3, 3)] public float realShyHumor;
    public PersonalityT realShyPersona;

    [Header("-----Personality Crazy Woman-----")]
    public GenderT realCrazyGender;
    public uint realCrazyAge;
    public RaceT realCrazyRace;
    public MoneyT realCrazyMoney;
    [Range(-3, 3)] public float realCrazyHumor;
    public PersonalityT realCrazyPersona;

    [Header("-----Personality Angry Old Guy-----")]
    public GenderT realAngryGender;
    public uint realAngryAge;
    public RaceT realAngryRace;
    public MoneyT realAngryMoney;
    [Range(-3, 3)] public float realAngryHumor;
    public PersonalityT realAngryPersona;

    [Header("-----Fake Values-----"), Space]
    public GenderT fakeGender;
    public uint fakeAge;
    public RaceT fakeRace;
    public MoneyT fakeMoney;
    [Range(-3, 3)] public float fakeHumor;
    public PersonalityT fakePersona;



    [HideInInspector] public SpriteRenderer spriteRenderer;
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

    public void GetRealCurrentIdentity(out GenderT realGender, out uint realAge, out RaceT realRace, out MoneyT realMoney, out float realHumor, out PersonalityT realPersona)
    {
        switch (currentIdentity)
        {
            case Identity.ShyKid:
            default:
                realGender = realShyGender;
                realAge = realShyAge;
                realRace = realShyRace;
                realMoney = realShyMoney;
                realHumor = realShyHumor;
                realPersona = realShyPersona;
                break;
            case Identity.CrazyWoman:
                realGender = realCrazyGender;
                realAge = realCrazyAge;
                realRace = realCrazyRace;
                realMoney = realCrazyMoney;
                realHumor = realCrazyHumor;
                realPersona = realCrazyPersona;
                break;
            case Identity.AngryOldMan:
                realGender = realAngryGender;
                realAge = realAngryAge;
                realRace = realAngryRace;
                realMoney = realAngryMoney;
                realHumor = realAngryHumor;
                realPersona = realAngryPersona;
                break;
        }
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
        if (socialMaskingEnergy > 0)
        {
            DefineCharValues(charInfo);
            socialMaskingEnergy--;
        }
        CheckIfAbilityIsLowCharge();
        ReactToOtherNpc(charInfo);
    }

    public void ReactToOtherNpc(CharBase charInfo)
    {
        switch(currentIdentity)
        {
            case Identity.ShyKid:
                switch(charInfo.Persona)
                {
                    case PersonalityT.Kind:
                    case PersonalityT.Shy:
                        realShyHumor += 1;
                        break;
                    default:
                        realShyHumor -= 1;
                        break;
                }
                //Continuar daqui
                break;
        }
    }

    private bool RollForSuccessChance()
    {
        return Random.value > socialMaskingEnergy / maxSocialMaskingEnergyCount;
    }
    private float GetSuccessChance()
    {
        return socialMaskingEnergy / maxSocialMaskingEnergyCount;
    }
    private void DefineCharValues(CharBase otherChar)
    {
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
        GetRealCurrentIdentity(out GenderT realGender, out uint realAge, out RaceT realRace, out MoneyT realMoney, out float realHumor, out PersonalityT realPersona);
        //Define gender
        if (RollForSuccessChance()) gender = fakeGender;
        else gender = realGender;

        //Define age
        if (RollForSuccessChance()) age = fakeAge;
        else age = (uint)Mathf.RoundToInt(Mathf.Lerp(realAge, fakeAge, GetSuccessChance() * (Random.value * 0.75f)));

        //Define race
        if (RollForSuccessChance()) race = fakeRace;
        else race = realRace;

        //Define money
        if (RollForSuccessChance()) money = fakeMoney;
        else money = realMoney;

        //Define humor
        if (RollForSuccessChance()) humor = fakeHumor;
        else humor = Mathf.Lerp(realHumor, fakeHumor, GetSuccessChance() * (Random.value * 0.75f));

        //Define persona
        if (RollForSuccessChance()) persona = fakePersona;
        else persona = realPersona;
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
        if (periodToLocation.ContainsKey(periodo))
        {
            Vector3 locationObject = GameManager._placePosition[periodToLocation[periodo]];

            if (locationObject != null)
            {
                targetPosition = locationObject;
            }
        }
    }
}
