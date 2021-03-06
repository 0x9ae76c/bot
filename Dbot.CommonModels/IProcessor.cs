﻿namespace Dbot.CommonModels {
  public interface IProcessor {
    void Process(PublicMessage message);
    void Process(PrivateMessage message);
    void Process(Mute mute);
    void Process(Ban ban);
    void Process(UnMuteBan unMuteBan);
    void Process(Broadcast broadcast);
    void Process(ConnectedUsers connectedUsers);
  }
}
