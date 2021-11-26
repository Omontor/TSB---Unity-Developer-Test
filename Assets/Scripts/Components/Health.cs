using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Health : IComponentData
{
    public float health;
    public bool ishit;
    
}
