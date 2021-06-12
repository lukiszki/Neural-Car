using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ValueContaier : MonoBehaviour
{
    public static Values value = SaveSystem.LoadValues();
    public ValueContaier()
    {
        value = SaveSystem.LoadValues();
    }
    
}
