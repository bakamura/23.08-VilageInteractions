using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterExample : Character
{
    //Para criarmos o personagem duplique esse script e troque o nome acima de "CharacterExample" para o nome do script que voce criou
    void Awake()
    {
        //A Funcao "periodToLocation" sera usada para definir o local que os personagens devem estar baseado no momento do dia, para usarmos ela:

        //Usar a funcao de forma que sempre o primeiro parametro seja o periodo que a atividade eh realizada;
        //Sabendo que o horario eh atualmente eh contado no intervalo das 10 da manha ate as 16
        //Para por exemplo falarmos ao nosso npc ir a um local as 12 horas, colocariamos o 2 na funcao, visto que o valor inicial do dia eh 10
        //E supondo que nesse exemplo o npc as 12 va ao hospital, escreveriamos a funcao assim periodToLocation.Add(2, "Hospital");
        //Em resumo ao usar a funcao, sempre lembrar que o primeiro parametro eh o periodo do dia que a funcao eh realizada, sabendo que o valor escrito sera a somado as 10 inciais
        //E o segundo parametro sera o local onde o npc ficara 

        //Lembrete use os nomes iguais aos escritos em baixo
        periodToLocation.Add(0, "Hospital");
        periodToLocation.Add(1, "Padaria");
        periodToLocation.Add(2, "Biblioteca");
        periodToLocation.Add(3, "Square");
        periodToLocation.Add(4, "Bar");
        periodToLocation.Add(5, "Biblioteca");
    }

    public override void Interagir(Character otherCharacter)
    {
        //As variaveis escritas sem o "otherCharacter." sao as variaveis do proprio personagem, por exemplo:
        humor += 1;

        //As variaveis escritas com o "otherCharacter." sao as variaveis do personagem q ele interagi, por exemplo:
        otherCharacter.humor -= 1;

        //Vamos fazer um exemplo se o outro personagem que o seu npc intergir for barulhento ele ficara mais triste, porem se a pessoa for gentil ele ficara mais feliz

        //Se caso a persona da pessoa for do tipo Loud
        if (otherCharacter.persona == Persona.Loud)
        {
            //Meu humor tera seu valor alterado em menos 1
            humor -= 1;
        }
        //Se ele nao for barulhento e for do tipo de persona Kind
        else if (otherCharacter.persona == Persona.Kind)
        {
            //Meu humor tera seu valor alterado em mais 1
            humor += 1;
        }

    }
}
