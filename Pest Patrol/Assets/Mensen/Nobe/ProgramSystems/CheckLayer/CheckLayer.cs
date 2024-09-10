using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLayer
{

    public static bool LayerCheck(LayerMask layer1, LayerMask layer2)
    {
        if(layer1 == layer2) return true;
        else return false;
    }

}
