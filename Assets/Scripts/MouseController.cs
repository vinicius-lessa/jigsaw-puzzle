/**
* File DOC
* 
* @Description Este é responsável por Mover as peças na Tela. Ele está atribuído ao GameObject 'MainCamera'. Por aqui também são realizadas algumas chamadas para Métodos externos (GameManager). Definições dos métodos e processamentos:
*   - Neste Script são salvas a Peça Selecionada atual, e a última pela Selecionada, afim de gerenciar a propriedad "SortingOrder";
*   - CheckHitObject()  Method - Verifica e salva o objeto selecionado através da Classe RaycastHit2D juntamente com informações do Input Mouse;
*   - DragObject()      Method - Atualiza em tempo real a posição da Peça selecionada de acorco com a posição do Mouse;
*   - DropObject()      Method - Limpa a pela seleciona, consequentemente não a movendo mais. Chama o método GameManager.CheckPieceSnapping() para verificar se a peça está sobre algum 'Quadrado';
*
* @ChangeLog 
*   - Vinícius Lessa - 08/28/2022: Criação do arquivo e documentação de cabeçalho.
*   - Vinícius Lessa - 08/30/2022: Implementação do gerenciamento de 'SortinOrder'. Implementação da chamada dos métodos presentes no componente 'GameManager'.
* 
**/

using UnityEngine;

public class MouseController : MonoBehaviour
{
    GameObject objSelected = null; // Objeto clicado e/ou arrastado
    GameObject lastSelectedObj = null; // Irá Salvar o último objeto "arrastado". Utilizado para gerenciar "SortingOrder"
    
    Vector3 mousePositionOffset; // Deslocamento do Mouse

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Checa se o Objeto foi clicado
            CheckHitObject();
        }
        if (Input.GetMouseButton(0) && objSelected != null)
        {
            // Segura Peça
            DragObject();
        }
        if (Input.GetMouseButtonUp(0) && objSelected != null)
        {
            // Solta Peça
            DropObject();
        }
    }

    private void CheckHitObject() {
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (hit2D.collider != null && (hit2D.transform.gameObject.CompareTag("LeftPiece") || hit2D.transform.gameObject.CompareTag("RightPiece")))
        {
            mousePositionOffset = hit2D.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objSelected = hit2D.transform.gameObject;
            
            // Sorting Order
            if (lastSelectedObj != objSelected){
                objSelected.GetComponent<SpriteRenderer>().sortingOrder++;
            }

            if (!(lastSelectedObj == null) && lastSelectedObj != objSelected) 
            {
                lastSelectedObj.GetComponent<SpriteRenderer>().sortingOrder--;
            }

            // Limpa registro do Objeto da Box atual (caso ele exista)
            GameManager.Instance.Unassign(objSelected);
        }
    }

    private void DragObject()
    {        
        objSelected.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 10.0f)) + mousePositionOffset;
    }

    private void DropObject()
    {
        GameManager.Instance.CheckPieceSnapping(objSelected); // Verifica se Fará Snapping
        lastSelectedObj = objSelected;
        objSelected = null;
    }
}