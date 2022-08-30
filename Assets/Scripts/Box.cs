/**
* File DOC
* 
* @Description Este script é responsável por definir a Estrutura do Objeto "Box". Ele será INSTANCIADO e utilizado para armazenar informações sobre o atual estado de cada Box presente na Tabela.
*
* @ChangeLog 
*   - Vinícius Lessa - 08/29/2022: Criação do arquivo e documentação de cabeçalho. Criada a estrutura inicial e seus Fields.
* 
**/

using UnityEngine;

public struct Box
{
    public enum Type
    {
        Empty,
        LeftFilled,
        RightFilled,
        Filled,
    }

    public string boxName; // Nome do 'Quadrado'
    public GameObject boxObj; // Objeto Armazenado de cada 'Quadrado'
    public GameObject leftPiece; // Objeto Armazenado da Peça ESQUERDA
    public GameObject rightPiece; // Objeto Armazenado da Peça DIREITA
    public Type type;  // Tipo (Empty, Filled...)
    public Vector3 position;  // Posição no Mundo
    public int allowedPiece; // Quantidade de Figuras
}
