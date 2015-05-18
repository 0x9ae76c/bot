﻿//http://www.reddit.com/r/InternetIsBeautiful/comments/2zwvpm/%EF%BD%95%EF%BD%8E%EF%BD%89%EF%BD%83%EF%BD%8F%EF%BD%84%EF%BD%85_%EF%BD%94%EF%BD%8F%EF%BD%8F%EF%BD%8C%EF%BD%93/
//todo Destiny Dharma needs another command where you can temp make a phrase a ban phrase for like 30 minutes NoTears Just for all these OuO faggots NoTears

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks.Dataflow;
using Dbot.Common;
using Dbot.CommonModels;
using Dbot.Data;
using Dbot.Utility;
using Dbot.WebsocketClient;
using Dbot.InfiniteClient;
using Dbot.Banner;

namespace Dbot.Main {
  static class Dbot {

    private static ActionBlock<Message> _logger;
    private static ActionBlock<Message> _modder;
    private static ActionBlock<object> _sender;
    private static ActionBlock<Message> _commander;
    private static ActionBlock<Message> _modCommander;
    private static ActionBlock<Message> _banner;
    private static bool _exit;
    private static IClient _client;
    private static ConcurrentQueue<Message> _recentMessages;
    private static ConcurrentDictionary<int, Message> _dequeueDictionary = new ConcurrentDictionary<int, Message>();
    private static int _index;
    private static int _dequeueIndex;

    static void Main(string[] args) {

      InitializeDatastore.Run();
      _recentMessages = new ConcurrentQueue<Message>();

      var hungryCaterpillar = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded };
      _logger = new ActionBlock<Message>(m => Log(m), hungryCaterpillar);
      _sender = new ActionBlock<object>(m => Send(m), hungryCaterpillar);
      _banner = new ActionBlock<Message>(m => Ban(m), hungryCaterpillar);
      _commander = new ActionBlock<Message>(m => Command(m), hungryCaterpillar);
      _modCommander = new ActionBlock<Message>(m => ModCommand(m), hungryCaterpillar);
      
      //todo, make sure this dosn't run more often than once a minute
      PeriodicTask.Run(() => Tools.LiveStatus(), TimeSpan.FromMinutes(2));
      PeriodicTask.Run(InitializeDatastore.UpdateEmoticons, TimeSpan.FromHours(1));

      //_banner.LinkTo(_commander);
      //consoleSync.Consumer(Constants.ConsoleBuffer);
      
      _client = new InfiniteClient.InfiniteClient();
      _client.Run();
      _client.PropertyChanged += client_PropertyChanged;
      Console.CancelKeyPress += Console_CancelKeyPress;
      //http://stackoverflow.com/questions/14255655/tpl-dataflow-producerconsumer-pattern
      //http://msdn.microsoft.com/en-us/library/hh228601(v=vs.110).aspx

      _exit = false;
      while (!_exit) { //Process.GetCurrentProcess().WaitForExit(); // If you ever have to get rid of the while(true)
        var input = Console.ReadLine();
        if (!String.IsNullOrEmpty(input)) {
          if (input == "exit") {
            _exit = true;
          } else if (input[0] == '~') {
            _client.Send(input.Substring(1));
          } else if (input[0] == '<') {
            _modCommander.Post(Make.Message(input));
          }
        }
      }

      Exit();
    }

    private static void Command(Message message) {

    }

    private static void ModCommand(Message message) {
      var mc = new ModCommander.ModCommander(message.Text);
      if (mc.Message != null) {
        Send(mc.Message);
      }
    }

    private static void Send(object input) {
      if (input is Victim) {

      } else if (input is Message) {
        _client.Send(((Message) input).Text);
      } else if (input is String) {
        _client.Send((string) input);
      } else Tools.ErrorLog("Unsupported type.");
    }

    private static void Ban(Message input) {
      var bantest = new Banner.Banner(input, _recentMessages).BanParser();
      if (bantest == null) {
        //do thing
      } else {
        //do other thing
      }
      var success = _dequeueDictionary.TryAdd(input.Ordinal, input);
      Debug.Assert(success);
    }

    private static void Log(Message message) {
      Console.WriteLine(message.Nick + ": " + message.Text);
      message.Nick = message.Nick.ToLower();
      Datastore.InsertMessage(message);
    }

    static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e) {
      Exit();
    }

    static void Exit() {
      Datastore.Terminate();
    }

    // todo: you can make this better with http://stackoverflow.com/questions/3668217/handling-propertychanged-in-a-type-safe-way
    private static void client_PropertyChanged(object sender, PropertyChangedEventArgs e) {
      var client = (IClient) sender;
      client.CoreMsg.Ordinal = _index;
      _recentMessages.Enqueue(client.CoreMsg);
      _index++;

      var dequeue = true;
      for (var i = _dequeueIndex; i < _dequeueIndex + Settings.MessageLogSize; i++) {
        if (_dequeueDictionary.ContainsKey(i)) {
          continue;
        }
        dequeue = false;
      }

      var success = true;
      while (dequeue && success && _dequeueDictionary.Count > 0) {
        var asdf = new Message();
        success = _recentMessages.TryDequeue(out asdf);
        if (success) {
          _dequeueDictionary.TryRemove(_dequeueIndex, out asdf);
          Console.WriteLine("dequeued " + _dequeueIndex);
        }
        _dequeueIndex++;
      }
      if (dequeue) { }

      _logger.Post(client.CoreMsg);
      _banner.Post(client.CoreMsg);
    }
  }
}