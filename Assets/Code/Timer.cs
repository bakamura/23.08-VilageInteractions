using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Timer : MonoBehaviour
{
    public float timeIntervalSeconds = 5.0f; 
    public float currentTime = 0.0f; 
    public Text horario;
    public int periodo;

    public  GameObject[] Chars;

  public List<GameObject> Lugares = new List<GameObject>();
    public List<Vector3> positions = new List<Vector3>();
    public List<GameObject> Characters = new List<GameObject>();

    void Awake(){
        horario.text = "Começa o dia";
        periodo = -1;
        Chars = GameObject.FindGameObjectsWithTag("Char");
        Characters.AddRange(Chars);

        if (positions.Count < 5)
        {
            foreach (GameObject obj in Lugares)
            {
                positions.Add(obj.transform.position);
            }
        }

    }
    private void Update()
    {
        
        currentTime += Time.deltaTime;
        // Print the positions to the console
       
        if (currentTime >= timeIntervalSeconds)
        {
           
            SimulatePassageOfTwoHours();
            currentTime = 0.0f;
        }
    }

    private void SimulatePassageOfTwoHours()
    {
        if(periodo<6){
        periodo += 1;
        foreach(GameObject obj in Chars){
        obj.BroadcastMessage("MudaLugar", periodo);
        }
        horario.text = "1"+periodo+":00";
    }}

     public int GetCurrentPeriod()
    {
       
        float currentTimeInSeconds = Time.time;
        float periodDurationInSeconds = 120.0f; 

 
        int currentPeriod = Mathf.FloorToInt(currentTimeInSeconds / periodDurationInSeconds);

        return currentPeriod;
    }
}
