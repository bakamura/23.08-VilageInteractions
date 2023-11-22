using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FelipeMoraesChar : CharBase
{
    private Dictionary<int, string> periodToLocation = new Dictionary<int, string>();
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 5f;
    string lugar;
    /*
    Nome dos lugares no mapa:
    TownSquare
    Bakery
    Bar
    Library
    Hospital
    ?
    */

    private void Update()
    {
        //Nao precisa mexer
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed / 10 * Time.deltaTime);
    }
    private void AdicionarARotina(int periodoDoDia, string lugar)
    {
        periodToLocation.Add(periodoDoDia, lugar);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Char")
        {
            if (collision.TryGetComponent<CharTemplate>(out CharTemplate charTemplate))
            {
                //Aqui pode acessar as variaveis unicas daquele npc
            }
            // Colocar apos o {} "else"
            else if (collision.TryGetComponent<CharBase>(out CharBase charBase))
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
                    humor = 2;
                    persona = PersonalityT.Loud;
                    lugar = "TownSquare";
                    break;
                case "Bakery":
                    humor = 3;
                    persona = PersonalityT.Loud;
                    money = MoneyT.Medium;
                    lugar = "Bakery";
                    break;
                case "Bar":
                    lugar = "Bar";
                    break;
                case "Library":
                    persona = PersonalityT.Shy;
                    humor = 0;
                    lugar = "Library";
                    break;
                case "Hospital":
                    lugar = "Hospital";
                    break;
                case "?":
                    humor = 3;
                    persona = PersonalityT.Grumpy;
                    lugar = "?";
                    break;
            }
        }
    }

    void Start()
    {
        gender = GenderT.Male;
        race = RaceT.Human;
        age = 30;
        money = MoneyT.Poor;
        humor = 1;
        persona = PersonalityT.Kind;

        AdicionarARotina(0, "?");
        AdicionarARotina(1, "TownSquare");
        AdicionarARotina(2, "Bakery");
        AdicionarARotina(3, "Bakery");
        AdicionarARotina(4, "TownSquare");
        AdicionarARotina(5, "Library");
        AdicionarARotina(6, "?");

        //Nao mexer na linha a baixo 
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        
        if(charInfo.Race == RaceT.Human)
        {
            persona = PersonalityT.Sadistic;
            humor = -3;
        }
        else if(charInfo.Race == RaceT.NonHuman && charInfo.Gender == GenderT.Male && charInfo.Persona == PersonalityT.Shy && charInfo.Age <= 90)
        {
            persona = PersonalityT.Loud;
            humor = -1;
        }
        else if(charInfo.Race == RaceT.NonHuman && charInfo.Gender == GenderT.Male && charInfo.Persona == PersonalityT.Loud && charInfo.Age >= 100 && charInfo.Money == MoneyT.Rich)
        {
            persona = PersonalityT.Shy;
            humor = 3;
        }
        else if(charInfo.Race == RaceT.Human && lugar == "Bakery")
        {
            persona = PersonalityT.Loud;
            humor = -3;
        }
        else if(charInfo.Race == RaceT.Human && lugar == "Library")
        {
            persona = PersonalityT.Loud;
            humor = -3;
        }
        else
        {

        }

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
