using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSetup : MonoBehaviour
{
    public KeyCode jumpKey;
    public KeyCode walkKey;
    public KeyCode meleeKey;
    public KeyCode shootKey;

    private void Awake()
    {
        OptionSetting.jumpKey = jumpKey;
        OptionSetting.walkKey = walkKey;
        OptionSetting.meleeKey = meleeKey;
        OptionSetting.shootKey = shootKey;
    }
}

