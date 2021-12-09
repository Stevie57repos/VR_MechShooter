using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IKConstraintClip : PlayableAsset
{
    public float IKValue = 0;
    public AnimationCurve IKCurve;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<IKConstraintBehaviour>.Create(graph);

        IKConstraintBehaviour ikBehaviour = playable.GetBehaviour();
        ikBehaviour.IKValue = IKValue;
        ikBehaviour.IKCurve = IKCurve;
        return playable;
    }
}
