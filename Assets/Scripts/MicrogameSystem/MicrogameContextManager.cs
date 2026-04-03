using System;
using System.Collections.Generic;
using MicrogameSystem;
using SerializedInterface;
using UnityEngine;
using UnityUtils;
using Random = UnityEngine.Random;

public class MicrogameContextManager : MonoBehaviour
{
    [SerializeField] private List<InterfaceReference<IMicrogameContext>> workMicrogames;
    [SerializeField] private List<InterfaceReference<IMicrogameContext>> personalMicrogames;

    private void Update()
    {
        if (InputManager.Instance.InputReader.PlayMicrogame.WasPressedThisFrame)
            StartRandomMicrogame();
    }

    public void StartRandomMicrogame()
    {
        var microgame = workMicrogames.Random();
        microgame.Value.StartMicrogame();
    }
}
