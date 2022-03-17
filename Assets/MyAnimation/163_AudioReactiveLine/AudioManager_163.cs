using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Mathematics;

public class AudioManager_163 : MonoBehaviour
{
    public LightProbeProxyVolume test;

    AudioSource _audioSource;


    //sample num
    private const int SampleNum = 8192;
    public static float[] _sample = new float[SampleNum];
    public static float[] _resample = new float[SampleNum * 2];
    public static float[] _freqBand = new float[8];
    public float BandCoefficient = 1.0f;
    public VisualEffect VFX;
    public bool Use_DivideFliquency = false;

    public bool Band_0_Use;
    public string Band_0_PropertyName;
    public Vector2 Band0_Value = new Vector2(0, 0);
    public Vector4 Band0_Remap = new Vector4(0, 0, 0, 0);

    public bool Band_1_Use;
    public string Band_1_PropertyName;
    public Vector2 Band1_Value = new Vector2(0, 0);
    public Vector4 Band1_Remap = new Vector4(0, 0, 0, 0);

    public bool Band_2_Use;
    public string Band_2_PropertyName;
    public Vector2 Band2_Value = new Vector2(0, 0);
    public Vector4 Band2_Remap = new Vector4(0, 0, 0, 0);

    public bool Band_3_Use;
    public string Band_3_PropertyName;
    public Vector2 Band3_Value = new Vector2(0, 0);
    public Vector4 Band3_Remap = new Vector4(0, 0, 0, 0);

    public bool Band_4_Use;
    public string Band_4_PropertyName;
    public Vector2 Band4_Value = new Vector2(0, 0);
    public Vector4 Band4_Remap = new Vector4(0, 0, 0, 0);

    public bool Band_5_Use;
    public string Band_5_PropertyName;
    public Vector2 Band5_Value = new Vector2(0, 0);
    public Vector4 Band5_Remap = new Vector4(0, 0, 0, 0);

    public bool Band_6_Use;
    public string Band_6_PropertyName;
    public Vector2 Band6_Value = new Vector2(0, 0);
    public Vector4 Band6_Remap = new Vector4(0, 0, 0, 0);

    public bool Band_7_Use;
    public string Band_7_PropertyName;
    public Vector2 Band7_Value = new Vector2(0, 0);
    public Vector4 Band7_Remap = new Vector4(0, 0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        GetSpectrumAudioSource();

        if (Use_DivideFliquency)
        {
            MakeFrequecyBands();
        }

    }

    void resample(float[] sample)
    {
        for(int i = 0; i < sample.Length - 1; i += 3)
        {
            _resample[i] = sample[i];
            _resample[i + 1] = (sample[i] + sample[i + 1]) / 2;
            _resample[i + 2] = sample[i + 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();

        GraphicsBuffer gbuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, SampleNum, sizeof(float));
        //resample(_sample);
        gbuffer.SetData(_sample);

        VFX.SetGraphicsBuffer("_sample", gbuffer);

        if (Use_DivideFliquency)
        {
            MakeFrequecyBands();

            if (Band_0_Use)
            {
                Band0_Value[0] = _freqBand[0];
                Band0_Value[1] = math.remap(Band0_Remap[0], Band0_Remap[1], Band0_Remap[2], Band0_Remap[3], Band0_Value[0]);
            }
            if (Band_1_Use)
            {
                Band1_Value[0] = _freqBand[1];
                Band1_Value[1] = math.remap(Band1_Remap[0], Band1_Remap[1], Band1_Remap[2], Band1_Remap[3], Band1_Value[0]);
            }
            if (Band_2_Use)
            {
                Band2_Value[0] = _freqBand[2];
                Band2_Value[1] = math.remap(Band2_Remap[0], Band2_Remap[1], Band2_Remap[2], Band2_Remap[3], Band2_Value[0]);
            }
            if (Band_3_Use)
            {
                Band3_Value[0] = _freqBand[3];
                Band3_Value[1] = math.remap(Band3_Remap[0], Band3_Remap[1], Band3_Remap[2], Band3_Remap[3], Band3_Value[0]);
            }
            if (Band_4_Use)
            {
                Band4_Value[0] = _freqBand[4];
                Band4_Value[1] = math.remap(Band4_Remap[0], Band4_Remap[1], Band4_Remap[2], Band4_Remap[3], Band4_Value[0]);
            }
            if (Band_5_Use)
            {
                Band5_Value[0] = _freqBand[5];
                Band5_Value[1] = math.remap(Band5_Remap[0], Band5_Remap[1], Band5_Remap[2], Band5_Remap[3], Band5_Value[0]);
            }
            if (Band_6_Use)
            {
                Band6_Value[0] = _freqBand[6];
                Band6_Value[1] = math.remap(Band6_Remap[0], Band6_Remap[1], Band6_Remap[2], Band6_Remap[3], Band6_Value[0]);
            }
            if (Band_7_Use)
            {
                Band7_Value[0] = _freqBand[7];
                Band7_Value[1] = math.remap(Band7_Remap[0], Band7_Remap[1], Band7_Remap[2], Band7_Remap[3], Band7_Value[0]);
            }

            VFX.SetFloat(Band_0_PropertyName, Band0_Value[1]);
            VFX.SetFloat(Band_1_PropertyName, Band1_Value[1]);
            VFX.SetFloat(Band_2_PropertyName, Band2_Value[1]);
            VFX.SetFloat(Band_3_PropertyName, Band3_Value[1]);
            VFX.SetFloat(Band_4_PropertyName, Band4_Value[1]);
            VFX.SetFloat(Band_5_PropertyName, Band5_Value[1]);
            VFX.SetFloat(Band_6_PropertyName, Band6_Value[1]);
            VFX.SetFloat(Band_7_PropertyName, Band7_Value[1]);
        }
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_sample, 0, FFTWindow.Blackman);
    }

    void MakeFrequecyBands()
    {
        /*
         * 22050 / 512  = 43 Hz per Sample
         * 20 - 60
         * 250 - 500
         * 500 - 2000
         * 2000 - 4000
         * 6000 - 20000
         * 
         *********************************
         * 
         * 0 : 2    = 86hz
         * 1 : 4    = 172hz    [87-258]
         * 2 : 8    = 344hz    [259-602]
         * 3 : 16   = 688hz    [603-1290]
         * 4 : 32   = 1376hz   [1291-2666]
         * 5 : 64   = 2752z    [2667-5418]
         * 6 : 128  = 5504hz   [5419-10922]
         * 7 : 256  = 11008hz  [10923-21930]
         * 
         */

        int count = 0;

        for(int i = 0; i < 8; i ++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7) 
            {
                sampleCount += 2;
            }
            for(int j = 0; j < sampleCount; j++)
            {
                average += _sample[count] * (count + 1);
                count++;
            }

            //ƒTƒ“ƒvƒ‹‚Ì’l‚ð‰ÁŽZ‚µ‚Ä•½‹Ï‚ðŽæ‚é
            average /= count;

            //¬‚³‚·‚¬‚é‚½‚ß10”{
            _freqBand[i] = average * 10;

        }


    }

}
