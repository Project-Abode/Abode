using UnityEngine;
using System.Collections;
using System;

namespace ExitGames.SportShooting
{
    public abstract class BaseGameState
    {
        /// <summary>
        /// Requested on state start
        /// </summary>
        public virtual void InitState()
        {
            //Notification about state changed
            Debug.Log("State Changed to: " + this);
        }

        /// <summary>
        /// Calls every frame when state is active
        /// </summary>
        public virtual void ExecuteState()
        {
            //Nothing here
        }

        /// <summary>
        /// Called before destroy state
        /// </summary>
        public virtual void FinishState()
        {
            //Nothing here
        }
    }
}
