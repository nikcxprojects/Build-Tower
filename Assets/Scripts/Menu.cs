using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// The class responsible for interacting with the UI in the menu
/// </summary>
public class Menu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _homeWindow;
    [SerializeField] private GameObject _scoreWindow;
    [SerializeField] private GameObject _colorWindow;

    [Header("Scores")]
    [SerializeField] private Transform _scoreWindowTransform;
    [SerializeField] private GameObject _scorePrefab;
    private List<GameObject> _scoreInitialied = new List<GameObject>();
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// The method includes the main menu
    /// </summary>
    public void Home()
    {
        if (_homeWindow.activeSelf)
            return;

        _colorWindow.SetActive(false);
        _scoreWindow.SetActive(false);
        _homeWindow.SetActive(true);

        if (_scoreInitialied.Count > 0)
            ClearScores();
    }


    /// <summary>
    /// Clears information about records
    /// </summary>
    public void ClearScores()
    {
        foreach (GameObject score in _scoreInitialied)
            Destroy(score);

        _scoreInitialied.Clear();
    }

    /// <summary>
    /// Includes a window with player records
    /// </summary>
    public void ViewScores()
    {
        _homeWindow.SetActive(false);
        _scoreWindow.SetActive(true);

        List<int> scores = Database.instance.GetScoresDatabase();

        if (scores.Count == 0)
            return;

        for (int i = scores.Count - 1; i > -1; i--)
        {
            GameObject scoreObj = Instantiate(_scorePrefab, _scoreWindowTransform);
            scoreObj.GetComponent<RectTransform>().localScale = Vector3.one;

            TMP_Text scoreText = scoreObj.GetComponentInChildren<TMP_Text>();
            scoreText.text = $"{scores[i]} blocks";

            _scoreInitialied.Add(scoreObj);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
            Home();
    }
}
