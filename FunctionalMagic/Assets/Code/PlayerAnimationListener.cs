using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour
{
    public void PlayFootstep()
    {
        GameLogic.GetInstance().GetPlayer().PlayFootstep();
    }
}
