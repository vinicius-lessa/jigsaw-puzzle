/**
* File DOC
* 
* @Description Este é responsável por gerenciar de maneira geral o "Core" do jogo estilo JigSaw Puzzle. Ele está atribuído ao GameObject 'MainCamera'. Um resumo dos métodos e do que é tratado neste script:
*   - Um Instância própria é criada para ser chamada em outros Scripts e componentes (Awake() Method);
*   - CheckPieceSnapping()  Method - É chamado pelo Script "MouseController" quando uma peça é "Solta" na Scene. Dentro dele é feito uma verificação dentre todos os "Boxes" da cena para verificar se o 'Snapping' será feito ou não;
*   - Unassign()            Method - É chamado pelo Script "MouseController" quando uma peça é "Selecionada". Ele verifica se é necessário DESATRIBUIR a peça selecionada de algumdos "Boxes" da cena (caso a pela já teho sido 'encaixada' em algum deles);
*   - IsGameFinished()      Method - É chamado pelo Script "PieceSnapping" logo após uma peça "encaixar" em um Box. Ele verificar o estado de todos os boxes, e caso eles estejam todos Completos ('Filled'), chama o método PuzzleSolved();
*   - PuzzleSolved()        Method - É chamado pelo neste mesmo Script, pelo Método 'IsGameFinished()'. É responsável por renderizar a tela Final do jogo (Quando o Puzzle é resolvido);
*
* @ChangeLog 
*   - Vinícius Lessa - 08/28/2022: Criação do arquivo e documentação de cabeçalho.
*   - Vinícius Lessa - 08/30/2022: Implementação de novos métodos para realização do Snapping e Unassign das peças em relação aos Quadrados.
* 
**/

