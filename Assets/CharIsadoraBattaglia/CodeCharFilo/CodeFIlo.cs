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
        AdicionarARotina(0, "Bakery");
        AdicionarARotina(2, "TownSquare");
        AdicionarARotina(4, "Bakery");
        AdicionarARotina(5, "Bar");


        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    //colocar a�oes e personalidades especificas da filo: playful, sweet, bite, pee.
    public override void Interact(CharBase charInfo)
    {
        switch (charInfo.Persona)
        {
            case PersonalityT.Shy:
                humor += 1;
                break;

            case PersonalityT.Grumpy:
                humor -= 1;
                break;

            case PersonalityT.Kind:
                humor += 2;
                break;

            case PersonalityT.Sadistic:
                humor -= 2;
                break;
        }

        switch (charInfo.Race)
        {
            case RaceT.Human:
                humor += 1;
                break;

            case RaceT.Animal:
                humor += 2;
                break;

            case RaceT.Spirit:
                humor -= 1;
                break;

            case RaceT.NonHuman:
                humor -= 1;
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
