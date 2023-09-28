using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float moveSpeed;

    [System.NonSerialized] public Vector3 targetPosition;
    [System.NonSerialized]public GameObject gamemanager;
    [System.NonSerialized] public Timer timer;
    [System.NonSerialized] public List<GameObject> lugares;
    [System.NonSerialized] public Dictionary<int, string> periodToLocation = new Dictionary<int, string>();

    public int Age = 90;
    public enum Gender { Male, Female, Neutro }
    public enum Money { Poor, Medium, Rich }
    public enum Persona { Grumpy, Shy, Kind, Flirty, Sado, Loud }
    public enum Race { Dog, Elemental, Boto, Human }
    [Range(-3,3)]public float humor;
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

    public void ChangeLocation(int periodo)
    {

        if (periodToLocation.ContainsKey(periodo))
        {
            string locationName = periodToLocation[periodo];
            GameObject locationObject = lugares.Find(lugar => lugar.name == locationName);

            if (locationObject != null)
            {
                Debug.Log(timer.positions);
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

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Char")
        {
            if (other.TryGetComponent<Character>(out Character character))
            {
                Interagir(character);
            }

        }
    }

    public abstract void Interagir(Character otherCharacter);
}

