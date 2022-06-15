using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

static public class CommandReceiver
{
    static readonly List<string> symbols = new List<string>() { "now", "time", "map", "section", "counter" };

    static public EventHandler<string> CommandRequestBegin { get; set; }
    static public EventHandler<string> UnknownCommandRequest { get; set; }
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

    static public bool RequestCommand(string _sentence)
    {
        var sentence = string.Copy(_sentence);

        sentence = sentence.ToLower();
        sentence = Grouping(sentence);
        sentence = ReplaceSymbol(sentence);
        sentence = ReplaceEscapedSymbol(sentence);
        sentence = OverrideCommand.OverrideSentence(sentence);
        sentence = CorrectSentence(sentence);

        // start //
        CommandRequestBegin?.Invoke(null, sentence);

        var values = GetValues(sentence);
        if (values == null || values.Count == 0) { return false; }

        var commandName = values[0];
        if (CommandList == null || CommandList.Count == 0) { UnknownCommandRequest?.Invoke(null, sentence); return false; }
        if (!CommandList.ContainsKey(commandName)) { UnknownCommandRequest?.Invoke(null, sentence); return false; }

        var command = CommandList[commandName];
        var options = GetOptions(sentence);

        var tracer = new Tracer(command);

        command.CommandMethod(tracer, values, options);
        CommandRequestEnd?.Invoke(null, tracer);

        if (tracer.NoError) { return true; }
        return false;

        // end //
    }

    static public string Grouping(string sentence)
    {
        var rgx = new Regex(@"\"".*?\""");

        sentence = rgx.Replace(sentence, _Grouping);

        return sentence;

        // - inner function
        static string _Grouping(Match match)
        {
            if (match.Value == null)
            {
                return "//";
            }

            var value = match.Value;
            value = value.TrimEnd(new char[1] { '"' });
            value = value.TrimStart(new char[1] { '"' });

            var splitted = SplitString(value);
            var group = "/";

            foreach (var s in splitted)
            {
                group += s + "/";
            }

            group = EscapeSymbol(group);

            return group;

            // - - inner function
            static string EscapeSymbol(string sentence)
            {
                foreach(var symbol in symbols)
                {
                    var sym = "%" + symbol + "%";
                    var esc = "&" + symbol + "&";

                    sentence = Regex.Replace(sentence, sym, esc);
                }

                return sentence;
            }
        }
    }

    static public string ReplaceSymbol(string sentence)
    {
        sentence = Regex.Replace(sentence, "%now%", ReplaceNow);
        sentence = Regex.Replace(sentence, "%time%", ReplaceTime);
        sentence = Regex.Replace(sentence, "%map%", ReplaceMap);
        sentence = Regex.Replace(sentence, "%section%", ReplaceSection);
        sentence = Regex.Replace(sentence, "%counter%", ReplaceCounter);

        return sentence;

        // - inner function
        static string ReplaceNow(Match match)
        {
            return TimerCommand.DateTimeString();
        }

        // - inner function
        static string ReplaceMap(Match match)
        {
            return MapsManager.CurrentMap.MapName.ToString();
        }

        // - inner function
        static string ReplaceTime(Match match)
        {
            return TimerCommand.SecMSec(TimerCommand.PastTime, separator: "s");
        }

        // - inner function
        static string ReplaceSection(Match match)
        {
            return TimerCommand.PaddingZero(MapsManager.CurrentMap.Index);
        }

        // - inner function
        static string ReplaceCounter(Match match)
        {
            return CounterCommand.Counter.ToString();
        }
    }

    static string ReplaceEscapedSymbol(string sentence)
    {
        foreach(var symbol in symbols)
        {
            var esc = "&" + symbol + "&";
            var sym = "%" + symbol + "%";

            sentence = Regex.Replace(sentence, esc, sym);
        }

        return sentence;
    }

    static List<string> SplitString(string str)
    {
        // 1st : hankaku, 2nd : zenkaku
        return new List<string>(str.Split(new string[] { " ", "@" }, StringSplitOptions.RemoveEmptyEntries));
    }

    static bool IsOption(string value)
    {
        return Regex.IsMatch(value, @"\A-[a-z]+\z");
    }

    static public string CorrectSentence(string sentence)
    {
        var values = GetValues(sentence);
        var options = GetOptions(sentence);

        var corrected = "";

        if (values != null)
        {
            foreach (var value in values)
            {
                corrected += value + " ";
            }
        }

        if (options != null && options.Count > 0)
        {
            corrected += options.Last();
        }

        return corrected.TrimEnd(new char[1] { ' ' });
    }

    static public List<string> GetValues(string sentence)
    {
        var splitted = SplitString(sentence);
        if (splitted == null || splitted.Count == 0) { return null; }

        var values = new List<string>();

        for (var n = 0; n < splitted.Count; n++)
        {
            var value = splitted[n].ToLower();

            if (!IsOption(value))
            {
                values.Add(value);
            }
        }

        return values;
    }

    static public List<string> GetOptions(string sentence)
    {
        var splitted = SplitString(sentence);
        if (splitted == null) { return null; }

        var options = new List<string>();

        for (var n = 0; n < splitted.Count; n++)
        {
            var value = splitted[n].ToLower();

            if (IsOption(value))
            {
                options.Add(value);
            }
        }

        return options;
    }

    static public string UnpackGrouping(string group)
    {
        if (Regex.IsMatch(group, @"\/.*\/"))
        {
            var splitted = group.Split(new string[1] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var corrected = "";

            if (splitted == null) { return corrected; }

            foreach (var s in splitted)
            {
                corrected += s + " ";
            }

            return corrected.Trim();
        }

        else
        {
            return group.Trim();
        }
    }
}
