using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
    public class VersionDisplayer : MonoBehaviour
    {
        private Text versionText;

        private void Awake()
        {
            versionText = GetComponent<Text>();
        }

        private void Start()
        {
            versionText.text = string.Format("Version {0}", Application.version);
        }
    }
}