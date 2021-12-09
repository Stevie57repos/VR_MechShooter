using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;

[TrackBindingType(typeof(TwoBoneIKConstraint))]
[TrackClipType(typeof(IKConstraintClip))]
public class IKConstraintTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<IKConstraintTrackMixer>.Create(graph, inputCount);
    }
}
