﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour
{

    static GameManager s_instance = null;

    public static GameManager Instance { get { if (s_instance == null) return null; return s_instance; } }


    public enum Puzzle
    {
        Poster = 10,
        WetTissue,
        Star,
        SignatureCard, // 13
        Wire,
        ShadowLight, // 15
        ArcadeConsole,
        LightSwitch,
        ChangeView,
        Curtain
    }

    UI ui;

    public GameObject[] Puzzles;

    public bool onPuzzle;

    bool[] ClearPuzzles;

    bool curtain;

    Action Room;
    public bool this[int idx] // 인덱서 사용
    {
        get { return ClearPuzzles[idx]; }
        set
        {
            ClearPuzzles[idx] = value;
            Room?.Invoke();
        }
    }

    [SerializeField]
    GameObject light2D;
    [SerializeField]
    GameObject globalLight;

    void Start()
    {
        Room -= CheckRoomClear;
        Room += CheckRoomClear;
        ClearPuzzles = new bool[10];
        curtain = false;
        if (s_instance == null)
        {
            s_instance = this;
        }
        ui = FindObjectOfType<UI>();
        onPuzzle = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 좌표 바꾸기
        RaycastHit2D hit = Physics2D.Raycast(mousPos, transform.forward, 10f); // 레이캐스트
        if (hit.collider == null)
            return;
        if (Input.GetMouseButtonDown(0))
        {

            if (hit.collider.gameObject.layer == (int)Puzzle.LightSwitch)
            {
                if (ClearPuzzles[(int)Puzzle.LightSwitch - 10])
                    globalLight.GetComponent<Light2D>().intensity = globalLight.GetComponent<Light2D>().intensity == 0.5f ? 1f : 0.5f;
                else // 테스트용
                    Debug.Log("전선 연결 필요");
                return;
            }

            if (hit.collider.gameObject.layer == (int)Puzzle.ChangeView)
            {
                Debug.Log("창밖 변환");
                return;
            }
            if (hit.collider.gameObject.layer == (int)Puzzle.Curtain)
            {
                Debug.Log("커튼 상태 " + (curtain = !curtain));
                return;
            }

            if (hit.collider.gameObject.layer == (int)Puzzle.ArcadeConsole)
            {
                if (ClearPuzzles[(int)Puzzle.LightSwitch - 10])
                    Puzzles[(int)Puzzle.ArcadeConsole - 10].SetActive(true);
                else
                {
                    Debug.Log("전선 연결 필요");

                    return;
                }
                Debug.Log("ArcadeConsole");
            }

            if (!onPuzzle)
            {
                ui.prevCameraPos = Camera.main.transform.position;
                Camera.main.transform.position = new Vector3(0, -25f, -100f);
                ActiveBackArrow();
                onPuzzle = true;
            }

            if (hit.collider.gameObject.layer == (int)Puzzle.Poster)
            {
                Debug.Log("Poster");

                Puzzles[(int)Puzzle.Poster - 10].SetActive(true);

            }
            else if (hit.collider.gameObject.layer == (int)Puzzle.WetTissue)
            {

                Puzzles[(int)Puzzle.WetTissue - 10].SetActive(true);
                Debug.Log("WetTissue");

            }
            else if (hit.collider.gameObject.layer == (int)Puzzle.SignatureCard)
            {

                Puzzles[(int)Puzzle.SignatureCard - 10].SetActive(true);
                Debug.Log("SignatureCard");
            }
            else if (hit.collider.gameObject.layer == (int)Puzzle.Star)
            {

                Puzzles[(int)Puzzle.Star - 10].SetActive(true);
                LightEnable();
                Debug.Log("Star");
            }
            else if (hit.collider.gameObject.layer == (int)Puzzle.Wire)
            {

                Puzzles[(int)Puzzle.Wire - 10].SetActive(true);
                Debug.Log("Wire");
            }
            else if (hit.collider.gameObject.layer == (int)Puzzle.ShadowLight)
            {

                Puzzles[(int)Puzzle.ShadowLight - 10].SetActive(true);
                Debug.Log("ShadowLight");
            }

        }

    }

    public void LightEnable()
    {
        if (light2D.gameObject.activeSelf)
        {
            light2D.gameObject.SetActive(false);
            globalLight.gameObject.SetActive(true);
        }
        else
        {
            light2D.transform.position = new Vector2(0, -25f);
            light2D.gameObject.SetActive(true);
            globalLight.gameObject.SetActive(false);
        }

    }

    void ActiveBackArrow()
    {
        ui.backArrow.SetActive(true);
        ui.DisbleArrow();
    }

    void CheckRoomClear()
    {

        foreach(var puzzle in ClearPuzzles)
        {
            if(!puzzle)
            {

                Debug.Log("풀 퍼즐이 남았음");
                return;
            }
        }

    }
}
