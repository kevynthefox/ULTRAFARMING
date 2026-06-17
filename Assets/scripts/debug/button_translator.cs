using UnityEngine;
using UnityEngine.UI;

public class button_translator : MonoBehaviour
{
    public Button my_button;

    public void clicked_on()
    {
        my_button.onClick.Invoke();
    }
}
