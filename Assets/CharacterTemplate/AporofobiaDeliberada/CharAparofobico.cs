using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAparofobico : CharBase
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
        periodToLocation.Add(1, "TownSquare");
        periodToLocation.Add(2, "Bar");
        periodToLocation.Add(3, "Bar");
        periodToLocation.Add(4, "Bar");
        periodToLocation.Add(5, "Bakery");
        periodToLocation.Add(6, "Bakery");
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

        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        if (charInfo.Money == MoneyT.Poor)
        {
            Debug.Log("Evitando intera��o com pessoa pobre.");

            // Afasta o personagem 
            Vector3 direction = transform.position - charInfo.transform.position;
            Vector3 newPosition = transform.position + direction.normalized * 2f;  

           
            targetPosition = newPosition;
        }
        // Verifica se a pessoa � "rica" antes de interagir
        else if (charInfo.Money == MoneyT.Rich)
        {
            Debug.Log("Aproximando-se de pessoa rica.");

            // Aproxima o personagem ao detectar uma pessoa rica
            Vector3 direction = charInfo.transform.position - transform.position;
            Vector3 newPosition = transform.position + direction.normalized * 2f;  // Ajuste a magnitude conforme necess�rio

            // Atualiza a posi��o do personagem
            targetPosition = newPosition;
        }
        switch (charInfo.Persona)
        {
            case PersonalityT.Sadistic:
                humor -= 1;

                break;

            case PersonalityT.Grumpy:
                humor += 1;

                break;

            case PersonalityT.Loud:
                humor += 2;

                break;

            case PersonalityT.Shy:
                humor += 2;

                break;

            case PersonalityT.Kind:
                humor -= 1;

                break;

            case PersonalityT.Flirty:
                humor -= 1;

                break;

            default:
           
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

