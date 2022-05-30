using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clocks : MonoBehaviour
{
    [Header("Clock Images")] 
    [SerializeField] private Image _clockImage;
    [SerializeField] private Image _coverImage;
    [Header("Clock Sprites")]
    [SerializeField] private Sprite _daySprite;
    [SerializeField] private Sprite _eveningSprite;
    [SerializeField] private Sprite _nightSprite;

    public void Update()
    {
        _coverImage.fillAmount = GameTimeController.CurrentTime;
        _clockImage.sprite = GameTimeController.CurrentTime switch
        {
            <= 0.33f => _daySprite,
            <= 0.66f => _eveningSprite,
            <= 1f => _nightSprite,
            _ => _clockImage.sprite
        };
    }
}
