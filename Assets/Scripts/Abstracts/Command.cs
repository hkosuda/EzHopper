using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public enum CommandType
    {
        normal,
        values,
    }

    public string commandName;
    public string description;
    public CommandType commandType = CommandType.normal;

    public virtual void OnMapInitialized() { }

    public virtual List<string> AvailableValues(List<string> values)
    {
        return new List<string>();
    }

    public abstract void CommandMethod(Tracer tracer, List<string> values);
}

public class Tracer
{
    public enum MessageLevel
    {
        normal,
        warning,
        error,
    }

    public bool NoError { get; private set; }
    List<MsgLvl> messageList;

    public Tracer()
    {
        NoError = true;
        messageList = new List<MsgLvl>();
    }

    public void AddMessage(string message, MessageLevel level)
    {
        if (messageList == null) { messageList = new List<MsgLvl>(); }
        if (level == MessageLevel.error) { NoError = false; }

        messageList.Add(new MsgLvl(message, level));
    }

    public string GetFullMessage()
    {
        if (messageList == null) { return ""; }

        var fullMessage = "";

        foreach (var msgLvl in messageList)
        {
            var level = msgLvl.level;
            var message = msgLvl.message;

            if (level == MessageLevel.normal) { fullMessage += "\t" + message + "\n"; continue; }
            if (level == MessageLevel.warning) { fullMessage += "\t<color=orange>" + message + "</color>\n"; continue; }
            if (level == MessageLevel.error) { fullMessage += "\t<color=red>" + message + "</color>\n"; }
        }

        return fullMessage.TrimEnd(new char[] { '\n' });
    }

    class MsgLvl
    {
        public string message;
        public MessageLevel level;

        public MsgLvl(string message, MessageLevel level)
        {
            this.message = message;
            this.level = level;
        }
    }
}

