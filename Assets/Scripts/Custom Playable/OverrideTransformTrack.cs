using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;

[TrackBindingType(typeof(OverrideTransform))]
[TrackClipType(typeof(OverrideTransformClip))]

public class OverrideTransformTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<OverrideTransformTrackMixer>.Create(graph, inputCount);
    }
}

