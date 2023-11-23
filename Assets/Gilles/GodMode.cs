using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GodMode: CharBase
{
    public GameObject controle;
    private Dictionary<int, string> periodToLocation = new Dictionary<int, string>();
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 1f;
    public GameObject[] Personagens;
    public int Valor;
    public RaceT raca;
    private void Update()
    {
        //Nao precisa mexer
        if (Input.GetKeyDown(KeyCode.P))
        {
            controle.GetComponent<Animator>().SetTrigger("Appear");
            Repasse();

        }
        
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
   
    
    void Start()
    {
        
        
        AdicionarARotina(0, "TownSquare");
        AdicionarARotina(1, "TownSquare");
        AdicionarARotina(2, "TownSquare");
        AdicionarARotina(3, "TownSquare");
        AdicionarARotina(4, "TownSquare");
        AdicionarARotina(5, "TownSquare");
        AdicionarARotina(6, "TownSquare");
        Valor = Random.Range(0, Personagens.Length);

        Personagens = GameObject.FindGameObjectsWithTag("Char");
        //Nao mexer na linha a baixo
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }

   
    public void Repasse()
    {
        foreach (GameObject objet in Personagens)
        {
            Valor = Random.Range(0, Personagens.Length);
            raca = (RaceT)Random.Range(0, 3);
            Personagens[Valor].GetComponent<CharBase>().Race = raca;
            Personagens[Valor].GetComponent<CharBase>().Age = (uint)Random.Range(0, 100);
            Personagens[Valor].GetComponent<CharBase>().Gender = (GenderT)Random.Range(0, 2);
            Personagens[Valor].GetComponent<CharBase>().Money = (MoneyT)Random.Range(0, 2);
            Personagens[Valor].GetComponent<CharBase>().Persona = (PersonalityT)Random.Range(0, 5);
        }

        
       

    }
    public override void Interact(CharBase charInfo)
    {
       
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
