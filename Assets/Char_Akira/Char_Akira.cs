using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Char_Akira : CharBase
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
                    humor--;
                    break;

                case "Bakery":
                    humor = 0;
                    break;

                case "Bar":
                    humor++;
                    money = MoneyT.Poor;
                    break;

                case "Library":
                    humor++;
                    money = MoneyT.Rich;
                    break;

                case "Hospital":
                    humor--;
                    break;

                case "?":

                    break;
            }
        }
    }
    void Start()
    {

        AdicionarARotina(0, "Library");
        AdicionarARotina(1, "Bakery");
        AdicionarARotina(2, "Library");
        AdicionarARotina(3, "Hospital");
        AdicionarARotina(4, "Bar");
        AdicionarARotina(5, "TownSquare");
        AdicionarARotina(6, "Library");

        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {

        switch (charInfo.Money)
        {

            case MoneyT.Poor:
                humor++;
                persona = PersonalityT.Kind;

                break;

            case MoneyT.Medium:
                humor += 0;
                persona = PersonalityT.Shy;

                break;

            case MoneyT.Rich:
                humor--;
                persona = PersonalityT.Grumpy;
                if (targetPosition == GameObject.Find("Bar").transform.position)
                {
                    targetPosition = GameObject.Find("Outro").transform.position;
                }
                else
                {
                    targetPosition = GameObject.Find("Bar").transform.position;
                }

                break;


        }

        switch (charInfo.Gender)
        {

            case GenderT.Female:
                humor++;
                persona = PersonalityT.Flirty;

                break;

            case GenderT.Male:
                humor++;
                persona = PersonalityT.Flirty;

                break;

            case GenderT.Other:
                humor++;
                persona = PersonalityT.Flirty;

                break;
        }

        switch (charInfo.Persona)
        {

            case PersonalityT.Flirty:

                humor++;
                persona = PersonalityT.Flirty;
                break;

            case PersonalityT.Shy:

                humor--;
                persona = PersonalityT.Grumpy;
                break;

            case PersonalityT.Loud:

                humor--;
                persona = PersonalityT.Sadistic;
                break;

            case PersonalityT.Sadistic:

                humor = 0;
                persona = PersonalityT.Sadistic;
                break;

            case PersonalityT.Kind:

                humor++;
                persona = PersonalityT.Kind;
                break;

            case PersonalityT.Grumpy:

                humor = 0;
                persona = PersonalityT.Loud;
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
