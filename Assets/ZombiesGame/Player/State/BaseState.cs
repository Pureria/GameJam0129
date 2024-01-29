using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.State
{
    public class BaseState
    {
        protected bool _animationFinished;
        protected bool _endState;
        protected float _startTime;

        private string _animName;
        private Animator _anim;

        public bool EndState => _endState;
        
        public BaseState(Animator anim, string animName)
        {
            _anim = anim;
            _animName = animName;
        }
        
        public virtual void Enter()
        {
            _anim.SetBool(_animName, true);
            
            _animationFinished = false;
            _startTime = Time.time;
            _endState = false;
        }

        public virtual void Exit()
        {
            _anim.SetBool(_animName, false);
            _endState = true;
        }

        public virtual void LogicUpdate()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }

        public virtual void AnimationTrigger()
        {
            
        }

        public void AnimationFinishTrigger() => _animationFinished = true;
    }
}
