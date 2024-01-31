using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.State
{
    [CreateAssetMenu(fileName = "PlayerStateEventSO", menuName = "Zombies/PlayerStateEvent")]
    public class PlayerStateEvent : ScriptableObject
    {
        [HideInInspector] public Action<Vector2, float> MoveEvent;
        [HideInInspector] public Action ViewPointEvent;
        [HideInInspector] public Action ShotEvent;
        [HideInInspector] public Action InteractEvent;
        [HideInInspector] public Action ReloadEvent;
        [HideInInspector] public Action ChangeWeaponEvent;
        [HideInInspector] public Action GamePlayEvent;
        [HideInInspector] public Action LastStandEvent;
    }
}
