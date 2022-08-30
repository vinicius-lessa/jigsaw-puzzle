/**
* File DOC
* 
* @Description Este script é responsável por Randomizar a posição das pelas do Puzzle. Ele está atribuído ao GameObject 'Peças' (Parent de todas as peças).
*
* @ChangeLog 
*   - Vinícius Lessa - 08/29/2022: Criação do arquivo e documentação de cabeçalho. Aplicada a Lógica de randomização das posições de cada peça.
* 
**/

using UnityEngine;

public class PiecesSpawn : MonoBehaviour
{
    private float limitPosX = 6f;
    private float limitPosY = .8f;        

    private void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++) { // Passa por todas peças
            
            Vector3 newPosition = new Vector3( // Cria nova posição Randômica
                Random.Range(-limitPosX, limitPosX),
                Random.Range(transform.position.y - limitPosY, transform.position.y + limitPosY),
                0f);

            gameObject.transform.GetChild(i).gameObject.transform.position = newPosition; // Atualiza Posição da Peça
            
            gameObject.transform.GetChild(i).gameObject.GetComponent<PieceSnapping>().originalPosition = newPosition;  // Atualiza Posição Original
            
        }
    }
}
