using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TalismanSwitch.HarmonyPatches {
    internal class ControlsButtonDescriptor {
        internal GameObject ButtonObject { get; set; }

        public ControlsButtonDescriptor(GameObject buttonObject) {
            ButtonObject = buttonObject;
        }
    }
}
