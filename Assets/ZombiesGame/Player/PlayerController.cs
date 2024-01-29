using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Input;

namespace Zombies.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private InputSO _inputSO;

        [SerializeField] private PlayerStateInfo _stateInfo;
    }
}
