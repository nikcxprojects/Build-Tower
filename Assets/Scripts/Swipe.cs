using UnityEngine;
using UnityEngine.UI;

public class Swipe : MonoBehaviour
{
    public GameObject scrollbar;
    private float _posScroll = 0.0f;
    private float[] _pos;
    private int _choosed;

    private void Update()
    {
        _pos = new float[transform.childCount];
        float distance = 1.0f / (_pos.Length - 1.0f);

        for (int i = 0; i < _pos.Length; i++)
            _pos[i] = distance * i;

        if (Input.GetMouseButton(0))
            _posScroll = scrollbar.GetComponent<Scrollbar>().value;
        else
        {
            for(int i = 0; i < _pos.Length; i++)
            {
                if (_posScroll < _pos[i] + (distance / 2.0f) && _posScroll > _pos[i] - (distance / 2.0f))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, _pos[i], 0.1f);
                    _choosed = i;
                }
            }
        }
    }

    public void SaveColor()
    {
        PlayerPrefs.SetInt("Color", _choosed);
    }
}
