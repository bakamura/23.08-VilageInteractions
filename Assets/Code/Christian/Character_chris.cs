using System.Collections.Generic;
using UnityEngine;

public class Character_chris : MonoBehaviour
{
    public float moveSpeed;

    public Vector3 targetPosition;
    public GameObject gamemanager;
    private Timer timer;
    public List<GameObject> lugares;
    private Dictionary<int, string> periodToLocation = new Dictionary<int, string>();

    public int Age = 75;
    public enum Gender { Male, Female, Neutro }
    public enum Money { Poor, Medium, Rich }
    public enum Persona { Grumpy, Shy, Kind, Flirty, Sado, Loud }
    public enum Race { Dog, Elemental, Boto, Human}
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

        
        periodToLocation.Add(0, "Hospital");
        periodToLocation.Add(1, "Padaria");
        periodToLocation.Add(2, "Biblioteca");
        periodToLocation.Add(3, "Square");
        periodToLocation.Add(4, "Bar");
        periodToLocation.Add(5, "Biblioteca");
        

        
        int initialPeriod = timer.GetCurrentPeriod(); 
        ChangeLocation(initialPeriod);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

      if(targetPosition == GameObject.Find("Hospital").transform.position)
        {
            Humor = 0;
            persona = Persona.Sado;
        }
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

        if(other.gameObject.tag == "Char"){

            Relacoes(other.gameObject);

            Interagir();
            Debug.Log("Pegar Valores");
        }
        Debug.Log("Chegou");
    }

   public void Interagir(){

       
    }

    void Relacoes(GameObject other)
    {

        if (other.GetComponent<Character_chris>().Age > 40)
        {
            Humor++;
        }
        else if (other.GetComponent<Character_chris>().Age <= 40 && other.GetComponent<Character_chris>().Age < 100)
        {
            Humor--;
        }
        else
        {
            Humor = 0;
        }
        Debug.Log(other.GetComponent<Character_chris>().money);

        switch (other.GetComponent<Character_chris>().money){


            case Money.Poor:

                Humor-= 2;

               persona = Persona.Sado;
                break;

            case Money.Medium:

                if (persona == Persona.Kind)
                {
                  
                    other.GetComponent<Character_chris>().money = Money.Rich;
                }
                else if (persona == Persona.Grumpy || persona == Persona.Sado)
                {
                    Humor--;
                    other.GetComponent<Character_chris>().money = Money.Poor;
                    money = Money.Rich;
                }
                else
                {
                    Humor--;
                    other.GetComponent<Character_chris>().money = Money.Poor;
                    money = Money.Rich;
                }
                break;

            case Money.Rich:
                if (persona == Persona.Grumpy || persona == Persona.Sado)
                {
                    Humor--;
                    other.GetComponent<Character_chris>().money = Money.Medium;
                    money = Money.Rich;
                }
                else
                {
                    Humor += 2;
                    persona = Persona.Kind;
                }
                break;
        }


        switch (other.GetComponent<Character_chris>().persona)
        {


            case Persona.Loud:

                persona = Persona.Grumpy;

                break;

            case Persona.Shy:

                persona = Persona.Sado;

                break;

            case Persona.Sado:

                persona = Persona.Flirty;

                break;

            case Persona.Kind:

                persona = Persona.Loud;
                money = Money.Rich;

                break;

        }

        if (money == Money.Poor)
        {
            other.GetComponent<Character_chris>().money = Money.Poor;
            money = Money.Rich;
            Humor = 0;
        }

        switch (other.GetComponent<Character_chris>().race)
        {

            case Race.Dog:

                Humor++;

                break;

            case Race.Human:

                if (persona == Persona.Grumpy || persona == Persona.Sado || persona == Persona.Loud)
                {
                    Humor--;
                }else if (persona == Persona.Flirty)
                {
                    Humor++;
                }

                break;

           

        }
        if (Humor == -3)
        {
            targetPosition = GameObject.Find("Hospital").transform.position;
        }


    }

}

