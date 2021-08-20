using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverBackgroundPanel;

    [SerializeField] private GameObject _mainMenuBackgroundPanel;

    [SerializeField] private LevelPreparer _levelPreparer;

    [SerializeField] private GameObject[] _panels;

    private IGameOverPanels[] _gameOverPanels;

    private int panelOpenedIndex;

    private LevelInfo _levelInfo;

    private void Awake()
    {
        _gameOverPanels = new IGameOverPanels[_panels.Length];

        for (int i = 0; i < _panels.Length; i++)
        {
            _gameOverPanels[i] = _panels[i].GetComponent<IGameOverPanels>();

            _gameOverPanels[i].Init(this);
        }
    }

    private void Start()
    {
        _gameOverBackgroundPanel.SetActive(false);

        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].SetActive(false);
        }
    }

    public void GameOverUI(LevelInfo levelInfo)
    {
        _levelInfo = levelInfo;

        _gameOverBackgroundPanel.gameObject.SetActive(true);
        
        OpenPanel();
    }

    private void OpenPanel()
    {
        _panels[panelOpenedIndex].SetActive(true);

        _gameOverPanels[panelOpenedIndex].OpenPanel(_levelInfo);
    }

    public void GoToNextPanel()
    {
        _panels[panelOpenedIndex].SetActive(false);

        panelOpenedIndex++;

        if (panelOpenedIndex < _panels.Length)
        {
            OpenPanel();
        }
        else
        {
            // 
            GameOverUIFinish();
        }
    }

    public void GameOverUIFinish()
    {
        panelOpenedIndex = 0;

        _gameOverBackgroundPanel.SetActive(false);

        _mainMenuBackgroundPanel.SetActive(true);

        _levelPreparer.SelectNewLevel();
    }
}
