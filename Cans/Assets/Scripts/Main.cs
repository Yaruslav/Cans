using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Pause;
    public GameObject Bridge;
    public GameObject Game;
    public GameObject Lose;
    public GameObject[] Sounds;

    public GameObject Press;
    public GameObject GreenLine;
    public GameObject CansFolder;
    public GameObject[] Cans;

    public GameObject[] Backgrounds;
    public Sprite[] Background_sprites;

    public Text[] Score;

    public Color[] Colors;
    public Text[] Texts;


    public static int _score;
    public static int _bestScore;
    public static float _speedTime;
    public static bool _brokenCan;
    public static bool _gameover;
    public static float _kf;


    public int _indexCan;

    private float _speedPress;
    private float _speedCan;

    private bool _tapped;
    private bool _muted;
    private bool _play;



    //==================================
    //          MenuButtons
    //==================================
    public void Play_btn()
    {
        StartCoroutine(Play());
    }
    public void Pause_btn()
    {
        Time.timeScale = 0;
        Pause.SetActive(true);
        _play = false;
    }
    public void Mute_Unmute()
    {
        if (!_muted)
        {
            Sounds[1].SetActive(false);
            Sounds[0].SetActive(true);
            _muted = true;
        }
        else
        {
            Sounds[0].SetActive(false);
            Sounds[1].SetActive(true);
            _muted = false;
        }
    }
    public void Again()
    {
        ChangeIndexLevel();
        SceneManager.LoadScene(0);
    }
    public void Continue()
    {
        _gameover = false;
        Time.timeScale = 1;
        Pause.SetActive(false);
        Lose.SetActive(false);
        _play = true;
    }
    //==================================


    private void Start()
    {
        //=============================================
        //               RESET SAVES
        //=============================================
        //_bestScore = 0;
        //_indexCan = 0;
        //PlayerPrefs.SetInt("BestScore", _bestScore);
        //PlayerPrefs.SetInt("IndexLevel", _indexCan);
        //=============================================

        Time.timeScale = 1;

        Menu.SetActive(true);
        Pause.SetActive(false);
        Bridge.SetActive(false);
        Game.SetActive(false);


        _brokenCan = false;
        _kf = 1;
        _muted = false;
        _play = false;
        _score = 0;
        Score[2].text = _bestScore.ToString();
        _speedPress = 2f;
        _tapped = false;
        _speedCan = 1f;
        _speedTime = 3f;
        Load();

        for (int i = 0; i <= 4; i++)
            Backgrounds[i].GetComponent<Image>().sprite = Background_sprites[_indexCan];
        for (int i = 0; i <= 7; i++)
            Texts[i].color = Colors[_indexCan];

        _gameover = false;
    }


    private void GameOver()
    {
        Time.timeScale = 0;
        Lose.SetActive(true);
        if (_score > _bestScore)
            _bestScore = _score;
        Score[1].text = _score.ToString();
        Save();
    }
    
    private void Save()
    {
        PlayerPrefs.SetInt("BestScore", _bestScore);
        PlayerPrefs.SetInt("IndexLevel", _indexCan);
    }

    private void Load()
    {
        _bestScore = PlayerPrefs.GetInt("BestScore");
        _indexCan = PlayerPrefs.GetInt("IndexLevel");
    }

    private void ChangeIndexLevel()
    {
        if (_indexCan == 6)
            _indexCan = 0;
        else
            _indexCan += 2;
    }

    private IEnumerator Tap()
    {
        _tapped = true; ;
        var _pressRB = Press.GetComponent<Rigidbody2D>();
        _pressRB.velocity = Vector2.down * _speedPress * _kf;
        yield return new WaitForSeconds(0.5f / _kf);
        _pressRB.velocity = Vector2.up * _speedPress * _kf;
        yield return new WaitForSeconds(0.5f / _kf);
        _pressRB.velocity = Vector2.zero;
        _tapped = false;
        Score[0].text = _score.ToString();
        Score[0].color = Colors[_indexCan];
    }

    private IEnumerator Play()
    {
        Menu.SetActive(false);
        Bridge.SetActive(true);
        yield return new WaitForSeconds(2f);
        Bridge.SetActive(false);
        Game.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _play = true;

        StartCoroutine(CanSpawner());
    }

    private IEnumerator CanSpawner()
    {
        while (!_gameover)
        {
            var rnd = UnityEngine.Random.Range(-2, 2);
            rnd = (rnd >= 0) ? _indexCan : _indexCan + 1;

            if (rnd == _indexCan)
                _brokenCan = false;
            else
                _brokenCan = true;

            Instantiate(Cans[rnd], CansFolder.transform);
            var canRB = CansFolder.transform.GetChild(CansFolder.transform.childCount - 1).GetComponent<Rigidbody2D>();
            canRB.velocity = Vector2.right * _speedCan * _kf;

            Destroy(canRB.gameObject, (_speedTime / _kf) * 2.3f);

            yield return new WaitForSeconds(_speedTime / _kf);

            if (GreenLine.transform.localScale.x > 0.3f)
            {
                GreenLine.transform.localScale = new Vector3(GreenLine.transform.localScale.x * 0.95f, GreenLine.transform.localScale.y);
                _kf *= 1.075f;
            }
        }
    }

    private void Update()
    {
        if (_play)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_tapped)
                    StartCoroutine(Tap());
            }
        }
        if (_gameover)
        {
            GameOver();
        }
    }
}
