namespace Common.GenericStateMachine
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GenericStateFactory<TStateIds, TStateInfo, TStateMachine>
        where TStateIds : System.Enum
        where TStateInfo : GenericStateInfo
        where TStateMachine : GenericStateMachine<TStateIds, TStateInfo>
    {

        public abstract GenericStateBase<TStateIds, TStateInfo> CreateState(TStateIds stateId, TStateMachine stateMachine);

        public virtual IEnumerable<GenericStateBase<TStateIds, TStateInfo>> GetStates(TStateMachine stateMachine) 
        {
            TStateIds[] initialStates = (TStateIds[])Enum.GetValues(typeof(TStateIds));

            if (initialStates == null || initialStates.Length <= 0)
                yield return null;

            foreach (TStateIds stateId in initialStates)
                yield return CreateState(stateId, stateMachine);
        }
    }
}