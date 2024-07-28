using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TUMO Coimbra - Game Dev - NÃ­vel 2 e 3
 * Dicas para os TUMOnautas:
 * - Teclas:
 *  - Caracteres que numa tecla aparecem em cima (como o !, &, =, |, ;): Shift + tecla
 *  - { => Option + Shift + 8
 *  - } => Option + Shift + 9
 *
 * - Tipos de variÃ¡vel base:
 *  - int (inteiro), string (frase), bool (true/false), float (nÃºmero decimal),
 *  - null (nada - usado Ã s vezes em comparaÃ§Ãµes)
 *  - void (devolve nada - usado apenas em funÃ§Ãµes)
 *
 * - Acesso a variÃ¡vel e funÃ§Ãµes (nÃ£o escrever nada fica "private"):
 *  - public (acessÃ­vel por outros scripts e inspector);
 *  - private (acessÃ­vel por este script apenas)
 *
 * - Ordens:
 *  - Dentro das funÃ§Ãµes escrevem-se ordens, uma por cada linha.
 *  - Cada ordem dentro de uma funÃ§Ã£o, tem de acabar com ; (Shift + ,)
 *
 * - FunÃ§Ãµes e classes
 *  - NÃ£o acabam com ; - comeÃ§am e acabam {  }
 *
 * - Comentar cÃ³digo:
 *  - // (comenta a linha)
 *  - e como fiz este comentÃ¡rio com barras e asteriscos, na ordem exata
 */

public class novaclass : MonoBehaviour
{
    // Start Ã© chamado antes do primeiro frame
    void Start()
    {
        
    }

    #region Nivel02-Aula05-Update-FixedUpdate
    // Update Ã© chamado uma vez por frame
    /*
    void Update()
    {
        
    }
    */
    
    // FixedUpdate Ã© sempre chamado num tempo especÃ­fico (normalmente a cada 0,02 segundos)
    // Usado para coisas com fÃ­sica (tipo aceleraÃ§Ã£o, velocidade e forÃ§as)
    /*
    void FixedUpdate()
    {
        // escrever cÃ³digo aqui
    }
    */
    #endregion

}