using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Self Instance
    public static GameManager Instance { get; private set; }
    
    // Components
    public Table table; // Script

    // Game Objects    
    public GameObject btn_RestartRunTime; // Fim do Jogo
    public GameObject panel_RestartEnd; // Fim do Jogo

    [SerializeField]
    private float piceOffset = 0.6f; // Usado no Snapping
    private float minimumDistance = 1.5f;  // Usado no Snapping

    private void Awake() {
        Instance = this;
    }

    public void CheckPieceSnapping(GameObject pieceObj)
    {
        Box[] tempState = table.state; // Stancia modelo de Objeto do tipo Array

        for (int i = 0; i < tempState.Length; i++)
        {
            Vector3 boxPosition = tempState[i].position;
            float distanceBetween =  Vector2.Distance(boxPosition, pieceObj.transform.position);

            // Perto o Suficiente do 'Box'
            if (distanceBetween < minimumDistance) {

                var isLeftPiece = (pieceObj.CompareTag("LeftPiece") ? true : false);

                int figureQtd = int.Parse(pieceObj.name.Substring(0, 1)); // Amarzena quantas figuras tem na peça                

                switch (tempState[i].type)
                {
                    case Box.Type.Empty:
                        if (isLeftPiece) {                            
                            pieceObj.GetComponent<PieceSnapping>().SnapPiece(new Vector3(boxPosition.x - piceOffset, boxPosition.y, boxPosition.z)); // Snap to Left

                            // Muda Atributos do Objeto "Box" correspondente
                            tempState[i].type       = Box.Type.LeftFilled;  // Muda Tipo
                            tempState[i].leftPiece  = pieceObj;             // Atribui Peça Esquerda

                        } else {
                            pieceObj.GetComponent<PieceSnapping>().SnapPiece(new Vector3(boxPosition.x + piceOffset, boxPosition.y, boxPosition.z)); // Snap to Right                            
                            
                            // Muda Atributos do Objeto "Box" correspondente
                            tempState[i].type       = Box.Type.RightFilled; // Muda Tipo
                            tempState[i].rightPiece = pieceObj;             // Atribui Peça Direita
                        }
                        
                        tempState[i].allowedPiece = figureQtd; // Tipo de Peça que pode se juntar a Colocada (1, 2, 3 ...)
                        
                        break;

                    case Box.Type.LeftFilled:
                        if (isLeftPiece) {
                            pieceObj.GetComponent<PieceSnapping>().MoveToOriginalPosition();
                            return; // Peça esqueda já Preenchida

                        } else {
                            if (figureQtd != tempState[i].allowedPiece) { // Somente se for o mesmo número de peças
                                pieceObj.GetComponent<PieceSnapping>().MoveToOriginalPosition();
                                return; // Peça Incorreta
                            }
                            
                            pieceObj.GetComponent<PieceSnapping>().SnapPiece(new Vector3(boxPosition.x + piceOffset, boxPosition.y, boxPosition.z)); // Snap to Right
                            tempState[i].type       = Box.Type.Filled;  // Muda Tipo
                            tempState[i].rightPiece = pieceObj;         // Atribui Peça Direita
                        }
                        break;

                    case Box.Type.RightFilled:
                        if (isLeftPiece) {
                            if (figureQtd != tempState[i].allowedPiece) { // Somente se for o mesmo número de peças
                                pieceObj.GetComponent<PieceSnapping>().MoveToOriginalPosition();
                                return; // Peça Incorreta
                            }
                            
                            pieceObj.GetComponent<PieceSnapping>().SnapPiece(new Vector3(boxPosition.x - piceOffset, boxPosition.y, boxPosition.z)); // Snap to Left
                            tempState[i].type       = Box.Type.Filled;  // Muda Tipo
                            tempState[i].leftPiece  = pieceObj;         // Atribui Peça Esquerda

                        } else {
                            pieceObj.GetComponent<PieceSnapping>().MoveToOriginalPosition();
                            return; // Peça esqueda já Preenchida
                        }

                        break;
                        
                    case Box.Type.Filled: // Box Já Preenchida com Ambas as peças
                        pieceObj.GetComponent<PieceSnapping>().MoveToOriginalPosition();
                        return;
                }
                break; // Exit For
            }
        }

        table.state = tempState; // Reatribui Objetos
    }

    public void Unassign(GameObject currentSelectPiece)
    {
        Box[] tempState = table.state; // Stancia modelo de Objeto do tipo Array

        var isLeftPiece = (currentSelectPiece.CompareTag("LeftPiece") ? true : false);

        for (int i = 0; i < tempState.Length; i++)
        {
            if (tempState[i].type == Box.Type.Empty && (tempState[i].leftPiece == null && tempState[i].rightPiece == null)) {
                continue; // Skip current Looping
            }

            if ( isLeftPiece && tempState[i].leftPiece != null ) {
                
                if ( tempState[i].leftPiece.name == currentSelectPiece.name ) {

                    tempState[i].leftPiece = null; // Remove Peça ESQUERDA

                    // Atualiza Estado do 'Box'
                    switch (tempState[i].type)
                    {
                        case (Box.Type.LeftFilled):
                            tempState[i].type = Box.Type.Empty;
                            break;
                        
                        case (Box.Type.Filled):
                            tempState[i].type = Box.Type.RightFilled;
                            break;
                    }
                    break; // Exit For Loop
                }

            } else if ( !isLeftPiece && tempState[i].rightPiece != null ) { // Remove Peça DIREITA

                if ( tempState[i].rightPiece.name == currentSelectPiece.name ) {

                    tempState[i].rightPiece = null;

                    // Atualiza Estado do 'Box'
                    switch (tempState[i].type)
                    {
                        case (Box.Type.RightFilled):
                            tempState[i].type = Box.Type.Empty;
                            break;
                        
                        case (Box.Type.Filled):
                            tempState[i].type = Box.Type.LeftFilled;
                            break;
                    }
                    break; // Exit For Loop
                }
            }
        }

        table.state = tempState; // Reatribui Objetos
    }

    public void IsGameFinished()
    {
        Box[] boxes = table.state;
        bool allFilled = true;

        foreach ( Box box in boxes )
        {
            // Debug.Log(" Nome: " + box.boxName + " - Estado: " + box.type);
            if (box.type != Box.Type.Filled) { 
                allFilled = false ;
            }
        }

        if (allFilled) { PuzzleSolved(); }
    }

    private void PuzzleSolved()
    {
        btn_RestartRunTime.gameObject.SetActive(false); // DISABLE - Botão apresentado em Runt Time        
        panel_RestartEnd.gameObject.SetActive(true);    // ENABLE - Painel de "Sucesso"
        
        Debug.Log("Jogo Completado!");
    }
}