﻿using System;
using System.Linq;
using Dbot.Client;
using Dbot.CommonModels;
using Dbot.Logic;
using Dbot.Utility;

namespace Dbot.Mod {
  class Program {

    //var server = "irc.rizon.net";
    //const int port = 6667;
    //const string channel = "#destinyecho2";
    //const string nick = "destiny_botboop";
    //const string pass = null;
    const string server = "irc.twitch.tv";
    const int port = 6667;
    const string channel = "#destiny";
    const string nick = "destiny_bot";
    const string pass = PrivateConstants.TwitchOauth;

    static void Main(string[] args) {
      var firstArg = args.FirstOrDefault();

      if (string.IsNullOrWhiteSpace(firstArg)) {
        Console.WriteLine("Select a client:");
        Console.WriteLine("");
        firstArg = Console.ReadLine();
      }

      IClientVisitor client;

      switch (firstArg) {
        case "gg":
          client = new WebSocketClient(PrivateConstants.BotWebsocketAuth);
          break;
        case "ggl":
          client = new WebSocketListenerClient(PrivateConstants.BotWebsocketAuth);
          break;
        case "t":
          client = new SimpleIrcClient(server, port, channel, nick, pass);
          break;
        case "tl":
          client = new SimpleIrcListenerClient(server, port, channel, nick, pass);
          break;
        default:
          throw new Exception("Invalid input");
      }

      new PrimaryLogic(client).Run();
    }
  }
}
