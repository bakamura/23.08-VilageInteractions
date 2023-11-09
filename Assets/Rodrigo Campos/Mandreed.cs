using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mandreed : CharBase
{
    private Dictionary<int, string> periodToLocation = new Dictionary<int, string>();
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationDegree;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        //Nao precisa mexer
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed / 10 * Time.deltaTime);
        transform.eulerAngles = Vector3.forward * Mathf.Sin(Time.realtimeSinceStartup * rotationSpeed) * rotationDegree;
    }
    private void AdicionarARotina(int periodoDoDia, string lugar)
    {
        periodToLocation.Add(periodoDoDia, lugar);
    }

    //Para implementacao simples mexer apenas a baixo

    /*
    Nome dos lugares no mapa:
    TownSquare
    Bakery
    Bar
    Library
    Hospital
    ?
    */

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Char")
        {
            //Para colocar uma interacao especifica entre alguns personagens
            //Use o modelo a baixo
            // if (collision.TryGetComponent<"Nome da classe do outro npc especifico">(out "Nome da classe do outro npc especifico" "Como vc pretende chamar a classe desse npc"))
            if (collision.TryGetComponent<CharTemplate>(out CharTemplate charTemplate))
            {
                //Aqui pode acessar as variaveis unicas daquele npc
            }
            // Colocar apos o {} "else"
            else if (collision.TryGetComponent<CharBase>(out CharBase charBase))
            {
                charBase.Interact(this);
            }
            else
            {
                Debug.Log("Erro em pegar informacoes de" + collision.gameObject.name);
            }
        }
        else
        {
            //Reacao do npc baseado na posicao de mundo
            switch (collision.gameObject.name)
            {
                case "TownSquare":

                    break;
                case "Bakery":

                    break;
                case "Bar":
                    //Caso o npc estiver no bar, ele ficara pobre e tera seu humor restaurado ao valor maximo
                    money = MoneyT.Poor;
                    humor = 3;
                    break;
                case "Library":
                    //Caso o npc estiver na biblioteca o npc ficara timido
                    persona = PersonalityT.Shy;
                    break;
                case "Hospital":
                    break;
                case "?":

                    break;
            }
        }
    }
    void Start()
    {
        //A rotina no caso atual q trabalharemos sera segmentada em 7 momentos diferentes sendo eles do tempo 0 ao 6
        //Sabendo q o dia eh dividido nos horarios de 0 a 6
        //Vamos fazer um exemplo que o jogador deve estar na TownSquare as 0
        //E as 1 deve estar indo ao Hospital

        //Exemplo de adicionar um local a sua rotina

        //Voce devera usar a funcao AdicionarARotina, nela dentro dos () primeiro colocaremos o horario e apos a virgula o local que vamos ir entre ""

        AdicionarARotina(0, "TownSquare");

        AdicionarARotina(2, "Library");

        AdicionarARotina(4, "Bar");

        AdicionarARotina(6, "TownSquare");

        //Nao mexer na linha a baixo 
        targetPosition = transform.position;
        GameManager.onChangePeriod.AddListener(OnChangePeriod);
    }
    public override void Interact(CharBase charInfo)
    {
        //Para interagirmos com outro personagem usaremos essa funcao
        //Nela poderemos mudar tanto as variaveis do seu personagem quanto a do seu colega
        //Essa funcao sera executada apos seu personagem chegar proximo a outro

        //Para usarmos ela temos q entender uma divisao, de como iremos diferenciar o seu personagem do personagem do seu colega
        //As informacoes do seu personagem sempre serao escritas desta forma : a informacao q vc deseja acessar em minusculo
        //humor;
        //As informacoes do outro personagem sempre serao escritas desta forma: charInfo. e a informacao q vc deseja com a primeira letra em maisculo
        //charInfo.Humor;

        //Vamos fazer um exemplo que o nosso personagem se o outro for da personalidade "Loud" ele ira perder humor
        if (charInfo.Persona == PersonalityT.Loud)
        {
            humor -= 2;
        }

        if (charInfo.Persona == PersonalityT.Grumpy)
        {
            humor -= 1.5f;
        }

        if (charInfo.Persona == PersonalityT.Sadistic)
        {
            humor -= 3;
        }

        if (charInfo.Persona == PersonalityT.Shy)
        {
            humor += 1.5f;
        }

        if (charInfo.Persona == PersonalityT.Kind)
        {
            humor += 3;
        }

        if (charInfo.Race == RaceT.Animal)
        {
            humor += 1.5f;
        }

        if (charInfo.Race == RaceT.Spirit)
        {
            humor += 1.5f;
        }

    }

    public void OnChangePeriod(int periodo)
    {
        if (periodToLocation.ContainsKey(periodo))
        {
            Vector3 locationObject = GameManager._placePosition[periodToLocation[periodo]];

            if (locationObject != null)
            {
                targetPosition = locationObject;
            }
        }
    }
}
