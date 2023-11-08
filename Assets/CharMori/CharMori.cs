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
    [Range(0, 20)] public int morphAbility = 20;
    readonly private int maxAbilityCount = 20;

    [Header("-----Real Values-----"), Space]
    public GenderT realGender;
    public uint realAge;
    public RaceT realRace;
    public MoneyT realMoney;
    [Range(-3, 3)] public float realHumor;
    public PersonalityT realPersona;

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
    void Start()
    {

        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        if (morphAbility > 0)
        {
            //Try get other npc Sprite Info
            if (RollForSuccessChance())
            {
                spriteRenderer.sprite = charInfo.GetComponent<SpriteRenderer>().sprite;
                spriteRenderer.color = new Color(0.5f, 0, 1);
            }
            morphAbility--;
            CheckIfAbilityIsLowCharge();
        }
        else
        {

        }
    }
    private bool RollForSuccessChance()
    {
        return Random.value > morphAbility / maxAbilityCount;
    }
    private float GetSuccessChance()
    {
        return morphAbility / maxAbilityCount;
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

        //Define gender
        if (RollForSuccessChance()) gender = fakeGender;
        else gender = realGender;

        //Define age
        if (RollForSuccessChance()) age = fakeAge;
        else age = (uint)Mathf.RoundToInt(Mathf.Lerp(fakeAge, realAge, GetSuccessChance() * (Random.value * 0.75f)));

        //Define race
        

        //Define money


        //Define humor


        //Define persona
    }
    private void CheckIfAbilityIsLowCharge()
    {
        if (morphAbility <= 0)
        {
            if (RestoreAbilityCoroutine == null) RestoreAbilityCoroutine = StartCoroutine(RestoreAbility());
        }
    }
    Coroutine RestoreAbilityCoroutine;
    private IEnumerator RestoreAbility()
    {
        yield return new WaitForSeconds(5f);
        morphAbility = maxAbilityCount;
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
