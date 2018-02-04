using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null; // Allows us to access this from other scripts
    private bool isMenuOpen;
	private bool menuToggleProtect;
	public GameObject inGameMenu; 

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		// Uncomment to keep game manager persistent
		//DontDestroyOnLoad(gameObject);
		isMenuOpen = false;
		menuToggleProtect = false;
	}
	
    void Start()
    {
		DynamicGI.UpdateEnvironment();
	}
	
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape)) {
			if (!menuToggleProtect) {
				isMenuOpen = !isMenuOpen;
			}
			menuToggleProtect = true;
		} else if (menuToggleProtect) {
			menuToggleProtect = false;
		}
		
		inGameMenu.gameObject.SetActive(isMenuOpen);
	}
	
	public void closeMainMenu()
	{
		isMenuOpen = false;
	}
	
	public void reloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);isMenuOpen = false;
	}
}