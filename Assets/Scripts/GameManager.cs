using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

/// <summary>
/// This class is responsible for game logic, cube spawning, player level, etc.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _currentCube;
    [SerializeField] private GameObject _lastCube;
    [SerializeField] private TMP_Text _scoreText;
    private int _level;
    private bool _isDone;
    private bool _isPaused;
    private int _colorStyle;
    // 150 - blue
    // 0 - red
    // 130 - green

    private void Start()
    {
        switch (PlayerPrefs.GetInt("Color"))
        {
            case 0:
                _colorStyle = 160;
                break;
            case 1:
                _colorStyle = 130;
                break;
            case 2:
                _colorStyle = 0;
                break;
        }

        _currentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(((_colorStyle + _level) / 100f) % 1f, 1f, 1f));
        SpawnBlock();
    }
    private void Update()
    {
        if (_isDone || _isPaused) return;

        if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
            SceneManager.LoadScene("Menu");

        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);

        var pos1 = _lastCube.transform.position + Vector3.up * 10f;
        var pos2 = pos1 + ((_level % 2 == 0) ? Vector3.left : Vector3.forward) * 120;

        if (_level % 2 == 0)
            _currentCube.transform.position = Vector3.Lerp(pos2, pos1, time);
        else
            _currentCube.transform.position = Vector3.Lerp(pos1, pos2, time);


        if (Input.GetMouseButtonDown(0))
            SpawnBlock();
    }
    private void SpawnBlock()
    {
        if (_lastCube != null)
        {
            _currentCube.transform.position = new Vector3(Mathf.Round(_currentCube.transform.position.x),
                                                         _currentCube.transform.position.y,
                                                         Mathf.Round(_currentCube.transform.position.z));

            _currentCube.transform.localScale = new Vector3(_lastCube.transform.localScale.x - Mathf.Abs(_currentCube.transform.position.x - _lastCube.transform.position.x),
                                                           _lastCube.transform.localScale.y,
                                                           _lastCube.transform.localScale.z - Mathf.Abs(_currentCube.transform.position.z - _lastCube.transform.position.z));

            _currentCube.transform.position = Vector3.Lerp(_currentCube.transform.position, _lastCube.transform.position, 0.5f) + Vector3.up * 5f;
            if (_currentCube.transform.localScale.x <= 0f ||
               _currentCube.transform.localScale.z <= 0f)
            {
                StartCoroutine(EndGame());
                return;
            }
        }
        _scoreText.text = $"SCORE: {_level}";
        _lastCube = _currentCube;
        _currentCube = Instantiate(_lastCube);
        _currentCube.name = _level + "";
        _currentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(((_colorStyle + _level) / 100f) % 1f, 1f, 1f));
        _level++;
        Camera.main.transform.position = _currentCube.transform.position + new Vector3(150, 150f, -150);
        Camera.main.transform.LookAt(_currentCube.transform.position + Vector3.down * 30f);
        Vibration.Vibrate();

    }

    public void PauseGame(bool isPaused)
    {
        _isPaused = isPaused;
        Time.timeScale = _isPaused ? 1.0f : 0.0f;
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator EndGame()
    {
        _isDone = true;
        Database.instance.AddNewScoreDatabase(_level - 1);

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Game");
    }
}
