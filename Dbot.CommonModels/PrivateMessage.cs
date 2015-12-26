﻿using Dbot.WebSocketModels;
using Newtonsoft.Json;

namespace Dbot.CommonModels {
  public class PrivateMessage : Message {
    public PrivateMessage(string nick, string originalText)
      : base(nick, originalText) { }

    public override void Accept(IClientVisitor visitor) {
      visitor.Visit(this);
    }

    public override string ToString() {
      return "Private Messaged " + Nick + " with: " + OriginalText;
    }
  }
}