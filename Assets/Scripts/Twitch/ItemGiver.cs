using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemGiver : MonoBehaviour
{
    [Header("UI References")] 
    public GameObject RootObject;
    public Text toolTipText;

    [Header("Spawning Prefab")] 
    public GameObject HealthPrefab;
    public GameObject AmmoPrefab;
    public Transform SpawnPosition;
    
    [Header("Twitch Variables")] 
    public TwitchClient twitchClient;
    
    // Start is called before the first frame update
    void Start()
    {
        twitchClient.chatMessage += OnChatMessage;
    }

    public void StartItemGiver()
    {
        RootObject.SetActive(true);
    }

    private void OnChatMessage(ChatMessage msg)
    {
        if(msg.Message.Equals("!health"))
        {
            //Spawn health
            Instantiate(HealthPrefab, SpawnPosition.position, SpawnPosition.rotation);
        }
        else if (msg.Message.Equals("!ammo"))
        {
            //Spawn Ammo
            Instantiate(AmmoPrefab, SpawnPosition.position, SpawnPosition.rotation);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ItemGiver))]
public class ItemGiverInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.Space(10f);

        if (GUILayout.Button("Enable"))
        {
            ((ItemGiver)target).StartItemGiver();
        }
    }
}
#endif

