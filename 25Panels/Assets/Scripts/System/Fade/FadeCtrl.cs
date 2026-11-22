using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCtrl : SingletonMonoBehaviour<FadeCtrl>
{
    // フェードの種類
    public enum FadeType
    {
        NONE = -1,

        SIMPLE,
    }

    private FadeType    _fade_type  = FadeType.SIMPLE;
    private FadeType    _prev_fade  = FadeType.NONE;
    private bool        _is_working = false;

    [SerializeField]
    private FadeBase    _simple;

    public bool IsActive()
    {
        bool flag = false;

        switch (_fade_type)
        {
            case FadeType.SIMPLE:
                flag = _simple.IsActive();
                break;
        }

        return flag;
    }

    // フェード稼働中か
    public bool IsFade()
    {
        return _is_working;
    }

    public void FadeIn(FadeType in_type = FadeType.SIMPLE)
    {
        if (in_type == FadeType.NONE ||     // NONEは不正な値なので弾く
            _is_working == true ||          // 処理中に多重命令されてたら弾く
            CheckAnyFadeActive() == false)  // フェードONになっていないなら弾く
        {
            return;
        }
        _is_working = true;
        _fade_type = in_type;

        switch (in_type)
        {
            case FadeType.SIMPLE:
                _simple.FadeIn(() => { Finish(); });
                break;
        }
    }

    public void FadeOut(FadeType in_type = FadeType.SIMPLE)
    {
        FadeOut(in_type, Color.black);
    }

    public void FadeOut(FadeType in_type, Color in_color)
    {
        if (in_type == FadeType.NONE ||     // NONEは不正な値なので弾く
            _is_working == true ||          // 処理中に多重命令されてたら弾く
            CheckAnyFadeActive() == true)   // フェードOFFでないなら弾く
        {
            return;
        }

        _is_working = true;
        _fade_type = in_type;

        switch (_fade_type)
        {
            case FadeType.SIMPLE:
                _simple.SetColor(in_color);
                _simple.FadeOut(()=>{ Finish(); });
                break;
        }
    }

    // コールバック用
    private void Finish()
    {
        _is_working = false;
        _prev_fade = _fade_type;
    }

    // 存在する種類のフェードのいずれかが有効になっているか
    private bool CheckAnyFadeActive()
    {
        return _simple.IsActive();
    }
}
