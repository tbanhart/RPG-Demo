using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : StateHandlerBase
{
    public PlayerHandler(GameObject owner, GameObject hand, GameObject sheathe, Camera cam, GameObject gui)
     : base(owner, hand, sheathe, cam, gui){ }
}
