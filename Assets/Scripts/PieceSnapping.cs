/**
* File DOC
* 
* @Description Este Script está atribuído a CADA peça do Puzzle. Ele possui apenas métodos que são chamados por outros Componentes. Definição dos Métodos e Processamentos:
*   - MoveToOriginalPosition()  Method - Realiza a mudança na Posição da Peça (Gameobject.transform) para a posição Original. É chamado pelo Componente/Classe 'GameManager';
*   - SnapPiece()               Method - Realiza o "Snapping" da peça na Box correta. Em seguida, chama a Coroutine 'VerifyGameIsFinished()' para verificar se o Jogo foi concluído. É chamado pelo Componente/Classe 'GameManager';
*
* @ChangeLog 
*   - Vinícius Lessa - 08/30/2022: Criação do Arquivo e Documentação de cabeçalho. Implementação de Todos os Métodos de movimentação das peças.
* 
**/

using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PieceSnapping : MonoBehaviour
{
    [HideInInspector]
    public Vector3 originalPosition;

    private float animationTime = 1f;    

    public void MoveToOriginalPosition()
    {
        transform.DOMove(originalPosition, animationTime); // DOTween
    }

    public void SnapPiece(Vector3 newPosition)
    {
        transform.DOMove(newPosition, (animationTime / 2));  // DOTween

        StartCoroutine(VerifyGameIsFinished());
    }

    private IEnumerator VerifyGameIsFinished()
    {
        yield return new WaitForSeconds(animationTime / 2); // Espera a Animação do Snpping (DOTween) finalizar

        GameManager.Instance.IsGameFinished();
    }

}