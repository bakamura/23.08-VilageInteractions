using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_GuardaSonolento_V1 : CharBase
{
    private Dictionary<int, string> periodToLocation = new Dictionary<int, string>();
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 1f;
    private void Update()
    {
        //Nao precisa mexer
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed/10 * Time.deltaTime);
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
        AdicionarARotina(0, "TownSquare");
        AdicionarARotina(1, "Bakery");
        AdicionarARotina(2, "Library");
        AdicionarARotina(3, "TownSquare");
        AdicionarARotina(4, "Bar");
        AdicionarARotina(5, "TownSquare");

        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        switch(charInfo.Persona)
        {
            case PersonalityT.Shy:
            humor += 1;
            break;

            case PersonalityT.Kind:
            humor += 1;
            break;

            case PersonalityT.Sado:
            humor -= 1;
            break;

            case PersonalityT.Loud:
            humor -= 2;
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
