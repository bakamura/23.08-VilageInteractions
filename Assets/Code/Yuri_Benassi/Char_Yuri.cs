using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Char_Yuri : CharBase
{
    private Dictionary<int, string> periodToLocation = new Dictionary<int, string>();
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 5f;
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
    [Range(0, 1)] public int lobisomem;
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Char")
        {
            //Para colocar uma interacao especifica entre alguns personagens
            //Use o modelo a baixo
            // if (collision.TryGetComponent<"Nome da classe do outro npc especifico">(out "Nome da classe do outro npc especifico" "Como vc pretende chamar a classe desse npc"))
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

                    if (lobisomem > 0)
                    {
                        persona = PersonalityT.Loud;
                        money--;
                    }
                    else
                    {
                        persona = PersonalityT.Kind;
                        money++;
                    }
                    break;

                case "Bakery":

                    money = MoneyT.Poor;
                    if (lobisomem > 0)
                    {
                        persona = PersonalityT.Loud;
                    }
                    else
                    {
                        persona = PersonalityT.Shy;
                    }
                    break;

                case "Bar":

                    money = MoneyT.Poor;
                    if (lobisomem > 0)
                    {
                        persona = PersonalityT.Grumpy;
                    }
                    else
                    {
                        persona = PersonalityT.Kind;
                    }
                    break;

                case "Library":

                    if (lobisomem > 0)
                    {
                        persona = PersonalityT.Sadistic;
                    }
                    else
                    {
                        persona = PersonalityT.Shy;
                    }
                    break;

                case "Hospital":

                    if (lobisomem > 0)
                    {
                        persona = PersonalityT.Grumpy;
                    }
                    else
                    {
                        persona = PersonalityT.Shy;
                    }
                    break;

                case "?":

                    break;
            }
        }
    }
    void Start()
    {
        AdicionarARotina(0, "Bakery");
        AdicionarARotina(1, "TownSquare");
        AdicionarARotina(2, "TownSquare");
        AdicionarARotina(3, "Hospital");
        AdicionarARotina(4, "TownSquare");
        AdicionarARotina(5, "Library");
        AdicionarARotina(6, "Bar");

        //Nao mexer na linha a baixo 
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        switch (charInfo.Persona)
        {
            case PersonalityT.Loud:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Grumpy;
                }
                else
                {
                    persona = PersonalityT.Sadistic;
                }
                break;

            case PersonalityT.Shy:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Loud;
                }
                else
                {
                    persona = PersonalityT.Kind;
                }
                break;

            case PersonalityT.Sadistic:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Loud;
                }
                else
                {
                    persona = PersonalityT.Kind;
                }
                break;

            case PersonalityT.Grumpy:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Grumpy;
                }
                else
                {
                    persona = PersonalityT.Shy;
                }
                break;

            case PersonalityT.Kind:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Loud;
                }
                else
                {
                    persona = PersonalityT.Shy;
                }
                break;

            case PersonalityT.Flirty:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Grumpy;
                }
                else
                {
                    persona = PersonalityT.Flirty;
                }
                break;
        }
        switch (charInfo.Race)
        {
            case RaceT.Animal:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Kind;
                    humor++;
                }
                else
                {
                    persona = PersonalityT.Shy;
                    humor++;
                }
                break;

            case RaceT.Human:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Grumpy;
                    humor--;
                }
                else
                {
                    persona = PersonalityT.Shy;
                    humor++;
                }
                break;

            case RaceT.Spirit:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Kind;
                    humor++;
                }
                else
                {
                    persona = PersonalityT.Sadistic;
                    humor--;
                }
                break;

            case RaceT.NonHuman:

                if (lobisomem > 0)
                {
                    persona = PersonalityT.Kind;
                    humor++;
                }
                else
                {
                    persona = PersonalityT.Shy;
                    humor++;
                }
                break;
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
