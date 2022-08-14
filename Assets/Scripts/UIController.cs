using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IObservable
{
    [SerializeField] private GameObject _menu, _interface;

    public Button ExitMenuButton;

    private List<IObserver> MyObservers = new List<IObserver>();



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
        NotifyObservers();

        ExitMenuButton.interactable = true;
        ExitMenu();
    }


    public void QuitGame()
    {
        Application.Quit();
    }



    public void AddObserver(IObserver observer)
    {
        MyObservers.Add(observer);
    }



    public void RemoveObserver(IObserver observer)
    {
        MyObservers.Remove(observer);
    }



    public void NotifyObservers()
    {
        foreach(var observer in MyObservers)
        {
            observer.UpdateData();
        }
    }
}
