using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BindsText : MonoBehaviour
{
    static readonly Bools.Item item = Bools.Item.show_help;

    static Text text;

    private void Awake()
    {
        text = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    void Start()
    {
        UpdateVisibility(null, false);
        UpdateContent();

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
            Bools.Settings[item].ValueUpdated += UpdateVisibility;
            BindCommand.BindingUpdated += OnBindUpdated;
            ToggleCommand.ToggleUpdated += OnToggleUpdated;
        }

        else
        {
            Bools.Settings[item].ValueUpdated -= UpdateVisibility;
            BindCommand.BindingUpdated -= OnBindUpdated;
            ToggleCommand.ToggleUpdated -= OnToggleUpdated;
        }
    }

    static void UpdateVisibility(object obj, bool prev)
    {
        text.gameObject.SetActive(Bools.Get(item));
    }

    static void OnBindUpdated(object obj, bool mute)
    {
        UpdateContent();
    }

    static void OnToggleUpdated(object obj, bool mute)
    {
        UpdateContent();
    }

    static void UpdateContent()
    {
        var content = "";
        
        foreach(var bind in BindCommand.KeyBindingList)
        {
            content += bind.key.GetKeyString() + "\t| " + bind.command + "\n";
        }

        foreach(var toggle in ToggleCommand.ToggleGroupList)
        {
            content += toggle.key.GetKeyString() + "\t| " + toggle.command1 + " \t|| " + toggle.command2 + "\n";
        }

        text.text = content;
    }
}
