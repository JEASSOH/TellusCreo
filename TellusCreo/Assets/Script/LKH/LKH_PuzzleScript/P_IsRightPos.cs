﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_IsRightPos : MonoBehaviour
{
    [SerializeField] private GameObject correctObj;

    private bool isTrigger;
    private bool isRight;

    private P_PuzzleClear clearController;

    private void Awake()
    {
        clearController = transform.GetComponentInParent<P_PuzzleClear>();
    }

    private void OnEnable()
    {
        isTrigger = false;
        isRight = false;
    }

    void Start()
    {
        this.gameObject.layer = 30;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTrigger = true;

        if (System.Object.ReferenceEquals(collision.gameObject, correctObj))
            isRight = true;
        else 
            isRight = false;

        clearController.CheckClear_IsRightPos();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
         isTrigger = false;

        if (System.Object.ReferenceEquals(collision.gameObject, correctObj))
            isRight = false;
    }

    public void IsRight_true()
    {
        isRight = true;
        clearController.CheckClear_IsRightPos();
    }

    public void IsRight_false() { isRight = false; }

    public bool Get_isRight() { return isRight; }
}
