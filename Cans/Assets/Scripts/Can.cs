using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Can : MonoBehaviour
{
    public Sprite ClosedCan;

    private bool _broken;
    private bool _once;
    private bool _canPressed;
    private bool _canClosed;

    private void Start()
    {
        _broken = Main._brokenCan;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GreenLine"))
        {
            _canPressed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GreenLine"))
        {
            _canPressed = false;
            if (!_canClosed && !_broken) Main._gameover = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_once)
        {
            if (collision.gameObject.CompareTag("Press"))
            {
                if (_canPressed)
                {
                    if (!_broken)
                    {
                        gameObject.GetComponent<Image>().sprite = ClosedCan;
                        Main._score++;
                        _canClosed = true;
                    }
                    else
                        Main._gameover = true;
                }
                else
                    Main._gameover = true;
                _once = true;
            }
        }
    }
}
