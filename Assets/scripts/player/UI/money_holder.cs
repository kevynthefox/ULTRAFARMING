using TMPro;
using UnityEngine;

public class money_holder : MonoBehaviour
{
    public float money;

    public TextMeshProUGUI money_text;
    public void money_update(float money_change)
    {
        money += money_change;
        money_text.text = "$" + money;
    }
}
