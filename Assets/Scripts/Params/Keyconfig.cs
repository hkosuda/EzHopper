using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyconfig : MonoBehaviour
{
    static public EventHandler<KeyAction> KeyUpdated { get; set; }

    public class Key
    {
        public KeyCode keyCode;
        public float wheelDelta;

        public Key(KeyCode keyCode, float wheelDelta = 1.0f)
        {
            this.keyCode = keyCode;
            this.wheelDelta = wheelDelta;
        }

        public string GetKeyString()
        {
            if (keyCode == KeyCode.None)
            {
                if (wheelDelta > 0) { return "+wheel"; }
                return "-wheel";
            }

            else
            {
                return keyCode.ToString().ToLower();
            }
        }
    }

    public enum KeyAction
    {
        none,

        jump,
        autoJump,
        forward,
        backward,
        right,
        left,
        shot,
        menu,
        console,
    }

    static public Dictionary<KeyAction, Key> DefaultKeybindList { get; } = new Dictionary<KeyAction, Key>()
    {
        { KeyAction.jump, new Key(KeyCode.Space) },
        { KeyAction.autoJump, new Key(KeyCode.Mouse1) },
        { KeyAction.forward, new Key(KeyCode.W) },
        { KeyAction.backward, new Key(KeyCode.S) },
        { KeyAction.right, new Key(KeyCode.D) },
        { KeyAction.left, new Key(KeyCode.A) },
        { KeyAction.shot, new Key(KeyCode.Mouse0) },
        { KeyAction.menu, new Key(KeyCode.M) },
        { KeyAction.console, new Key(KeyCode.K) },
    };

    static public Dictionary<KeyAction, Key> KeybindList { get; private set; } = new Dictionary<KeyAction, Key>(DefaultKeybindList);

    static public void SetKey(KeyAction keyAction, KeyCode keyCode = KeyCode.None, float wheelDelta = 0.0f)
    {
        if (keyCode == KeyCode.None)
        {
            if (wheelDelta == 0.0f)
            {
                KeybindList[keyAction].keyCode = KeyCode.None;
                KeybindList[keyAction].wheelDelta = 1.0f;
            }

            else
            {
                KeybindList[keyAction].keyCode = KeyCode.None;
                KeybindList[keyAction].wheelDelta = wheelDelta;
            }
        }

        else
        {
            KeybindList[keyAction].keyCode = keyCode;
            KeybindList[keyAction].wheelDelta = 0.0f;
        }

        KeyUpdated?.Invoke(null, keyAction);
    }

    static public bool CheckInput(KeyAction action, bool getKeyDown)
    {
        var key = KeybindList[action];

        return InputSystem.CheckInput(key, getKeyDown);
    }

    static public Key StringToKey(string str)
    {
        str = str.ToLower();

        if (int.TryParse(str, out var num))
        {
            if (num > 0)
            {
                return new Key(KeyCode.None, 1);
            }

            else if (num < 0)
            {
                return new Key(KeyCode.None, -1);
            }

            else
            {
                return null;
            }
        }

        foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (keyCode == KeyCode.None) { continue; }

            if (str == keyCode.ToString().ToLower())
            {
                return new Key(keyCode);
            }
        }

        return null;
    }

    // for only keycheck command
    private void Update()
    {
        if (!KeycheckCommand.Active) { return; }
        if (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0.0f) { return; }

        foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                KeycheckCommand.EchoInputKey(keyCode.ToString());
            }
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            KeycheckCommand.EchoInputKey("1");
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            KeycheckCommand.EchoInputKey("-1");
        }
    }
}
