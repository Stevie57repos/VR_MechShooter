using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations.Rigging;

public class IKConstraintBehaviour : PlayableBehaviour
{
    public float IKValue;
    public AnimationCurve IKCurve;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TwoBoneIKConstraint twoBoneConstraint = playerData as TwoBoneIKConstraint;
        //twoBoneConstraint.weight = info.weight;
        //Debug.Log($"weight is {info.weight} and twobone is {twoBoneConstraint.weight}");

        //twoBoneConstraint.weight = IKCurve.Evaluate(info.weight);
        //Debug.Log($"IK Curve is {IKCurve.Evaluate(info.weight)} and twobone is {twoBoneConstraint.weight}");

        IKValue = info.weight;
        twoBoneConstraint.weight = IKCurve.Evaluate(IKValue);
        Debug.Log($"IK Curve is {IKCurve.Evaluate(info.weight)} and twobone is {twoBoneConstraint.weight}");
        //IKValue += Time.fixedDeltaTime;

        //float frameDataInfo = Mathf.RoundToInt(info.weight * 1000);
        //float weight = Mathf.Clamp(frameDataInfo , 0f, 1f);
        //twoBoneConstraint.weight = weight;
        //Debug.Log($"FrameData info is {info.weight} and weight is {weight} and constraint is {twoBoneConstraint.weight}");

        //float testWeight = 0.5f;
        //twoBoneConstraint.weight = testWeight;
        //Debug.Log($"I've set it to {testWeight} and it is currently {twoBoneConstraint.weight} and valid check is {twoBoneConstraint.IsValid()}");

        //twoBoneConstraint.weight = 0.5f;
    }
}
