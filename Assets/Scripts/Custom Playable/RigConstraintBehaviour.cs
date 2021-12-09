using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations.Rigging;

public class RigConstraintBehaviour : PlayableBehaviour
{
    public float IKValue;
    public AnimationCurve IKCurve;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Rig rig = playerData as Rig;
        IKValue = info.weight;
        rig.weight = IKCurve.Evaluate(IKValue);
    }
}