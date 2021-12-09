using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations.Rigging;

public class OverrideTransformBehaviour : PlayableBehaviour
{
    public float IKValue;
    public AnimationCurve IKCurve;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        OverrideTransform overrideTransform = playerData as OverrideTransform;

        IKValue = info.weight;
        overrideTransform.weight = IKCurve.Evaluate(IKValue);
    }
}