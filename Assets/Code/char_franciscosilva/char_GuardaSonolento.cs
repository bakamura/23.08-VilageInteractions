using System.Collections.Generic;
using UnityEngine;

public class char_GuardaSonolento : MonoBehaviour
{
    public float moveSpeed;

    public Vector3 targetPosition;
    public GameObject gamemanager;
    private Timer timer;
    public List<GameObject> lugares;
    private Dictionary<int, string> periodToLocation = new Dictionary<int, string>();

    public int Age;
    public enum Gender { Male, Female, Neutro }
    public enum Money { Poor, Medium, Rich }
    public enum Persona { Grumpy, Shy, Kind, Flirty, Sado, Loud }
    public enum Race { Dog, Elemental, Boto, Human }
    public float Humor;
    public Gender gender;
    public Money money; 
    public Persona persona;
    public Race race;

//----------------------------------------------------


    private void Start()
    {
       gamemanager = GameObject.FindGameObjectWithTag("Horario");
        timer = gamemanager.GetComponent<Timer>();
        lugares = timer.Lugares;

        
        periodToLocation.Add(0, "Square");
        periodToLocation.Add(1, "Padaria");
        periodToLocation.Add(2, "Biblioteca");
        periodToLocation.Add(3, "Square");
        periodToLocation.Add(4, "Bar");
        periodToLocation.Add(5, "Square");
        

        
        int initialPeriod = timer.GetCurrentPeriod(); 
        ChangeLocation(initialPeriod);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void MudaLugar(int periodo)
    {
        ChangeLocation(periodo);
    }

    private void ChangeLocation(int periodo)
    {
        
        if (periodToLocation.ContainsKey(periodo))
        {
            string locationName = periodToLocation[periodo];
            GameObject locationObject = lugares.Find(lugar => lugar.name == locationName);

            if (locationObject != null)
            {
                targetPosition = timer.positions[lugares.IndexOf(locationObject)];
            }
            else
            {
                Debug.LogWarning("Location not found: " + locationName);
            }
        }
        else
        {
            Debug.LogWarning("Period not found: " + periodo);
        }
    }

    void OnTriggerEnter2D(Collider2D other){

        Relacoes(other.gameObject);

        if(other.gameObject.tag == "Char"){
            Interagir();
            Debug.Log("Pegar Valores");
        }
        Debug.Log("Chegou");
    }

    public void Interagir(){

    }

    void Relacoes(GameObject other){

        Humor += ((other.GetComponent<char_GuardaSonolento>().Age) > 50) ? 0.5f : 0;
        //age check. if higher than 50, it gains 0.1, otherwise nothing changes
        
        switch (other.GetComponent<char_GuardaSonolento>().persona)
        {
            case Persona.Loud:
            Humor -= 1;
                if (targetPosition == GameObject.Find("Biblioteca").transform.position){
                    targetPosition = GameObject.Find("Outro").transform.position;
                    }
                else{
                    targetPosition = GameObject.Find("Biblioteca").transform.position;
                    }
            break;

            case Persona.Shy:
            Humor += 1;
            break;

            case Persona.Kind: 
            Humor += 0.5f;
            break;

            case Persona.Sado:
            Humor -= 0.5f;
            break;
        }
        //persona check
    }

}

    /*switch (other.GetComponent<char_GuardaSonolento>().Age)
        {
            case ():
            Humor -= 0.1f;
            break;

            case Shy:
            Humor += 0.2f;
            break;
        }
    }*/