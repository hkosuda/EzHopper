using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Suggest : MonoBehaviour
{
    static public Dictionary<string, GameObject> SuggestButtons { get; private set; } = new Dictionary<string, GameObject>();
    static public List<GameObject> ActiveButtons { get; private set; } = new List<GameObject>();

    static GameObject _suggestButton;

    static GameObject suggestPanel;
    static GameObject suggestView;
    static GameObject suggestContent;

    static GameObject myself;

    static readonly int viewHeight = 150;
    static readonly  int viewWidth = 260;

    static readonly int frameWidth = 8;

    static readonly int buttonHeight = 20;
    static readonly int buttonOffset = 15;

    private void Awake()
    {
        InitialSetup(gameObject);
        LoadSuggestButton();

        ShowSuggest(null, "");
    }

    private void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void Update()
    {
        Apply();
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            ConsoleInputField.ValueUpdated += ShowSuggest;
        }

        else
        {
            ConsoleInputField.ValueUpdated -= ShowSuggest;
        }
    }

    static void LoadSuggestButton()
    {
        if (_suggestButton == null)
        {
            _suggestButton = Resources.Load<GameObject>("UiComponent/SuggestButton");
        }
    }

    static void InitialSetup(GameObject _myself)
    {
        myself = _myself;

        suggestPanel = myself.transform.GetChild(0).gameObject;
        suggestView = suggestPanel.transform.GetChild(0).gameObject;
        suggestContent = suggestView.transform.GetChild(0).GetChild(0).gameObject;
        suggestContent.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
    }

    static void ShowSuggest(object obj, string content)
    {
        InactivateAllButtons();

        var suggestCommands = GetSuggestCommands(content);
        ActivateSuggestButtons(suggestCommands);

        UpdatePanelSize();

        // function
        static List<string> GetSuggestCommands(string content)
        {
            content = content.TrimStart();

            var suggestCommands = new List<string>();
            if (content == "") { return suggestCommands; }

            foreach (var command in CommandReceiver.CommandList.Values)
            {
                if (command.commandName.StartsWith(content))
                {
                    suggestCommands.Add(command.commandName);
                }
            }

            return suggestCommands;
        }

        // function
        static void InactivateAllButtons()
        {
            foreach (var button in SuggestButtons.Values)
            {
                button.SetActive(false);
            }
        }

        // function
        static void ActivateSuggestButtons(List<string> suggestCommands)
        {
            ActiveButtons = new List<GameObject>();

            foreach (var suggestCommand in suggestCommands)
            {
                if (SuggestButtons.ContainsKey(suggestCommand))
                {
                    SuggestButtons[suggestCommand].SetActive(true);
                }

                else
                {
                    var suggestButton = Instantiate(_suggestButton);

                    suggestButton.transform.SetParent(suggestContent.transform);
                    suggestButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = suggestCommand;
                    suggestButton.GetComponent<RectTransform>().sizeDelta = new Vector2(viewWidth - 2.0f * buttonOffset, buttonHeight);

                    SuggestButtons.Add(suggestCommand, suggestButton);
                }

                ActiveButtons.Add(SuggestButtons[suggestCommand]);
            }
        }

        // function
        static void UpdatePanelSize()
        {
            var heightSum = 0.0f;

            foreach (var button in SuggestButtons.Values)
            {
                if (!button.activeSelf) { continue; }
                heightSum += button.GetComponent<RectTransform>().sizeDelta.y;
            }

            if (heightSum == 0.0f)
            {
                suggestPanel.SetActive(false);
            }

            else
            {
                if (heightSum > viewHeight)
                {
                    suggestPanel.SetActive(true);
                    suggestPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(viewWidth + 2.0f * frameWidth, viewHeight + 2.0f * frameWidth);
                    suggestView.GetComponent<RectTransform>().sizeDelta = new Vector2(viewWidth, viewHeight);
                }

                else
                {
                    suggestPanel.SetActive(true);
                    suggestPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(viewWidth + 2.0f * frameWidth, heightSum + 2.0f * frameWidth);
                    suggestView.GetComponent<RectTransform>().sizeDelta = new Vector2(viewWidth, heightSum);
                }
            }
        }
    }

    static void Apply()
    {
        if (ActiveButtons == null) { return; }
        if (ActiveButtons.Count == 0) { return; }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Apply");

            var button = ActiveButtons[0];
            button.GetComponent<Button>().onClick?.Invoke();
        }
    }
}
