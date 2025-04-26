using UnityEngine;

public class sippo : MonoBehaviour
{
    private TrailRenderer trail;

    void Start()
    {
        // Trail Renderer ���A�^�b�`����Ă��邩�m�F�A�Ȃ���Βǉ�
        trail = GetComponent<TrailRenderer>();
        if (trail == null)
        {
            trail = gameObject.AddComponent<TrailRenderer>();
        }

        // �� �g���C���̌����ڐݒ� ��

        // trail �̒��������ɕۂ��߁A�\�����Ԃ�Z�߂ɌŒ�
        trail.time = 1.4f; // ���x�ɉ����Ē����i��F0.3�b�Ԃ����\���j

        // �������
        AnimationCurve curve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        trail.widthCurve = curve;

        // �S�̂̑����{��
        trail.widthMultiplier = 0.3f;

        // �F�i�������A��ɕs�����j
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 1.0f)
            }
        );
        trail.colorGradient = gradient;

        // �ގ��i�K�{�j
        trail.material = new Material(Shader.Find("Sprites/Default"));

        // �f�t�H���g�͔�\���ɂ��Ă���
        trail.emitting = false;
    }

    void Update()
    {
        // �����Ă���Ƃ����� trail ��\��
        float move = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.F) ? 1f : 0f;
        trail.emitting = move != 0;
    }
}
