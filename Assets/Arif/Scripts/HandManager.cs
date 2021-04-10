using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arif.Scripts
{
    public class HandManager : MonoBehaviour
    {
        public static HandManager instance;

        public HandController handController;

       

        private void Awake()
        {
            instance = this;
        }
    }
}
