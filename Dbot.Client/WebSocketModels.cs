﻿using System;

namespace Dbot.Client {
  public class NamesReceiver {
    public User[] Users { get; set; }
    public string Connectioncount { get; set; }
  }

  public class User {
    public string Nick { get; set; }
    public string[] Features { get; set; }

  }

  public class MessageReceiver : User {
    public long Timestamp { get; set; }
    public string Data { get; set; }
  }

  public class JoinReceiver : User {
    public long Timestamp { get; set; }
  }

  public class QuitReceiver : JoinReceiver {

  }

  public class MuteReceiver : MessageReceiver {
    //MUTE {"nick":"Bot","features":["protected","bot"],"timestamp":1429322359451,"data ":"venat"}
  }

  public class MessageSender {
    public MessageSender(string input) {
      data = input;
    }
    public string data { get; set; }
  }

  public class MuteSender {
    public MuteSender(string victim, TimeSpan duration) {
      data = victim;
      this.duration = ((ulong) duration.TotalMilliseconds) * 1000000UL;
    }
    public string data { get; set; }
    public ulong duration { get; set; }
  }
  /*
BROADCAST {"timestamp":1426360863360,"data":"test"}

elif command == "MUTE":
  s1msg( "<" + payload["nick"] + "> <=== just muted " + payload["data"])
elif command == "UNMUTE":
  s1msg( "<" + payload["nick"] + "> <=== just unmuted " + payload["data"])
elif command == "SUBONLY":
  if payload["data"] == "on":
    s1msg( "<" + payload["nick"] + "> <=== just enabled subscribers only mode.")
  else:
    s1msg( "<" + payload["nick"] + "> <=== just disabled subscribers only mode.")
elif command == "BAN":
  s1msg( "<" + payload["nick"] + "> <=== just banned " + payload["data"])
elif command == "UNBAN":
  s1msg( "<" + payload["nick"] + "> <=== just unbanned " + payload["data"])
elif command == "PING":
  sock.send("PONG" + data[4:])

elif command != "":
  s1msg( "<UNKNOWN_COMMAND> " + data)
   */
}
