using UnityEngine;
using UnityEngine.Playables;

public class RigConstraintClip : PlayableAsset
{
    public float IKValue = 0;
    public AnimationCurve IKCurve;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<RigConstraintBehaviour>.Create(graph);

        RigConstraintBehaviour rigBehaviour = playable.GetBehaviour();
        rigBehaviour.IKValue = IKValue;
        rigBehaviour.IKCurve = IKCurve;
        return playable;
    }
}
