using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(()=>SceneManager.LoadScene(Constants.MENU_SCENE_INDEX));
    }
}
