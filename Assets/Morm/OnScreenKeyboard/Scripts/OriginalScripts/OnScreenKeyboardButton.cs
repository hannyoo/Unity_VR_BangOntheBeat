using UnityEngine.UI;
using UnityEngine;

public class OnScreenKeyboardButton : MonoBehaviour
{
    Button myBtn;
    private OnScreenKeyboard _onScreenKeyboard;

    private void Awake() {
        myBtn = GetComponent<Button>();
        _onScreenKeyboard = GetComponentInParent<OnScreenKeyboard>();
        
        myBtn.onClick.AddListener(() => {
            _onScreenKeyboard?.SendKey(myBtn.gameObject.name);
        });
    }
}
