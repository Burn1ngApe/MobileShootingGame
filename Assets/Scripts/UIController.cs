using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _menu, _interface;
    [SerializeField] private EnemyFactory _enemyGeneral;
    [SerializeField] private CharacterController _player;

    public Button _exitMenuButton;


    public void EnterMenu()
    {
        Time.timeScale = 0;
        _menu.SetActive(true);
        _interface.SetActive(false);
    }



    public void ExitMenu()
    {
        Time.timeScale = 1;
        _interface.SetActive(true);
        _menu.SetActive(false);
    }



    public void RestartGame()
    {
        _enemyGeneral.RespawnEnemies();
        _player.ResetEverything();

        _exitMenuButton.interactable = true;
        ExitMenu();
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
