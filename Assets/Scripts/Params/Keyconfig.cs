using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyconfig : MonoBehaviour
{
    public class Key
    {
        static public readonly int reverb = 2; 

        public KeyCode keyCode;
        public float wheelDelta;
        public int reverbRemain;

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
                return keyCode.ToString();
            }
        }
    }

    public enum KeyAction
    {
        none,

        shot,
        reload,
        check,
        forward,
        backward,
        right,
        left,
        jump,
        crouch,
        menu,
        console,
    }

    static public Dictionary<KeyAction, Key> DefaultKeybindList { get; } = new Dictionary<KeyAction, Key>()
    {
        { KeyAction.shot, new Key(KeyCode.Mouse0) },
        { KeyAction.reload, new Key(KeyCode.R) },
        { KeyAction.check, new Key(KeyCode.F) },
        { KeyAction.forward, new Key(KeyCode.W) },
        { KeyAction.backward, new Key(KeyCode.S) },
        { KeyAction.right, new Key(KeyCode.D) },
        { KeyAction.left, new Key(KeyCode.A) },
        { KeyAction.jump, new Key(KeyCode.None, 1.0f) },
        { KeyAction.crouch, new Key(KeyCode.LeftShift) },
        { KeyAction.menu, new Key(KeyCode.Return) },
        { KeyAction.console, new Key(KeyCode.F3) },
    };

    private void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Timer.Updated += UpdateMethod;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        foreach(var keybind in KeybindList.Values)
        {
            keybind.reverbRemain--;
            if (keybind.reverbRemain < 0) { keybind.reverbRemain = 0; }
        }
    }

    static public void SetDefault()
    {
        KeybindList = new Dictionary<KeyAction, Key>(DefaultKeybindList);
    }

    static public Dictionary<KeyAction, Key> KeybindList { get; private set; } = new Dictionary<KeyAction, Key>(DefaultKeybindList);

    static public void SetKey(KeyAction keyAction, KeyCode keyCode = KeyCode.None, float wheelDelta = 0.0f)
    {
        KeybindList[keyAction].keyCode = keyCode;
        KeybindList[keyAction].wheelDelta = wheelDelta;

    }

    static public bool CheckInput(KeyAction action, bool getKeyDown)
    {
        var keybind = KeybindList[action];

        if (keybind.keyCode != KeyCode.None)
        {
            if (getKeyDown)
            {
                if (Input.GetKeyDown(keybind.keyCode))
                {
                    keybind.reverbRemain = Key.reverb;
                    return true;
                }

                else if (keybind.reverbRemain > 0)
                {
                    return true;
                }
            }

            else
            {
                if (Input.GetKey(keybind.keyCode))
                {
                    keybind.reverbRemain = Key.reverb;
                    return true;
                }
            }
        }

        // keybind.keyCode == KeyCode.None
        else
        {
            float wheelDelta = keybind.wheelDelta;

            if (wheelDelta > 0)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    keybind.reverbRemain = Key.reverb;
                    return true;
                }

                else if (keybind.reverbRemain > 0)
                {
                    return true;
                }
            }

            else
            {
                if (Input.mouseScrollDelta.y < 0)
                {
                    keybind.reverbRemain = Key.reverb;
                    return true;
                }

                else if (keybind.reverbRemain > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
