using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Char_Christian : CharBase
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

    //Para implementacao simples mexer apenas a baixo

    /*
    Nome dos lugares no mapa:
    TownSquare
    Bakery
    Bar
    Library
    Hospital
    ?
    */
    [Range(0, 3)] public float drunk;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Para colocar uma interacao especifica entre alguns personagens
        //Use o modelo a baixo
        // if (collision.TryGetComponent<"Nome da classe do outro npc especifico">(out "Nome da classe do outro npc especifico" "Como vc pretende chamar a classe desse npc"))
        //Exemplo:
        //if (collision.TryGetComponent<CharTemplate>(out CharTemplate charTemplate))
        //Lembrar de colocar o else apos o } se tiver um if apos
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
        else
        {
            //Reacao do npc baseado na posicao de mundo
            switch (collision.gameObject.name)
            {
                case "TownSquare":
                    break;
                case "Bakery":
                    break;
                case "Bar":
                    break;
                case "Library":
                    break;
                case "Hospital":
                    break;
                case "?":
                    break;
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
        if (charInfo.Age > 40)
        {
            humor++;
        }
        else if (charInfo.Age <= 40 && charInfo.Age < 100)
        {
            humor--;
        }
        else
        {
            humor = 0;
        }

        switch (charInfo.Money)
        {


            case MoneyT.Poor:

                humor -= 2;

                persona = PersonalityT.Sadistic;
                break;

            case MoneyT.Medium:

                if (persona == PersonalityT.Kind)
                {

                    money = MoneyT.Rich;
                }
                else if (persona == PersonalityT.Grumpy || persona == PersonalityT.Sadistic)
                {
                    humor--;
                    //charInfo.Money = MoneyT.Poor;
                    money = MoneyT.Rich;
                }
                else
                {
                    humor--;
                    //charInfo.money = Money.Poor;
                    money = MoneyT.Rich;
                }
                break;

            case MoneyT.Rich:
                if (persona == PersonalityT.Grumpy || persona == PersonalityT.Sadistic)
                {
                    humor--;
                    //charInfo.money = Money.Medium;
                    money = MoneyT.Rich;
                }
                else
                {
                    humor += 2;
                    persona = PersonalityT.Kind;
                }
                break;
        }


        switch (charInfo.Persona)
        {


            case PersonalityT.Loud:

                persona = PersonalityT.Grumpy;

                break;

            case PersonalityT.Shy:

                persona = PersonalityT.Sadistic;

                break;

            case PersonalityT.Sadistic:

                persona = PersonalityT.Flirty;

                break;

            case PersonalityT.Kind:

                persona = PersonalityT.Loud;
                money = MoneyT.Rich;

                break;

        }

        if (money == MoneyT.Poor)
        {
            //charInfo.money = Money.Poor;
            money = MoneyT.Rich;
            humor = 0;
        }

        switch (charInfo.Race)
        {

            case RaceT.Animal:

                humor++;

                break;

            case RaceT.Human:

                if (persona == PersonalityT.Grumpy || persona == PersonalityT.Sadistic || persona == PersonalityT.Loud)
                {
                    humor--;
                }
                else if (persona == PersonalityT.Flirty)
                {
                    humor++;
                }

                break;

            case RaceT.NonHuman:

                if (persona == PersonalityT.Kind || persona == PersonalityT.Flirty)
                {
                    humor++;
                }
                else if ( persona == PersonalityT.Grumpy || persona == PersonalityT.Sadistic)
                {
                    humor--;
                }
                break;


        }
        if (Humor == -3)
        {
            targetPosition = GameObject.Find("Bar").transform.position;
            money = MoneyT.Poor;
            drunk = 3;
        }
    }

    public void OnChangePeriod(int periodo)
    {
        drunk--;
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
