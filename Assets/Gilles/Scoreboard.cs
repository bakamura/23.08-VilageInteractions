using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public GameObject controle;
    public GameObject scoreboardEntryPrefab; // Prefab for the scoreboard entry
    public Transform scoreboardParent; // Parent transform for the scoreboard entries
    public GameObject Panel;
    public string scoreFormat = "<color=#FFD700>{0}</color> <color=#FFD700>{1}</color> <color=#FFD700>{2}</color>\n"; // Format for displaying each entry

    void Update()
    {
        Debug.Log(controle.GetComponent<GameManager>()._periodCurrent);
        if (controle.GetComponent<GameManager>()._periodCurrent == 6)
        {
            UpdateScoreboard();
        }
    }

    void UpdateScoreboard()
    {
        Panel.SetActive(true);
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Char");

        foreach (Transform child in scoreboardParent)
        {
            Destroy(child.gameObject); // Clear existing entries
        }

        foreach (GameObject character in characters)
        {
            if (character.TryGetComponent<CharBase>(out CharBase charBase))
            {
                // Create a new scoreboard entry using the prefab
                GameObject entry = Instantiate(scoreboardEntryPrefab, scoreboardParent);
                Text entryText = entry.GetComponent<Text>();

                // Format and set the text for the entry
                entryText.text = string.Format(scoreFormat, character.name, charBase.Humor, charBase.Race, charBase.Age, charBase.Gender, charBase.Money, charBase.Persona);
            }
            else
            {
                Debug.LogError("CharBase component not found on " + character.gameObject.name);
            }
        }
    }
}
