using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataFetcher : MonoBehaviour
{
    public string url = "https://api.jsonbin.io/v3/b/6686a992e41b4d34e40d06fa";
    public DataWrapper dataWrapper;

    public Text playerNameText;
    public Text levelText;
    public Text healthText;
    public Text positionText;
    public Text inventoryText;

    void Start()
    {
        StartCoroutine(FetchData());
    }

    IEnumerator FetchData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                dataWrapper = JsonUtility.FromJson<DataWrapper>(jsonResponse);
                DisplayData();
            }
        }
    }

    void DisplayData()
    {
        playerNameText.text = "Player Name: " + dataWrapper.record.playerName;
        levelText.text = "Level: " + dataWrapper.record.level;
        healthText.text = "Health: " + dataWrapper.record.health;
        positionText.text = "Position: (" + dataWrapper.record.position.x + ", " + dataWrapper.record.position.y + ", " + dataWrapper.record.position.z + ")";
        
        inventoryText.text = "Inventory:\n";
        foreach (var item in dataWrapper.record.inventory)
        {
            inventoryText.text += item.itemName + " - Quantity: " + item.quantity + ", Weight: " + item.weight + "\n";
        }
    }
}
