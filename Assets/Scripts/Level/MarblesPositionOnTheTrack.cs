using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MarblesPositionOnTheTrack : MonoBehaviour
{
    [Header("Track fill UI")]
    [SerializeField] private Image _trackDistanceImage;

    [SerializeField] private RectTransform _playerOnTheTrackUI;

    [SerializeField] private RectTransform _finishTrackOnUI;

    [SerializeField] private Transform _crown;

    private Vector3 _playerStartPositionUI;

    private Vector3 _finishPositionUI;

    private LevelPreparer _levelPreparer;

    private EventMachine _playerEventMachine;

    private Enemys _enemys;

    private Player _player;

    private RoadMover _winnerRoadMover;

    private RoadMover _playerRoadMover;

    private RoadMover[] _roadMover;

    private float _progress;

    private bool _isFillDistance;

    private bool _isFinishMove;

    private void Awake()
    {
        _levelPreparer = GetComponent<LevelPreparer>();

        _player = _levelPreparer.PlayerMarble;

        _playerRoadMover = _player.GetComponent<RoadMover>();

        _playerEventMachine = _player.GetComponent<EventMachine>();

        _enemys = _levelPreparer.Enemys;

        _playerStartPositionUI = _playerOnTheTrackUI.localPosition;

        _finishPositionUI = _finishTrackOnUI.localPosition;

        RoadMover[] enemyRoadMover = _enemys.GetEnemyRoadMover();

        _roadMover = new RoadMover[enemyRoadMover.Length + 1];

        _roadMover[0] = _player.GetComponent<RoadMover>();

        for (int i = 1; i < _roadMover.Length; i++)
        {
            _roadMover[i] = enemyRoadMover[i - 1];
        }
    }

    private void Start()
    {
        _playerEventMachine?.SubscribeOnRoadStartStart(EnableFillDistance);

        _playerEventMachine?.SubscribeOnRoadEnd(DisableFillDistance);

        _playerEventMachine?.SubscribeOnMoveToNextLevel(DelayWhentNextLevel);

        StartDistanceSet();
    }

    private void FixedUpdate()
    {
        MarbleSortByDistance();

        if (_isFillDistance)
        {
            _progress = _playerRoadMover.Distance / _levelPreparer.LevelEventZone.RoadDistance;

            _trackDistanceImage.fillAmount = _progress;

            _playerOnTheTrackUI.localPosition = Vector3.Lerp(_playerStartPositionUI, _finishPositionUI, _progress);
        }
    }

    public void EnableFillDistance()
    {
        _trackDistanceImage.fillAmount = 0;

        _isFinishMove = true;

        _isFillDistance = true;
    }

    private void DisableFillDistance()
    {
        _isFillDistance = false;
    }

    private void MarbleSortByDistance()
    {
        var distances = _roadMover.OrderByDescending(u => u.Distance);

        int count = 1;

        if (_isFinishMove)
        {
            foreach (RoadMover marble in distances)
            {
                if (marble.IsMove == false)
                {
                    count++;
                }
            }
        }

        foreach (RoadMover marble in distances)
        {
            if (marble.IsMove)
            {
                marble.PositionOnTheTrackView.SetPosition(count);

                if (count == 1)
                {
                    if (_winnerRoadMover != marble)
                    {
                        _crown.SetParent(marble.transform.GetChild(0));

                        _crown.localPosition = Vector3.up;
                    }

                    _winnerRoadMover = marble;
                }

                count++;
            }
        }
    }

    private void StartDistanceSet()
    {
        var distances = _roadMover.OrderByDescending(u => u.Distance);

        int count = 1;

        foreach (RoadMover marble in distances)
        {
            marble.PositionOnTheTrackView.SetPosition(count);

            if (count == 1)
            {
                _crown.SetParent(marble.transform.GetChild(0));

                _crown.localPosition = Vector3.up;

                _winnerRoadMover = marble;
            }

            count++;
        }
    }

    private void DelayWhentNextLevel()
    {
        //_isFinishMove = false;

        //MarbleSortByDistance();

        //StartDistanceSet();

        Invoke("WhentNextLevel", Time.deltaTime);
    }

    private void WhentNextLevel()
    {
        _isFinishMove = false;

        MarbleSortByDistance();

        StartDistanceSet();
    }
}
