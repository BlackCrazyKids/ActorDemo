using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 新手引导动画
/// 1.直接通过像素透明实现事件穿透
/// 2.通过计算碰撞区域实现事件穿透----OK
/// </summary>
public class Guide : MonoBehaviour, IPointerUpHandler //IPointerClickHandler, IPointerDownHandler,
{
    public Image target;
    //public bool reversed;

    private Vector4 center;
    private Material material;
    private float diameter; // 直径
    private float current = 0f;
    float velocity = 0f;
    private Canvas canvas;

    Vector3[] corners = new Vector3[4];

    void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        target.rectTransform.GetWorldCorners(corners);
        diameter = Vector2.Distance(WordToCanvasPos(canvas, corners[0]), WordToCanvasPos(canvas, corners[2])) / 2f;

        float x = corners[0].x + ((corners[3].x - corners[0].x) / 2f);
        float y = corners[0].y + ((corners[1].y - corners[0].y) / 2f);

        Vector3 center = new Vector3(x, y, 0f);
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, center, canvas.GetComponent<Camera>(), out position);

        center = new Vector4(position.x, position.y, 0f, 0f);
        material = GetComponent<Image>().material;
        material.SetVector("_Center", center);

        (canvas.transform as RectTransform).GetWorldCorners(corners);
        for (int i = 0; i < corners.Length; i++)
        {
            current = Mathf.Max(Vector3.Distance(WordToCanvasPos(canvas, corners[i]), center), current);
        }

        material.SetFloat("_Silder", current);
    }

    void Update()
    {


        float value = Mathf.SmoothDamp(current, diameter, ref velocity, 0.3f);
        if (!Mathf.Approximately(value, current))
        {
            current = value;
            material.SetFloat("_Silder", current);
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            Awake();
        }
    }

    Vector2 WordToCanvasPos(Canvas canvas, Vector3 world)
    {
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out position);
        return position;
    }

    public bool IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
    {
        if (!enabled) return true;

        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, eventCamera, out position);
        float length = Vector2.Distance(position, center);
        Debug.LogErrorFormat("{0}\t{1}\t{2}", (diameter / 2) > length, length, diameter);
        return (diameter / 2) > length;
    }

    //监听抬起
    public void OnPointerUp(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerUpHandler);
    }
    ////监听按下
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    PassEvent(eventData, ExecuteEvents.pointerDownHandler);
    //}
    ////监听点击
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    PassEvent(eventData, ExecuteEvents.submitHandler);
    //    PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    //}

    //把事件透下去
    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, data.position, data.enterEventCamera, out position);
        float length = Vector2.Distance(position, center);
        if (diameter < length) return;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++)
        {
            if (current != results[i].gameObject)
            {
                ExecuteEvents.Execute(results[i].gameObject, data, function);
                return;
            }
        }
    }
}