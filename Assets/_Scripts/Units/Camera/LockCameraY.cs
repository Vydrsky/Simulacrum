using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Y coordinate
/// in a certain range
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraY : CinemachineExtension {
    
    private float reachedPos=float.MinValue;

    public float minYPosition = 0;
    public float maxYPosition = 22;
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
        if (stage == CinemachineCore.Stage.Body) {
            var pos = state.RawPosition;
            if (reachedPos < pos.x)
                reachedPos = pos.x;
            pos.y = Mathf.Clamp(pos.y,minYPosition,maxYPosition);   //clamp camera in the level
            pos.x = Mathf.Clamp(pos.x,reachedPos,Mathf.Infinity);   //clamp camera from left
            state.RawPosition = pos;
        }
    }
}
