using System.Collections.Generic;
using UnityEngine;

public class Character2 : Character
{
    public override void Interagir(Character character)
    {
        character.Humor += 100;
        character.Age += 100;
    }


}

