using UnityEngine;
using Unity.Mathematics;

sealed class SpiralMove : MonoBehaviour
{
    [SerializeField] float _noiseFrequency = 0.5f;
    [SerializeField] float _noiseToRotation = 1;
    [SerializeField] float _speed = 0.2f;


    [SerializeField] float _noise_x = 323.341f;
    [SerializeField] float _noise_y = 113.434f;
    [SerializeField] float _noise_z = 3.14334f;

    float3 _position;

    void Update()
    {
        var t = Time.time * _noiseFrequency;

        var n = math.float3(

            //Perlin さんが提供したn次元のノイズ生成アルゴリズム
            noise.snoise(math.float2(t, _noise_x)),
            noise.snoise(math.float2(t, _noise_y)),
            noise.snoise(math.float2(t, _noise_z))
        );

        //https://docs.unity.cn/Packages/com.unity.mathematics@1.2/api/Unity.Mathematics.float4x4.EulerZXY.html
        //最初にz軸、次にx軸、最後にy軸を中心に回転を実行して
        var rot = quaternion.EulerZXY(n * _noiseToRotation);

        //行列演算を使用して x と y を乗算します。
        _position += math.mul(rot, math.float3(0, 0, _speed * Time.deltaTime));

        transform.localPosition = _position;
        transform.localRotation = rot;
    }
}
