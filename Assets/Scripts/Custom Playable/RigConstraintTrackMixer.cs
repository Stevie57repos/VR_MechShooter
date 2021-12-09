using UnityEngine.Playables;
using UnityEngine.Animations.Rigging;

public class RigConstraintTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Rig rig = playerData as Rig;
        float currentValue = 0f;

        if (!rig) return;

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0)
            {
                ScriptPlayable<RigConstraintBehaviour> inputPlayable = (ScriptPlayable<RigConstraintBehaviour>)playable.GetInput(i);

                RigConstraintBehaviour input = inputPlayable.GetBehaviour();
                currentValue = input.IKValue;
            }
        }

        rig.weight = currentValue;
    }
}
