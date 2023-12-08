using System;
using UnityEngine;

namespace _Project.Scripts
{
    public static class GameEventPool
    {
        public static Action<TinyHouseCar> OnTinyHouseCarSpawned;
        public static Action<int> OnTinyHouseCompleted;
        public static Action<Transform> OnHireableWorkerSpawned;
        public static Action<AIHireWorkerData> OnBoughtHireWorker;
        public static Action OnMobileJoystickDown;
        public static Action OnMobileJoystickUp;
        public static Action OnCraneAreaOnBoardCompleted;
    }
}