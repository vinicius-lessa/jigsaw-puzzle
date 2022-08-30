/**
* File DOC
* 
* @Description Script simples para recarregar a Scene/Level sempre que um botão com a finalidade de Reiniciar o jogo seja pressionado.
*
* @ChangeLog 
*   - Vinícius Lessa - 08/29/2022: Criação do arquivo e documentação de cabeçalho. Implementação da Estrutura inicial e carregamento da Cena.
* 
**/

using UnityEngine;
using UnityEngine.SceneManagement;

public class RefreshButton : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}