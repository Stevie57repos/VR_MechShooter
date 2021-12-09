using UnityEngine.Playables;
using UnityEngine.Animations.Rigging;

public class OverrideTransformTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        OverrideTransform overrideTransform = playerData as OverrideTransform;
        float currentValue = 0f;

        if (!overrideTransform) return;

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0)
            {
                ScriptPlayable<OverrideTransformBehaviour> inputPlayable = (ScriptPlayable<OverrideTransformBehaviour>)playable.GetInput(i);

                OverrideTransformBehaviour input = inputPlayable.GetBehaviour();
                currentValue = input.IKValue;
            }
        }
        overrideTransform.weight = currentValue;
    }
}
