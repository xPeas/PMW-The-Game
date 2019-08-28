using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine.PlayerLoop;

public class TwitchClient : MonoBehaviour
{
    //the client object is defined within the TwitchLib Library
    public Client client;
    private string channel_name = "paymoneywubby";
    
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        
        //Set up the bot and tell which channel to join
        ConnectionCredentials credentials = new ConnectionCredentials(Secrets.TwitchAccountName, Secrets.AccessToken);
        client = new Client();
        client.Initialize(credentials, channel_name);
        
        //Subscribe to any event the bot will listen to
        client.OnConnected += OnConnected;
        client.OnMessageReceived += OnMessageReceived;
        client.OnNewSubscriber += OnNewSub;
        client.OnReSubscriber += OnReSub;
        client.OnGiftedSubscription += OnGiftedSub;
        
        //Connect bot to the channel
        client.Connect();
    }

    private void OnReSub(object sender, OnReSubscriberArgs e)
    {
        var ReSub = e.ReSubscriber;
        Debug.LogFormat("ReSub: Username:{0}, TierSub:{1}, SubLength:{2}", ReSub.DisplayName, ReSub.SubscriptionPlan, ReSub.Months);
    }

    private void OnGiftedSub(object sender, OnGiftedSubscriptionArgs e)
    {
        var GiftSub = e.GiftedSubscription;
        Debug.LogFormat("Gifted Subs: MsgParamRecipDisplayName: {0}, Display Name: {1}", GiftSub.MsgParamRecipientDisplayName, GiftSub.DisplayName);
    }

    private void OnNewSub(object sender, OnNewSubscriberArgs e)
    {
        Debug.LogFormat("SubChannel: {0}, Username: {1}, Login: {2}, Tier: {3}",e.Channel, e.Subscriber.DisplayName, e.Subscriber.Login, e.Subscriber.SubscriptionPlan);
    }

    private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        ChatMessage msg = e.ChatMessage;
        Debug.LogFormat("Messsage:{0}, Username:{1}, Subscriber:{2}, IsMod:{3}, UserType:{4}, UserID:{5}", msg.Message, msg.Username, msg.IsSubscriber, msg.IsModerator, msg.UserType, msg.UserId);
    }

    private void OnConnected(object sender, OnConnectedArgs e)
    {
        Debug.LogFormat("{0}:{1}", e.BotUsername, e.AutoJoinChannel);
    }
}
