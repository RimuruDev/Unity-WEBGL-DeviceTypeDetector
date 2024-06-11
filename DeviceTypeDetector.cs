// **************************************************************** //
//
//   Copyright (c) RimuruDev. All rights reserved.
//   Contact me: 
//          - Gmail:    rimuru.dev@gmail.com
//          - LinkedIn: https://www.linkedin.com/in/rimuru/
//          - GitHub:   https://github.com/RimuruDev
//
// **************************************************************** //

using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.DeviceSimulation;
#endif

namespace RimuruDev
{
    [Flags]
    [Serializable]
    public enum CurrentDeviceType : byte
    {
        WebPC = 0,
        WebMobile = 2,
    }

    [SelectionBase]
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    [HelpURL("https://github.com/RimuruDev/Unity-WEBGL-DeviceTypeDetector")]
    public sealed class DeviceTypeDetector : MonoBehaviour
    {
        [field: SerializeField] public CurrentDeviceType CurrentDeviceType { get; private set; }

#if UNITY_2020_1_OR_NEWER
        [SerializeField] private bool enableDeviceSimulator = true;
#endif
        private void Awake()
        {
            if (IsMobile() && enableDeviceSimulator)
            {
                Debug.Log("WEBGL -> Mobile");
                CurrentDeviceType = CurrentDeviceType.WebMobile;
            }
            else
            {
                Debug.Log("WEBGL -> PC");
                CurrentDeviceType = CurrentDeviceType.WebPC;
            }
        }

#if UNITY_EDITOR
        public static bool IsMobile()
        {
#if UNITY_2020_1_OR_NEWER
            if (DeviceSimulatorExists() && IsDeviceSimulationActive())
                return true;
#endif
            return false;
        }

        private static bool DeviceSimulatorExists()
        {
            var simulatorType = typeof(Editor).Assembly.GetType("UnityEditor.DeviceSimulation.DeviceSimulator");
            return simulatorType != null;
        }

        private static bool IsDeviceSimulationActive()
        {
            var simulatorType = typeof(Editor).Assembly.GetType("UnityEditor.DeviceSimulation.DeviceSimulator");
            if (simulatorType != null)
            {
                var simulatorInstance = simulatorType.GetProperty("instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)?.GetValue(null);
                var isDeviceActive = simulatorType.GetProperty("isDeviceActive", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(simulatorInstance);
                return (bool)isDeviceActive;
            }
            return false;
        }
#else
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern bool IsMobile();
#endif
    }
}