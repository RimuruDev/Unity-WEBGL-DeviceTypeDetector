// **************************************************************** //
//
//   Copyright (c) RimuruDev. All rights reserved.
//   Contact me:
//          - Gmail: rimuru.dev@gmail.com  
//          - LinkedIn: https://www.linkedin.com/in/rimuru/
//          - GitHub: https://github.com/RimuruDev
//
// **************************************************************** //

using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RimuruDev
{
    [Serializable]
    public enum CurrentDeviceType : byte
    {
        None = 0,
        WebPC = 2,
        WebMobile = 4,
    }

    [SelectionBase]
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    [HelpURL("https://github.com/RimuruDev/Unity-WEBGL-DeviceTypeDetector")]
    public sealed class DeviceTypeDetector : MonoBehaviour
    {
#if UNITY_EDITOR
        private const string WINDOW_TITLE_SIMULATOR = "Simulator";
        private const string WINDOW_TITLE_SIMULATOR_DEVICE = "Simulator Device";
#endif
        [field: SerializeField] public CurrentDeviceType CurrentDeviceType { get; private set; }

#if UNITY_2020_1_OR_NEWER
        [SerializeField] private bool enableDeviceSimulator = true;
#endif

        private void Awake()
        {
#if UNITY_EDITOR
            if (IsSimulatorWindowOpen() && enableDeviceSimulator)
            {
                Debug.Log("WEBGL -> Mobile");
                CurrentDeviceType = CurrentDeviceType.WebMobile;
            }
            else
            {
                Debug.Log("WEBGL -> PC");
                CurrentDeviceType = CurrentDeviceType.WebPC;
            }
#else
            if (IsMobile())
            {
                Debug.Log("WEBGL -> Mobile");
                CurrentDeviceType = CurrentDeviceType.WebMobile;
            }
            else
            {
                Debug.Log("WEBGL -> PC");
                CurrentDeviceType = CurrentDeviceType.WebPC;
            }
#endif
        }

#if UNITY_EDITOR
        private static bool IsSimulatorWindowOpen() =>
            Resources
                .FindObjectsOfTypeAll<EditorWindow>()
                .Any(window => window.titleContent.text
                    is WINDOW_TITLE_SIMULATOR
                    or WINDOW_TITLE_SIMULATOR_DEVICE);
#endif

#if !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern bool IsMobile();
#endif
    }
}