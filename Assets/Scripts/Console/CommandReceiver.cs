using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CommandReceiver
{
    static public EventHandler<string> CommandRequestBegin { get; set; }
    static public EventHandler<Tracer> CommandRequestEnd { get; set; }

    static public Dictionary<string, Command> CommandList { get; private set; }

    static public void AddCommand(Command command)
    {
        if (CommandList == null) { CommandList = new Dictionary<string, Command>(); }

        CommandList.Add(command.commandName, command);
    }

    static public void SubCommand(string commandName)
    {
        if (CommandList == null || CommandList.Count == 0) { return; }
        if (!CommandList.ContainsKey(commandName)) { return; }

        CommandList.Remove(commandName);
    }

    static public bool RequestCommand(string sentence, bool echo, bool exec = true)
    {
        // start //

        if (echo) { CommandRequestBegin?.Invoke(null, sentence); }

        var tracer = new Tracer();

        var values = GetValues(sentence);
        if (values == null || values.Count == 0) { SimpleEnd(tracer, echo); return false; }

        var commandName = values[0];
        if (CommandList == null || CommandList.Count == 0) { UnkownCommand(tracer, commandName, echo); return false; }
        if (!CommandList.ContainsKey(commandName)) { UnkownCommand(tracer, commandName, echo); return false; }

        var command = CommandList[commandName];

        if (exec) { command.CommandMethod(tracer, values); }
        if (echo) { CommandRequestEnd?.Invoke(null, tracer); }

        if (tracer.NoError) { return true; }
        return false;

        // end //

        // - inner function
        static void SimpleEnd(Tracer tracer, bool echo)
        {
            if (echo) { CommandRequestEnd?.Invoke(null, tracer); }
        }

        // - inner function
        static void UnkownCommand(Tracer tracer, string commandName, bool echo)
        {
            tracer.AddMessage(commandName + "というコマンドは存在しません", Tracer.MessageLevel.error);
            if (echo) { CommandRequestEnd?.Invoke(null, tracer); }
        }
    }

    static public List<string> GetValues(string sentence)
    {
        // 1st : hankaku, 2nd : zenkaku
        var splitted = sentence.Split(new string[] { " ", "　" }, StringSplitOptions.RemoveEmptyEntries);
        if (splitted == null) { return null; }

        var values = new List<string>(splitted);

        for(var n = 0; n < values.Count; n++)
        {
            values[n] = values[n].ToLower();
        }

        return values;
    }
}

