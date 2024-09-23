using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="HandData",menuName="poker25h/HandData",order=0)]
public class HandData : ScriptableObject
{
    public int[] rank=new int[5];
    public int[] suit=new int[5];
    public bool[] hold=new bool[5];
    public int handFlag;
}
