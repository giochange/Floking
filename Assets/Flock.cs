using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;//conexao com flockmanager.
    public float speed;//varia
    bool turing = false;//variavel de ativação do turning
    void Start()
    {
        //diretiva do controle de velocidade
        speed = Random.Range(myManager.minSpeed,
            myManager.maxSpeed);

    }

    void Update()
    {
        //limitador de espaço para nao deixar os peixes pederem referencia entre si
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        RaycastHit hit = new RaycastHit();//criação do raycast
        Vector3 direction = myManager.transform.position - transform.position;



        //limite que o peixe pode chegar - chegando no limite ele rotacioana circularmente e retorna ao ponto inicial
        {
            if (!b.Contains(transform.position))
            {
                turing = true;
                direction = myManager.transform.position - transform.position;
            }



            else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))//responsavel por evitar colisão
            {
                turing = false; // se ele nao etiver no limite significa que ainda esta no cardume
                direction = Vector3.Reflect(this.transform.forward, hit.normal);
            }
            else turing = false;

            if (turing)// rotate 
            {

                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationSpeed * Time.deltaTime);






               
            }
            else
            {
                if (Random.Range(0, 100) < 10)
                    speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
                if(Random.Range(0,100)<20)
                    ApplyRules();

            }
        }


        transform.Translate(0, 0, Time.deltaTime * speed);//set de velocidade

    }


   

    


    void ApplyRules()//regras para criaçao dos flocks
    {
        GameObject[] gos;//array dos peixes a serem criados
        gos = myManager.allFish;
       // bool turing = false;

        Vector3 vcentre = Vector3.zero;//calculo de centralização
        Vector3 vavoid = Vector3.zero;//refernca ponto central
        float gSpeed = 0.01f;// velocidade inicial para equlibrar 
        float nDistance;//calculo de distancia entre um objeto e outro
        int groupSize = 0;//quantidade de grupos de cardumes -




        foreach (GameObject go in gos)/*criando objetos peixes,calculando distancia entre eles,
            assim em seguida faz o calculo central e finaliza calculando a quantidade 


            */
        {
            if (go != this.gameObject)
            {
               
                    
               nDistance = Vector3.Distance(go.transform.position,
                   this.transform.position);
               if (nDistance <= myManager.neighbourDistance)
                {

                 vcentre += go.transform.position;
                            groupSize++;

                            if (nDistance < 1.0f)
                            {
                                vavoid = vavoid + (this.transform.position - go.transform.position);
                            }
                            //pegado do metododo da classe flock-
                            Flock anotherFlock = go.GetComponent<Flock>();
                            gSpeed = gSpeed + anotherFlock.speed;// reprograma quando o objeto esta muito rapido assim cria uma desaaceleração.
               }
                    
            }
        }
        if (groupSize > 0)//configuração dentro do grupo 
        { vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;


            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime); }
    }
}


