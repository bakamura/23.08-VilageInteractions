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

                    break;
            }
        }
    }
    void Start()
    {
        AdicionarARotina(0, "Bakery");
        AdicionarARotina(2, "TownSquare");
        AdicionarARotina(4, "Bakery");
        AdicionarARotina(5, "Bar");


        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    //colocar a�oes e personalidades especificas da filo: playful, sleepy, afraid, Angry e Sad;
    [Range(-1, 1)] public int Playfull;
    [Range(-3, 3)] public float Sleepy;
    [Range(-3, 3)] public float Afraid;
    [Range(-3, 3)] public float Angry;
    [Range(-3, 3)] public float Sad;
    public override void Interact(CharBase charInfo)
    {
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
                break;

            case PersonalityT.Loud:
                Sleepy -= 2.5f;
                Playfull = 1;
                persona = PersonalityT.Loud;
                break;
        }

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

        if (charInfo.Age <= 18)
        {
            humor += 2.5f;
            Playfull = 1;
            moveSpeed = 0.5f;
            persona = PersonalityT.Loud;
        }
        else if (charInfo.Age >= 19 && charInfo.Age <= 35)
        {
            humor += 1;
        }
        else
        {

        }


        switch (persona)
        {
            case PersonalityT.Grumpy:

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
