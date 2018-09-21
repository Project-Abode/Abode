using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextField : MonoBehaviour
{
    [SerializeField]
    private TextMesh _textField;

    private Vector3 _newPosition;
    private Quaternion _newRotation;

    public void SetValue(int value)
    {
        if (value >= 0) {
            _textField.text = string.Format("+{0}", value);
            _textField.color = Color.green;
        } else {
            _textField.text = string.Format("AUAUAUA! {0}", value);
            _textField.color = Color.red;
        }

        _newRotation = Camera.main.transform.rotation;
        _newRotation.z = transform.rotation.z;

        transform.rotation = _newRotation;
    }

    private void Update()
    {
        _newPosition.y = Time.deltaTime;
        transform.position += _newPosition;
    }
}
