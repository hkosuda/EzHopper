using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public delegate bool ValuesCheckMethod(Tracer tracer, List<string> values);
    public delegate void CommandMethod(Tracer tracer, List<string> values);

    public delegate List<string> AvailableValuesMethod(List<string> values);
    public delegate string DescriptionMethod(List<string> values);

    public string commandName;
    public CommandMethod commandMethod;
    public ValuesCheckMethod valuesCheckMethod;
    public AvailableValuesMethod availableValues;
    public DescriptionMethod description;
    public string simpleDescription;

    public Command(string commandName, CommandMethod commandMethod, ValuesCheckMethod valuesCheckMethod, 
        AvailableValuesMethod availableValues = null, DescriptionMethod description = null, string simpleDescription = "")
    {
        this.commandName = commandName;
        this.commandMethod = commandMethod;
        this.valuesCheckMethod = valuesCheckMethod;
        this.availableValues = availableValues;
        this.description = description;
        this.simpleDescription = simpleDescription;
    }

    public bool CheckValues(Tracer tracer, List<string> values)
    {
        if (valuesCheckMethod == null) { return true; }

        return valuesCheckMethod(tracer, values);
    }

    public void Execute(Tracer tracer, List<string> values)
    {
        commandMethod?.Invoke(tracer, values);
    }
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

            if (level == MessageLevel.normal) { fullMessage += message + "\n"; continue; }
            if (level == MessageLevel.warning) { fullMessage += "<color=orange>" + message + "</color>\n"; continue; }
            if (level == MessageLevel.error) { fullMessage += "<color=red>" + message + "</color>\n"; }
        }

        return fullMessage;
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

