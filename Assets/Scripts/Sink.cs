using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Sink : Food
{
    public Transform water;
    public Transform low;
    public Transform high;

    override public void FixedUpdate()
    {
        base.FixedUpdate();
        water.position = low.position + (high.position - low.position) * 百分比;
    }
}