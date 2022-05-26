using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CommandReceiver
{
    static public EventHandler<string> CommandRequestBegin { get; set; }
    static public EventHandler<Tracer> CommandRequestEnd { get; set; }

    static Dictionary<string, Command> commandList;

    static public void AddCommand(Command command)
    {
        if (commandList == null) { commandList = new Dictionary<string, Command>(); }

        commandList.Add(command.commandName, command);
    }

    static public void SubCommand(string commandName)
    {
        if (commandList == null || commandList.Count == 0) { return; }
        if (!commandList.ContainsKey(commandName)) { return; }

        commandList.Remove(commandName);
    }

    static public void RequestCommand(string sentence, bool echo)
    {
        // start //

        if (echo) { CommandRequestBegin?.Invoke(null, sentence); }

        var tracer = new Tracer();

        var values = GetValues(sentence);
        if (values == null || values.Count == 0) { SimpleEnd(tracer, echo); return; }

        var commandName = values[0];
        if (commandList == null || commandList.Count == 0) { UnkownCommand(tracer, commandName, echo); return; }
        if (!commandList.ContainsKey(commandName)) { UnkownCommand(tracer, commandName, echo); return; }

        var command = commandList[commandName];
        if (!command.CheckValues(tracer, values)) { InvalidValues(tracer, echo); return; }

        command.CommandMethod(tracer, values);
        if (echo) { CommandRequestEnd?.Invoke(null, tracer); }

        // end //

        // - inner function
        static List<string> GetValues(string sentence)
        {
            // 1st : hankaku, 2nd : zenkaku
            var splitted = sentence.Split(new string[] { " ", "　" }, StringSplitOptions.RemoveEmptyEntries);
            if (splitted == null) { return null; }

            return new List<string>(splitted);
        }

        // - inner function
        static void SimpleEnd(Tracer tracer, bool echo)
        {
            if (!echo) { return; }

            CommandRequestEnd?.Invoke(null, tracer);
        }

        // - inner function
        static void UnkownCommand(Tracer tracer, string commandName, bool echo)
        {
            if (!echo) { return; }

            tracer.AddMessage(commandName + "というコマンドは存在しません", Tracer.MessageLevel.error);
            CommandRequestEnd?.Invoke(null, tracer);
        }
        
        // - inner function
        static void InvalidValues(Tracer tracer, bool echo)
        {
            if (!echo) { return; }

            tracer.AddMessage("無効な値：", Tracer.MessageLevel.error);
            CommandRequestEnd?.Invoke(null, tracer);
        }
    }
}

