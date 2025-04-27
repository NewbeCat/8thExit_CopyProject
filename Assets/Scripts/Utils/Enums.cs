#region Input Actions
public enum EInputAction
{
    Move,
    Look,
    Sprint,
}
#endregion

#region Player States
public enum EPlayerMovement
{
    Idle,
    Walk,
    Sprint,
}
#endregion
#region Sound Types
public enum ESoundClip
{
    Ambience,
    Walk,
    Run,
    WalkBlood,
    RunBlood,
    ScreenVideo,
    SpeakerLoop,
    SpeakerOneShot,
    ScreenHand,
    LightTurnoff,
    LightTurnon,
    Spotlight,
    DarkComing,
    StokerLoop,
    StokerOneShot,
    StokerCatch,
    AmbienceEnding,
    Flim,
}
#endregion
#region Addressable
public enum EAddressableLabel
{
    Ambience,
    FootStep,
    ScreenVideo,
    Speaker,
    Light,
    DarkComing,
    Stoker,
    Screen,
}
#endregion