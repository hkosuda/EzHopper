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
    public string description = "";
    public string detail = "";
    public int counter = 0;

    public CommandType commandType = CommandType.normal;

    public virtual void OnMapInitialized() { }

    public virtual List<string> AvailableValues(List<string> values)
    {
        return new List<string>();
    }

    public virtual string CurrentValue() { return ""; }

    public virtual string DefaultValue() { return ""; }

    public abstract void CommandMethod(Tracer tracer, List<string> values, List<string> options);

    static public void AddMessage(string message, Tracer.MessageLevel level, Tracer tracer, List<string> options, int tabOffset = 1)
    {
        tracer.AddMessage(message, level, options, tabOffset);
    }

    static protected string ERROR_NotInteger(string str)
    {
        return str + "�𐮐��ɕϊ��ł��܂���D";
    }

    static protected string ERROR_OutOfRange(int index, int indexLim)
    {
        return index.ToString() + "�͗L���ȃC���f�b�N�X�͈̔͊O�ł��D�L���Ȕ͈͂�' 0 �` " + indexLim.ToString() + "' �ł��D";
    }

    static protected string ERROR_OverValues(int correctValues)
    {
        return (correctValues + 1).ToString() + "�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D";
    }

    static protected string ERROR_AvailableOnly(int place, List<string> values)
    {
        if(values == null || values.Count == 0) { return ""; }

        var message = place.ToString() +  "�Ԗڂ̒l�Ƃ��ẮC";

        foreach(var value in values)
        {
            message += "'" + value + "' ";
        }

        message += "�̂ݗ��p�\�ł��D";
        return message;
    }

    static protected string ERROR_InvalidKey(string keyString)
    {
        return keyString + "��L���ȃL�[�ɕϊ��ł��܂���D";
    }

    static protected string ERROR_InvalidKeyAlert()
    {
        return "�Q�[�����Ŏg�p�ł���L�[�̖��̂𒲂ׂ�ɂ́C'keycheck'�R�}���h���g�p���Ă��������D";
    }

    static protected string ERROR_NeedValue(int values)
    {
        return values.ToString() + "�ȏ�̒l���K�v�ł��D";
    }
}

public class Tracer
{
    public enum MessageLevel
    {
        normal,
        warning,
        error,
        emphasis,
    }

    public enum Option
    {
        mute,
        echo,
        flash,
    }

    public bool NoError { get; private set; }
    public Command Command { get; }

    List<MsgLv> consoleMessageList;
    List<MsgLv> chatMessageList;

    public Tracer(Command command)
    {
        Command = command;
        NoError = true;

        consoleMessageList = new List<MsgLv>();
        chatMessageList = new List<MsgLv>();
    }

    public void AddMessage(string message, MessageLevel level, List<string> options, int tabOffset = 1)
    {
        if (consoleMessageList == null) { consoleMessageList = new List<MsgLv>(); }
        if (chatMessageList == null) { chatMessageList = new List<MsgLv>(); }

        if (level == MessageLevel.error) { NoError = false; }

        var offset = "";
        for(var n = 0; n < tabOffset; n++) { offset += "\t"; }

        if (CheckOption(Option.mute, options))
        {
            return; // nothing to do.
        }

        else if (CheckOption(Option.echo, options))
        {
            consoleMessageList.Add(new MsgLv(offset + message, level));
            chatMessageList.Add(new MsgLv(message, level));
        }

        else if (CheckOption(Option.flash, options))
        {
            chatMessageList.Add(new MsgLv(message, level));
        }

        else
        {
            consoleMessageList.Add(new MsgLv(offset + message, level));
        }
    }

    public string ConsoleMessage()
    {
        return GetFullMessage(consoleMessageList);
    }

    public string ChatMessage()
    {
        return GetFullMessage(chatMessageList);
    }

    static string GetFullMessage(List<MsgLv> messageList)
    {
        if (messageList == null) { return ""; }

        var fullMessage = "";

        for(var n = 0; n < messageList.Count; n++)
        {
            var msgLv = messageList[n];

            var level = msgLv.level;
            var message = msgLv.message;

            if (level == MessageLevel.normal) { fullMessage += message + "\n"; continue; }
            if (level == MessageLevel.warning) { fullMessage += "<color=orange>" + message + "</color>\n"; continue; }
            if (level == MessageLevel.error) { fullMessage += "<color=red>" + message + "</color>\n"; continue; }
            if (level == MessageLevel.emphasis) { fullMessage += "<color=lime>" + message + "</color>\n"; }
        }

        return fullMessage.TrimEnd(new char[] { '\n' });
    }

    static public bool CheckOption(Option option, List<string> options)
    {
        if (options == null || options.Count == 0) { return false; }

        if (option == Option.mute)
        {
            return options.Contains("-m") || options.Contains("-mute");
        }

        else if (option == Option.echo)
        {
            return options.Contains("-e") || options.Contains("-echo");
        }

        else if (option == Option.flash)
        {
            return options.Contains("-f") || options.Contains("-flash");
        }

        return false;
    }

    class MsgLv
    {
        public string message;
        public MessageLevel level;

        public MsgLv(string message, MessageLevel level)
        {
            this.message = message;
            this.level = level;
        }
    }
}

