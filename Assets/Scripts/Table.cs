/**
* File DOC
* 
* @Description Este script é responsável por realizar o 'Assignment' (atribuição) das Boxes a uma nova intância da estruta BOX. Ele está atribuído ao GameObject 'Tabela' (Parent de todos os 'Quadrados').
*
* @ChangeLog 
*   - Vinícius Lessa - 08/30/2022: Criação do arquivo e documentação de cabeçalho. Implementação da lógica de atribuição de cada GameObject ao Array do tipo BOX.
* 
* @ Tips & Tricks: 
* 
**/

using UnityEngine;

public class Table : MonoBehaviour
{
    [HideInInspector]
    public Box[] state; // Nova Instância do tipo Array - É acessado pelo GameManager em RunTime
    private int boxCount; // Contagem das 'Childs'

    private void Awake() {
        boxCount    = gameObject.transform.childCount;
        state       = new Box[boxCount];

        AssignBoxes();
    }

    private void AssignBoxes()
    {
        for (int i = 0; i < boxCount; i++) 
        {
            GameObject childObj = gameObject.transform.GetChild(i).gameObject;

            Box box = new Box();
            
            box.boxName = childObj.name;
            box.boxObj = childObj;
            box.type = Box.Type.Empty;
            box.position = childObj.transform.position;
            
            state[i] = box;
        }
    }    
}