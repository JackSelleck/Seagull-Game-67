using UnityEngine;

public struct PlayerAnimHash
{
    public static readonly int Speed = Animator.StringToHash("Speed");
    public static readonly int Moving = Animator.StringToHash("Moving");
    public static readonly int Grounded = Animator.StringToHash("Grounded");
    public static readonly int Glide = Animator.StringToHash("Glide");
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int SprintButton = Animator.StringToHash("SprintButton");
}
