using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class EndingAnimation : MonoBehaviour
{
    [Serializable]
    public struct EndingSoundData
    {
        public Sprite sprite;
        public string name;
        public float volume;
    };


    [SerializeField]
    private List<EndingSoundData> _pages = new List<EndingSoundData>();

    [SerializeField]
    private Image _showPage;

    [SerializeField]
    private float _nextTime = 0.5f;
    private float _currentPasttime;
    private int _currentPage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentPasttime = _nextTime;
        _currentPage = 0;
        _showPage.sprite = _pages[_currentPage].sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentPage < _pages.Count - 1)
        _currentPasttime-=Time.deltaTime;
        if(_currentPasttime <= 0.0f)
        {
            _currentPasttime = _nextTime;
            _showPage.sprite = _pages[++_currentPage].sprite;
            if(_pages[_currentPage].name != null)
            {
                SoundManager.instance?.PlaySE(_pages[_currentPage].name, _pages[_currentPage].volume);
            }
        }

    }

    public void Finish()
    {
        SceneManager.LoadScene("Game");
    }
}
