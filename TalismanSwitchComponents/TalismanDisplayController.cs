using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TalismanSwitchComponents {
    public class TalismanDisplayController : MonoBehaviour {

        [SerializeField]
        public Animator _animator = null!;

        private const string ANIMATOR_BLAST_CLIP = "Blast";
        private const string ANIMATOR_BLAST_UPG_CLIP = "BlastU";

        private const string ANIMATOR_FLOW_CLIP = "Flow";
        private const string ANIMATOR_FLOW_UPG_CLIP = "FlowU";

        private const string ANIMATOR_FC_CLIP = "Control";
        private const string ANIMATOR_FC_UPG_CLIP = "ControlU";

        private bool _pendingUpdate = false;

        private EnabledTalisman _enabledTalisman = EnabledTalisman.None;

        public EnabledTalisman EnabledTalisman {
            get => _enabledTalisman;
            set {
                _enabledTalisman = value;
                UpdateDisplay();
            }
        }

        public void RequestUpdate() {
            _pendingUpdate = true;
        }

        private void Update() {
            if (_pendingUpdate) {
                _pendingUpdate = false;
                UpdateDisplay();
            }
        }

        private void UpdateDisplay() {
            switch (EnabledTalisman) {
                case EnabledTalisman.None:
                    _animator.Play("None");
                    break;
                case EnabledTalisman.QiBlast:
                    _animator.Play(ANIMATOR_BLAST_CLIP);
                    break;
                case EnabledTalisman.QiBlastUpgraded:
                    _animator.Play(ANIMATOR_BLAST_UPG_CLIP);
                    break;
                case EnabledTalisman.WaterFlow:
                    _animator.Play(ANIMATOR_FLOW_CLIP);
                    break;
                case EnabledTalisman.WaterFlowUpgraded:
                    _animator.Play(ANIMATOR_FLOW_UPG_CLIP);
                    break;
                case EnabledTalisman.FullControl:
                    _animator.Play(ANIMATOR_FC_CLIP);
                    break;
                case EnabledTalisman.FullControlUpgraded:
                    _animator.Play(ANIMATOR_FC_UPG_CLIP);
                    break;
            }
        }
    }
}
