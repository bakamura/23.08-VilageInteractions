using System.Collections.Generic;
using UnityEngine;

public class CharacterMori : Character
{
    public override void Interagir(Character character)
    {
        character.Humor += 1;
        character.Age += 1;
    }


}

