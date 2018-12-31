using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapToggle : Toggle {

    private void Update()
    {
        if(isOn)
        {
            targetGraphic.enabled = false;
        }
        else
        {
            targetGraphic.enabled = true;
        }
    }
}
