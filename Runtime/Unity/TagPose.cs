using UnityEngine;

namespace AprilTag {

//
// Tag pose structure for storing an estimated pose
//
public struct TagPose
{
    public Interop.Detection Detection { get; }
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }

    public TagPose(Interop.Detection detection, Vector3 position, Quaternion rotation)
    {
        Detection = detection;
        Position = position;
        Rotation = rotation;
    }
}

} // namespace AprilTag
