using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Char_Cesar : CharBase
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
                AoInteragirComPersonagem(collision.gameObject.name);
            }
            else
            {
                Debug.Log("Erro em pegar informacoes de" + collision.gameObject.name);
            }
        }
        else
        {
            AoChegarEmLocal(collision.gameObject.name);
        }

    }
    void Start()
    {
        AdicionarARotina(0, "Bakery");
        AdicionarARotina(1, "Bar");
        AdicionarARotina(2, "TownSquare");
        AdicionarARotina(3, "Library");
        AdicionarARotina(4, "Bar");

        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        if (charInfo.Persona == PersonalityT.Flirty)
        {
            humor += 1;
        }
    }

    public void OnChangePeriod(int periodo)
    {
        periodo = UnityEngine.Random.Range(0, GameManager._placePosition.Count - 1);
        //if (periodToLocation.ContainsKey(periodo))
        //{
        Vector3 locationObject = GameManager._placePosition.Values.ToArray()[periodo];

        if (locationObject != null)
        {
            if (GameManager._placePosition.ContainsKey("Cassino")) targetPosition = GameManager._placePosition["Cassino"];
            else targetPosition = locationObject;
        }
        //}
    }

    private void AoInteragirComPersonagem(string nome)
    {
        switch (nome)
        {
            case "Bily":
                targetPosition = GameManager._placePosition["Hospital"];
                GameManager.onChangePeriod.RemoveListener(OnChangePeriod);
                break;
            default:
                break;
        }
    }

    private void AoChegarEmLocal(string nome)
    {
        switch (nome)
        {
            case "Cassino":
                GameManager.onChangePeriod.RemoveListener(OnChangePeriod);
                break;
            default:
                break;
        }
    }
}
