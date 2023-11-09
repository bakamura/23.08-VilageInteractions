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

    private void FixedUpdate()
    {
        if (tired == 3)
        {
        targetPosition = collision.gameObject.name.TownSquare;
        }
    }

    [SerializeField, Range(-3, 3)] protected float tired;
    public float Tired { get { return tired; } }
    //relacionado ao sono

    public void OnTriggerEnter2D(Collider2D collision)
    {
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
                    tired += 1;
                    break;
                case "Bakery":
                    break;
                case "Bar":
                    tired -= 1;
                    humor -= 1;
                    break;
                case "Library":
                    tired += 1;
                    humor += 1;
                    break;
                case "Hospital":
                    tired += 1;
                    humor -= 1;
                    break;
                case "?":
                    break;
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
        AdicionarARotina(5, "Hospital");
        AdicionarARotina(6, "TownSquare");


        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        if (charInfo.gender == GenderT.Female)
        {
            persona = PersonalityT.Shy;
        } 
        switch(charInfo.Persona)
        {
            case PersonalityT.Shy:
            humor += 1;
            tired += 1;
            break;

            case PersonalityT.Kind:
            humor += 1;
            tired -= 1;
            break;

            case PersonalityT.Sadistic:
            humor -= 1;
            tired += 1;
            break;

            case PersonalityT.Loud:
            humor -= 2;
            tired -= 1;
                if (humor < -1) 
                {
                    persona = PersonalityT.Loud;
                }
                if (charInfo.targetPosition == GameObject.Find("Library").transform.position)
                {
                    targetPosition = GameObject.Find("?").transform.position;
                }
                else
                {
                    targetPosition = GameObject.Find("Library").transform.position;
                }
            break;
            //se encontrar com algm Loud, o guarda tentará fugir em direção à biblioteca. se o humor estiver abaixo de -1, ele se torna Loud.
        }
        //interações relacionadas ao fator de Persona

        
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
