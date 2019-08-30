using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PollSystem : MonoBehaviour
{
    [Header("UI References")] 
    public GameObject RootPanel;
    
    public Text QuestionText;

    public Text VotesText;

    public Text TimeText;

    public Text[] TextOptions;

    public Slider[] SliderOptions;

    [Header("Poll Settings")] 
    public float PollTimer;

    public string PollQuestion;

    public string[] PollAnswers;

    [Header("Twitch Variables")] 
    public TwitchClient twitchClient;

    private Dictionary<string, byte> userVotes;

    private int[] votes;

    private bool pollActive = false;

    private byte answerCount;

    private float timeRemaining;
    
    // Start is called before the first frame update
    void Start()
    {
        twitchClient.chatMessage += OnChatMessage;
    }

    private void Update()
    {
        if (pollActive)
        {
            timeRemaining -= Time.deltaTime;

            int secLeft = Mathf.FloorToInt(timeRemaining);
            TimeText.text = $"Time:{secLeft}";
        }
    }

    public void StartPoll()
    {
        if (pollActive)
            return;
        
        answerCount = (byte)PollAnswers.Length;

        for (int i = 0; i < TextOptions.Length; i++)
        {
            bool enable = i < answerCount;
            TextOptions[i].transform.parent.gameObject.SetActive(enable);
        }

        for (int i = 0; i < answerCount; i++)
        {
            TextOptions[i].text = PollAnswers[i];
            SliderOptions[i].value = 0f;
        }

        QuestionText.text = PollQuestion;

        VotesText.text = "Votes: 0";
        
        userVotes = new Dictionary<string, byte>();
        votes = new int[answerCount];

        timeRemaining = PollTimer;
        
        Invoke(nameof(EndPoll), PollTimer);
        
        RootPanel.SetActive(true);

        pollActive = true;
    }

    private void EndPoll()
    {
        pollActive = false;
    }

    private void OnChatMessage(ChatMessage msg)
    {
        if (!pollActive)
            return;
        
        //Check if this person has already voted
        if (!userVotes.ContainsKey(msg.UserId))
        {
            //Check if this is a vote or not
            if (byte.TryParse(msg.Message, out byte voteNum))
            {
                if (voteNum <= answerCount)
                {
                    userVotes.Add(msg.UserId, voteNum);
                    votes[voteNum - 1]++;
                    CountVotes();
                }
            }
        }
    }

    private void CountVotes()
    {
        int voteNum = userVotes.Count;

        for (int i = 0; i < answerCount; i++)
        {
            SliderOptions[i].value = votes[i] / (float) voteNum;
        }

        VotesText.text = $"Votes:{voteNum}";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PollSystem))]
public class PollSystemInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.Space(10f);

        if (GUILayout.Button("Start Poll"))
        {
            ((PollSystem)target).StartPoll();
        }
    }
}
#endif
