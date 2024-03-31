using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    // Start is called before the first frame update
    public void Attack();

    // Update is called once per frame
    public void UpdateAction();
}
