using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarPlayerNPCFov : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField]
    private GuitarPlayerNPCController guitarPlayerNpc;

    public void OnPlayerTriggered(PlayerController player)
    {
        GameController.Instance.OnEnterGuitarPlayerNPCView(GetComponentInParent<GuitarPlayerNPCController>());
    }

    public bool TriggerRepeatedly => false;
}
