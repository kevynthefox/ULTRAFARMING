using System;
using UnityEngine;

public class disable_after_start : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
}
