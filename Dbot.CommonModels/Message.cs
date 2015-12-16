using System;
using System.Diagnostics;

namespace Dbot.CommonModels {
  [DebuggerDisplay("{Ordinal}. {OriginalText}")]
  public abstract class Message : TargetedSendable, IEquatable<Message> {

    public string OriginalText {
      get { return _originalText; }
      set {
        _originalText = value;
        _text = value.ToLower();
      }
    }
    private string _originalText;

    public string Text {
      get { return _text; }
      set { OriginalText = value; }
    }
    private string _text;

    public int Ordinal { get; set; }

    public bool Equals(Message that) {
      return
        this.Nick == that.Nick &&
        this.Text == that.Text &&
        this.IsMod == that.IsMod &&
        this.Ordinal == that.Ordinal;
    }
  }
}