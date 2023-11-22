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
    public float radius = 5f;
    public Color HotColor = Color.magenta;
    public Color ColdColor = Color.blue;
    public Color defaultColor = Color.white;

    public float currentHumor;



    private void Update()
    {
        //Nao precisa mexer
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed/10 * Time.deltaTime);

        GetRandomColorFromHumor(currentHumor);

        // Verifica se a distância entre a posição atual e a posição alvo é menor que uma pequena tolerância
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            isMoving = true;
            anim.Play("DragAndar");
            Debug.Log("anda");
        }
        else
        {
            isMoving = false;
            anim.Play("DragDancaAtraente");
            Debug.Log("dance");
        }

        // Obtém a posição do collider do objeto
        Vector3 objectPosition = transform.position;
    }

    private Color GetRandomColorFromHumor(float humorValue)
    {
        Color coldColor = ColdColor;
        Color hotColor = HotColor;

        if (humorValue <= 0)
        {
            return coldColor;
        }
        else if (humorValue >= 1)
        {
            return hotColor;
        }
        else
        {
            // Interpolação linear entre as cores neutras
            float t = (humorValue + 3f) / 6f; // Normaliza o valor entre 0 e 1
            return Color.Lerp(coldColor, hotColor, t);
        }
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
                if (collision.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    currentHumor = charBase.Humor;
                    spriteRenderer.color = GetRandomColorFromHumor(currentHumor);
                    if (currentHumor <= 0)
                    {
                        spriteRenderer.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
                    }
                }

                charBase.Interact(this);
            }
            else
            {

                Debug.Log("Erro em pegar informacoes de" + collision.gameObject.name);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CharBase>(out CharBase charBase))
        {
            if (collision.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                currentHumor = charBase.Humor;
                spriteRenderer.color = defaultColor;

                if (currentHumor <= 0)
                {
                    spriteRenderer.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
                }
            }

            charBase.Interact(this);
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
        currentHumor = charInfo.Humor;
        Debug.Log("interação");


        if (charInfo.Gender == GenderT.Male)
        {
            Persona = PersonalityT.Flirty; 
        }
        
        switch (Persona)
        {
            case PersonalityT.Loud:

                if (!isMoving)
                {
                    anim.Play("DragDancaAtraente");
                    DragRenderer.color = new Color(255, 255, 255, 255);
                }
                break;

            case PersonalityT.Flirty:

                    //Anim.Play("Dominatrix");
                    DragRenderer.color = new Color(202, 207, 50, 255);
                
                break;
        }

        switch (humor)
        {
            case -3:
                if(humor >= -3 && humor <= 0)
                {
                    Persona = PersonalityT.Flirty; 
                }
                break;

            case 3:
                if (humor >= 3 && humor <= 0)
                {
                    Persona = PersonalityT.Loud;
                }
                break;
        }
        
        switch (charInfo.Persona)
        {
            //caso a personalidade da IA de interação seja negativa, a Drag se torna mais forte
            case PersonalityT.Sadistic:
            case PersonalityT.Grumpy:

                humor++;
                transform.localScale += new Vector3 (0.2f,0.2f,0.2f);
                break;


            case PersonalityT.Kind:

                humor +=3;
                break;


            case PersonalityT.Shy:
            case PersonalityT.Flirty:

                humor--;
                transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
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
