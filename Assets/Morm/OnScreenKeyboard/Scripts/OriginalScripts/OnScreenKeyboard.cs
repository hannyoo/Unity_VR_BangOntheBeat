using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class OnScreenKeyboard : MonoBehaviour
{
    public enum CurLang {
        EN,
    }
    public enum Caps {
        Caps,
        Uncaps
    }

    private OnScreenKeyboardInputfield currentOskInputfield;
    private InputField targetInputField;
    private string inputtedString, currentString;

    [SerializeField]
    private CurLang curLang;
    private CurLang beforeLang;
    [SerializeField]
    private Caps curCaps;
    private Caps beforeCaps;
    
    [SerializeField]
    private Button bgCloseBtn;
    [SerializeField]
    private Text showTextField;
    [SerializeField]
    private GameObject EnNormalCpas;
    [SerializeField]
    private Button exitBtn, capsBtn;
    Event fakeEvent;

    void Awake()
    {
        AddListenerToButtons();

        
        curLang = CurLang.EN;
        beforeLang = CurLang.EN;
        curCaps = Caps.Caps;
        ClearAllStrValue();
        CloseKeyboard();
    }

    private void Update()
    {
        if (targetInputField == null) return;

        //hide a mobile keyboard
        ForceToCloseMobileKeyboard();

        //Detect Language 
        if (beforeLang != curLang)
        {
            beforeLang = curLang;
            ClearAllStrValue();
            ChangeKeyboardType(curLang, curCaps);
        }

        //Detect Cap 
        if (beforeCaps != curCaps)
        {
            beforeCaps = curCaps;
            ChangeKeyboardType(curLang, curCaps);
        }
    }

    void AddListenerToButtons()
    {
        bgCloseBtn.onClick.AddListener(CloseKeyboard);
        exitBtn.onClick.AddListener(CloseKeyboard);
        capsBtn.onClick.AddListener(() => {
            switch (curCaps) {
                case Caps.Caps:
                    curCaps = Caps.Uncaps;
                    break;
                case Caps.Uncaps:
                    curCaps = Caps.Caps;
                    break;
            }
        });
    }
    
    public void SendKey(string value) {
        switch (value) {
            case "backspace":
            value = "";
            inputtedString = inputtedString.Length > 1 ? inputtedString.Substring(0, inputtedString.Length - 1) : "";

                currentString = currentString.Length > 1 ? currentString.Substring(0, currentString.Length - 1) : "";
                fakeEvent = Event.KeyboardEvent("backspace");
                fakeEvent.keyCode = KeyCode.Backspace;
            
            break;
            case "space":
            fakeEvent = Event.KeyboardEvent(value);
            fakeEvent.keyCode = KeyCode.Space;
            InputFieldProcessUpdate(fakeEvent);
            value = " ";
            break;
            case "&":
            fakeEvent = Event.KeyboardEvent("a");
            fakeEvent.keyCode = KeyCode.Ampersand;
            fakeEvent.character = value[0];
            return;
            case "^":
            fakeEvent = Event.KeyboardEvent("a");
            fakeEvent.keyCode = KeyCode.Caret;
            fakeEvent.character = value[0];
            return;
            case "%":
            fakeEvent = Event.KeyboardEvent("a");
            fakeEvent.keyCode = KeyCode.Percent;
            fakeEvent.character = value[0];
            return;
            case "#":
            fakeEvent = Event.KeyboardEvent("a");
            fakeEvent.keyCode = KeyCode.Hash;
            fakeEvent.character = value[0];
            return;
            default:
            if (value.Length != 1) {
                Debug.LogError("Ignoring spurious multi-character key value: " + value);
            }
            fakeEvent = Event.KeyboardEvent(value);
            char keyChar = value[0];
            fakeEvent.character = keyChar;
            if (Char.IsUpper(keyChar)) {
                fakeEvent.modifiers |= EventModifiers.Shift;
            }
            break;
        }

        InputProcess(value);
    }

    public void ShowKeyboard(InputField inputField, OnScreenKeyboardInputfield oskInputField) {
        Initialize(inputField, oskInputField);
        InputProcess();
        gameObject.SetActive(true);
    }

    private void CloseKeyboard()
    {
        currentOskInputfield?.SaveInputedString(inputtedString);
        gameObject.SetActive(false);
    }
    
    private void InputProcess(string value)
    {
        inputtedString += value;
        currentString += value;

        InputProcess();
    }
    
    private void InputProcess()
    {
        InputFieldProcessUpdate(fakeEvent);

            targetInputField.text = currentString;
            currentString = GetInputFieldText();
            showTextField.text =  GetInputFieldText();
        
    }
    
    private void Initialize(InputField inputField, OnScreenKeyboardInputfield oskInputField)
    {
        targetInputField = inputField;
        currentOskInputfield = oskInputField;
        inputtedString = oskInputField.inputtedString;

        //Set default state
        if (string.IsNullOrEmpty(targetInputField.text))
        {
            //Do check your language
            curCaps = Caps.Uncaps;
        }
        
        ChangeKeyboardType(curLang, curCaps);
        ForceToCloseMobileKeyboard();
    }
    
    void ChangeKeyboardType(CurLang curLang, Caps caps) {

        this.curLang = curLang;
        this.curCaps = caps;
        
        switch (curLang) {
            case CurLang.EN:
                switch (caps) {
                    case Caps.Caps:
                        EnNormalCpas.gameObject.SetActive(true);
                        break;
                    case Caps.Uncaps:
                        break;
                }
                break;
        }
    }

    private void SetInputFieldText(string str)
    {
        if (targetInputField)
            targetInputField.text = str;
    }

    private string GetInputFieldText()
    {
        if (targetInputField)
            return targetInputField.text;
        else
            return "";
    }

    private void InputFieldProcessUpdate(Event fakeEvent)
    {
        if (targetInputField)
        {
            if(fakeEvent != null)
                targetInputField.ProcessEvent(fakeEvent);
            targetInputField.ForceLabelUpdate();
        }
    }

    private void ForceToCloseMobileKeyboard()
    {
        if (targetInputField == null) return;

        if (targetInputField.touchScreenKeyboard != null)
            targetInputField.touchScreenKeyboard.active = false;
    }
    
    private void ClearAllStrValue()
    {
        inputtedString = "";
        currentString = "";
        showTextField.text = "";
        
        if(targetInputField)
            targetInputField.text = "";
    }

}
