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
        horario.text = "10h";
        periodo = -1;
        Chars = GameObject.FindGameObjectsWithTag("Char");
        Characters.AddRange(Chars); 
    }
    private void Update()
    {
        
        currentTime += Time.deltaTime;
        if(positions.Count<5){
        foreach (GameObject obj in Lugares)
        {
            positions.Add(obj.transform.position);
        }
        }
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
        // Implement your logic to determine the current period here.
        // You might want to use real-time or game time to calculate the period.
        // For this example, let's use a simple time-based calculation.

        float currentTimeInSeconds = Time.time; // Get the current time in seconds.
        float periodDurationInSeconds = 120.0f; // Duration of each period (2 minutes).

        // Calculate the current period.
        int currentPeriod = Mathf.FloorToInt(currentTimeInSeconds / periodDurationInSeconds);

        // Return the current period.
        return currentPeriod;
    }
}
