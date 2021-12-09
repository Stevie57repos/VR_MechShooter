using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;

[TrackBindingType(typeof(Rig))]
[TrackClipType(typeof(RigConstraintClip))]
public class RigConstraintTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<RigConstraintTrackMixer>.Create(graph, inputCount);
    }
}