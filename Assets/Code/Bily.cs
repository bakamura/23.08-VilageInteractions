using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bily : CharBase
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

        AdicionarARotina(0, "");
        AdicionarARotina(1, "");
        AdicionarARotina(2, "");
        AdicionarARotina(3, "");
        AdicionarARotina(4, "");
        AdicionarARotina(5, "");
        AdicionarARotina(6, "");



        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        if (charInfo.Money > MoneyT.Poor)
        {
            if (money < MoneyT.Rich) money++;
            charInfo.Money--;
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
