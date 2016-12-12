﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playerの移動に関するスクリプト
/// </summary>
public class PlayerMover : MonoBehaviour
{
    PlayerState _state = null;

    /// <summary>
    /// 移動中かどうか
    /// </summary>
    public bool isMove
    {
        get; private set;
    }

    void Start()
    {
        isMove = false;
        _state = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (isMove) return;
        if (_state.vertical != 0 || _state.horizontal != 0)
        {
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        yield return null;
        isMove = true;
        GetComponent<PlayerRotater>().PlayerRotate(); //後で変える
        //ターンをタス処理追加
        
        var time = 0.0f;
        var targetPos = transform.position;

        //カメラの向きを所得
        var cameraForward = Vector3.Scale(_state.cameraObj.transform.forward, new Vector3(1, 0, 1)).normalized;

        //Cameraの回転情報から向かう先を決める
        if (_state.cameraObj.cameraState == CameraRotater.RotateState.Vertical)
        {
            targetPos.z += _state.playerDirection == PlayerState.Direction.Up ? cameraForward.z * 1.0f : _state.playerDirection == PlayerState.Direction.Down ? cameraForward.z * -1.0f : 0.0f;
            targetPos.x += _state.playerDirection == PlayerState.Direction.Right ? _state.cameraObj.transform.right.x * 1.0f : _state.playerDirection == PlayerState.Direction.Left ? _state.cameraObj.transform.right.x * -1.0f : 0.0f;
        }
        else
        {
            targetPos.x += _state.playerDirection == PlayerState.Direction.Up ? cameraForward.x * 1.0f : _state.playerDirection == PlayerState.Direction.Down ? cameraForward.x * -1.0f : 0.0f;
            targetPos.z += _state.playerDirection == PlayerState.Direction.Right ? _state.cameraObj.transform.right.z * 1.0f : _state.playerDirection == PlayerState.Direction.Left ? _state.cameraObj.transform.right.z * -1.0f : 0.0f;
        }

        //移動中はループ回ってる
        while (time < 1.0f)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, time);
            yield return null;
        }

        isMove = false;
    }
}
