using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ObjectID : IComponentData
{

    public bool isAsteroid;
    public bool isShot;
    public bool isPlayer;
    public bool isChunk;
}
