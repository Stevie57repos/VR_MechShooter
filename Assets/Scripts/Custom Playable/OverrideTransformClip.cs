using UnityEngine;
using UnityEngine.Playables;

public class OverrideTransformClip : PlayableAsset
{
    public float IKValue = 0;
    public AnimationCurve IKCurve;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<OverrideTransformBehaviour>.Create(graph);

        OverrideTransformBehaviour overrideTransformBehaviour = playable.GetBehaviour();
        overrideTransformBehaviour.IKValue = IKValue;
        overrideTransformBehaviour.IKCurve = IKCurve;
        return playable;
    }
}