using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations.Rigging;

public class IKConstraintTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TwoBoneIKConstraint twoBoneConstraint = playerData as TwoBoneIKConstraint;
        float currentValue = 0f;

        if (!twoBoneConstraint) return;

        int inputCount = playable.GetInputCount();
        for(int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0)
            {
                ScriptPlayable<IKConstraintBehaviour> inputPlayable = (ScriptPlayable<IKConstraintBehaviour>)playable.GetInput(i);

                IKConstraintBehaviour input = inputPlayable.GetBehaviour();
                currentValue = input.IKValue;
            }
        }

        twoBoneConstraint.weight = currentValue;
    }
}
