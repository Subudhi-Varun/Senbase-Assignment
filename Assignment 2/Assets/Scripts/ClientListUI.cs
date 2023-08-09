using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DG.Tweening;

public class ClientListUI : MonoBehaviour
{
    public Text labelPointsText;
    public Dropdown filterDropdown;
    public Transform clientListContainer;
    public GameObject clientItemPrefab;
    public GameObject popupWindow;
    public Text popupNameText;
    public Text popupPointsText;
    public Text popupAddressText;

    public List<ClientData> allClients;
    public List<ClientData> filteredClients;

    private void Start()
    {
        StartCoroutine(FetchClientData());
    }

    IEnumerator FetchClientData()
    {
        string apiURL = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
        
        using (UnityWebRequest www = UnityWebRequest.Get(apiURL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching client data: " + www.error);
            }
            else
            {
                ClientDataWrapper dataWrapper = JsonUtility.FromJson<ClientDataWrapper>(www.downloadHandler.text);
                if (dataWrapper != null)
                {
                    allClients = dataWrapper.clients;
                    PopulateFilterDropdown();
                    PopulateClientList();
                }
                else
                {
                    Debug.LogWarning("Failed to process client data.");
                }
            }
        }
    }

    void PopulateFilterDropdown()
    {
        filterDropdown.ClearOptions();
        filterDropdown.AddOptions(new List<string> { "All clients", "Managers only", "Non managers" });
        filterDropdown.onValueChanged.AddListener(OnFilterValueChanged);
    }

    void PopulateClientList()
    {
        foreach (Transform child in clientListContainer)
        {
            Destroy(child.gameObject);
        }
        filteredClients = new List<ClientData>(allClients);
        foreach (ClientData client in filteredClients)
        {
        GameObject clientItem = Instantiate(clientItemPrefab, clientListContainer);
        Text clientText = clientItem.GetComponentInChildren<Text>(); 
        clientText.text = $"{client.label} - {client.points} points";
        clientItem.GetComponent<Button>().onClick.AddListener(() => OpenPopup(client));
        }
    }

    void OnFilterValueChanged(int value)
    {
        switch (value)
        {
            case 1:
                filteredClients = allClients.FindAll(client => client.isManager);
                break;
            case 2:
                filteredClients = allClients.FindAll(client => !client.isManager);
                break;
            default:
                filteredClients = new List<ClientData>(allClients);
                break;
        }

        PopulateClientList();
    }

    public void ShowPopup(ClientData client)
    {
        popupNameText.text = client.label;
        popupPointsText.text = $"Points: {client.points}";
        popupAddressText.text = $"Address: {client.address}";
        popupWindow.SetActive(true);
        popupWindow.transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero);
    }

    public void ClosePopup()
    {
        popupWindow.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => popupWindow.SetActive(false));
    }

    public void OpenPopup(ClientData client)
    {
    ShowPopup(client);
    }
}

[System.Serializable]
public class ClientDataWrapper
{
    public List<ClientData> clients;
}

[System.Serializable]
public class ClientData
{
    public string label;
    public int points;
    public string address;
    public bool isManager;
}
