using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeFilo : CharBase
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
            //Variações do eu personagem dentro de cada local da vila.
            switch (collision.gameObject.name)
            {
                case "TownSquare":
                    humor += 3;
                    persona = PersonalityT.Loud;
                    Playfull = 1;
                    break;

                case "Bakery":
                    persona = PersonalityT.Shy;
                    break;

                case "Bar":
                    humor -= 2;
                    Sad += 0.75f;
                    break;

                case "Library":
                    Sleepy += 3;
                    break;

                case "Hospital":
                    persona = PersonalityT.Shy;
                    Sad += 2;
                    break;

                case "?":
                    Confused += 3;
                    humor -= 0.5f;
                    break;
            }
        }
    }
    void Start()
    {
        //Rota e Rotina do meu personagem, com variação. cada rotina ele vai mudar (50%), ou ele vai para a padaria ou para o hospital e por ai vai.
        //Momento 0
        RadnomAddToRoutine(0, 0.5f, "Bakery", "Hospital");
        //Momento 1
        RadnomAddToRoutine(1, 0.5f, "TownSquare", "Library");
        //Momento 2
        RadnomAddToRoutine(2, 0.5f, "?", "Bar");
        //Momento 3
        RadnomAddToRoutine(3, 0.5f, "Hospital", "Bakery");
        //Momento 4
        RadnomAddToRoutine(4, 0.5f, "Library", "?");
        //Momento 5
        RadnomAddToRoutine(5, 0.5f, "Bar", "TownSquare");
        //Momento 6
        RadnomAddToRoutine(6, 0.5f, "Bakery", "Hospital");

        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public void RadnomAddToRoutine(int period,float chance,string firstPlaceOption, string secondPlaceOption)
    {
        //redução da quantidade de if else dentro da variavel de rotina.
        if (Random.value > chance) AdicionarARotina(period, firstPlaceOption);
        else AdicionarARotina(period, secondPlaceOption);
    }
    //colocar açoes e personalidades especificas da filo: playful, sleepy, afraid, Angry, Sad e Confused.
    [Header ("--------PersonalityT Values-------"),Space(1)]
    [Range(-1, 1)] public int Playfull;
    [Range(-3, 3)] public float Sleepy;
    [Range(-3, 3)] public float Afraid;
    [Range(-3, 3)] public float Angry;
    [Range(-3, 3)] public float Sad;
    [Range(-3, 3)] public float Confused;
    public override void Interact(CharBase charInfo)
    {

        //Variações do meu personagem ao interagir com pessoas com essas personalidaes.
        switch (charInfo.Persona)
        {
            case PersonalityT.Shy:
                humor += 1;
                Playfull = 0;
                Sleepy += 1.5f;
                persona = PersonalityT.Shy;
                moveSpeed -= 0.5f;
                break;

            case PersonalityT.Grumpy:
                humor -= 1;
                Playfull = -1;
                Afraid += 0.5f;
                persona = PersonalityT.Grumpy;
                break;

            case PersonalityT.Kind:
                humor += 2;
                Afraid -= 1.25f;
                break;

            case PersonalityT.Sadistic:
                humor -= 2;
                Afraid += 1.5f;
                Angry += 0.5f;
                persona = PersonalityT.Grumpy;
                moveSpeed += 1.5f;
                Confused += 1;
                break;

            case PersonalityT.Loud:
                Sleepy -= 2.5f;
                Playfull = 1;
                persona = PersonalityT.Loud;
                break;
        }
        //Variações do meu persoangem perante as raças dos outros personagens.
        switch (charInfo.Race)
        {
            case RaceT.Human:
                humor += 1;
                break;

            case RaceT.Animal:
                humor += 2;
                Playfull = 1;
                persona = PersonalityT.Loud;
                break;

            case RaceT.Spirit:
                humor -= 1;
                Afraid += 1;
                moveSpeed += 1.25f;
                break;

            case RaceT.NonHuman:
                humor -= 1;
                break;
        }

        //Variações do meu personagem perante a variação de idade de outras personagens.
        if (charInfo.Age <= 18)
        {
            humor += 2.5f;
            Playfull = 1;
            moveSpeed += 0.5f;
            persona = PersonalityT.Loud;
        }
        else if (charInfo.Age <= 47)
        {
            humor += 1;
            Playfull = 0;
            moveSpeed += 1;
        }
        else
        {
            moveSpeed -= 1;
            Sleepy += 1.25f;
            Playfull -= 1;
            persona = PersonalityT.Shy;
        }

        //Variações do meu persoangem perante as variações de Humor de outras personagens.
        if (charInfo.Humor >= -3 && charInfo.Humor <= -1)
        {
            humor -= 0.5f;
            Playfull = -1;
            moveSpeed -= 0.75f;
            persona = PersonalityT.Shy;
            Sad += 0.5f;
            Confused += 0.25f;
        }
        else if (charInfo.Humor <= 2)
        {
            humor += 1;
            Playfull = 0;
            moveSpeed += 0.5f;
            persona = PersonalityT.Kind;
            Sad -= 0.75f;
        }
        else
        {
            humor += 1.75f;
            Playfull = 1;
            moveSpeed += 1;
            persona = PersonalityT.Loud;
            Sad -= 1.25f;
        }

        //variações dentro das personalidades do meu personagem.
        switch (persona)
        {
            case PersonalityT.Grumpy:
                Playfull = -1;
                Angry += 1;
                humor -= 0.75f;
                Sad += 0.25f;
                break;

            case PersonalityT.Loud:
                Playfull = +1;
                Angry -= 2;
                humor += 1.75f;
                Sad -= 2;
                moveSpeed += 1.25f;
                break;

            case PersonalityT.Shy:
                Playfull = 0;
                Sleepy += 0.25f;
                humor = 0;
                Afraid = 0.5f;
                moveSpeed -= 1;
                break;

            case PersonalityT.Kind:
                Playfull = +1;
                Sleepy = 0.25f;
                Afraid = -0.25f;
                Angry = 0;
                Sad = 0;
                humor = 1;
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

