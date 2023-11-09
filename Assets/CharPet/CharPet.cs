using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharPet : CharBase
{
    private Dictionary<int, string> periodToLocation = new Dictionary<int, string>();
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 1f;

    private bool isMoving = false;

    public SpriteRenderer DragRenderer;
    public Animator anim;


    
    private void Update()
    {
        //Nao precisa mexer
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed/10 * Time.deltaTime);

       
    }
    private void AdicionarARotina(int periodoDoDia, string lugar)
    {
        periodToLocation.Add(periodoDoDia, lugar);
        isMoving = true;
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
        AdicionarARotina(1, "Bar");
        AdicionarARotina(2, "TownSquare");
        AdicionarARotina(3, "Bar");
        AdicionarARotina(4, "TownSquare");
        AdicionarARotina(5, "Bar");

        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        if(charInfo.Gender == GenderT.Male || charInfo.Gender == GenderT.Other)
        {
            persona = PersonalityT.Flirty; 
        }

        else
        {
            persona = PersonalityT.Loud;
        }
        
        switch (persona)
        {
            case PersonalityT.Loud:

                if (!isMoving)
                {
                    //Anim.Play("Dance");
                    DragRenderer.color = new Color(255, 255, 255, 255);
                }
                break;

            case PersonalityT.Flirty:

                if (!isMoving)
                {
                    //Anim.Play("Dominatrix");
                    DragRenderer.color = new Color(202, 207, 50, 255);
                }
                break;
        }

        switch (humor)
        {
            case -3:
                if(humor >= -3 && humor <= 0)
                {
                    persona = PersonalityT.Flirty; 
                }
                break;

            case 3:
                if (humor >= 3 && humor <= 0)
                {
                    persona = PersonalityT.Flirty;
                }
                break;
        }
        
        switch (charInfo.Persona)
        {
            //caso a personalidade da IA de interação seja negativa, a Drag se torna mais forte
            case PersonalityT.Sadistic:
            case PersonalityT.Grumpy:

                humor++;
                transform.localScale += new Vector3 (1,1,1);
                break;


            case PersonalityT.Kind:

                humor +=3;
                break;


            case PersonalityT.Shy:
            case PersonalityT.Flirty:

                humor--;
                transform.localScale -= new Vector3(1, 1, 1);
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
