using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeCommandHandler : MonoBehaviour
{
    [SerializeField] private string skillName;

    private const int COMMAND_INDEX = 0, CHANGE_BUTTON_INDEX = 1;
    private bool change = false;
    private bool isRecode = false;

    private List<string> commands;
    private TextMeshProUGUI currentChangeCommand;
    private GameObject changeButton;

    void Awake()
    {
        commands = new List<string>();
        currentChangeCommand = transform.GetChild(COMMAND_INDEX).GetComponent<TextMeshProUGUI>();
        changeButton = transform.GetChild(CHANGE_BUTTON_INDEX).gameObject;
    }

    public void Change()
    {
        if (change)
        {
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Change ?";
            change = false;
            StopRecode();
        }
        else
        {
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Change !";
            change = true;
            StartRecode();
        }
    }

    public void StartRecode()
    {
        currentChangeCommand.text = string.Empty;
        commands.Clear();
        InputActionManager.Manager.Inputs.Command.CommandInput.started += CommandRecoding;
    }

    public void StopRecode()
    {
        InputActionManager.Manager.Inputs.Command.CommandInput.started -= CommandRecoding;
        InputActionManager.Manager.comboInputHandler.ChangeCombo(skillName, commands);
        GameManager.Manager.SkillCommand[skillName] = commands;
    }

    private void CommandRecoding(InputAction.CallbackContext context)
    {
        commands.Add(context.control.name.ToUpper());
        currentChangeCommand.text +=
            CommandToCharacter(context.control.name.ToUpper()) + " ";
    }

    public static char CommandToCharacter(string command)
    {
        switch (command)
        {
            case "UPARROW":
                return '↑';
            case "DOWNARROW":
                return '↓';
            case "LEFTARROW":
                return '←';
            case "RIGHTARROW":
                return '→';
            default:
                return command.ToCharArray()[0];
        }
    }
}