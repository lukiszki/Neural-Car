using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UnityEngine;
using Random = System.Random;

public class Network
{


    float off = 0.01f;

    float distance1, distance2;

    [Range(-0.5f, 0.5f)]
    [SerializeField]
    float[] W1 = new float[25];
    [Range(-0.5f, 0.5f)]
    [SerializeField]
    float[] W2 = new float[15];

    [Range(-0.5f, 0.5f)]
    [SerializeField]
    float[] b1 = new float[5];
    [Range(-0.5f, 0.5f)]
    [SerializeField]
    float[] b2 = new float[3];

    float l, r,f;


    public Network()
    {
        //InitValues();
        //LoadValues();

    }

    public void LoadValues(ref Random rnd)
    {
        Values data =  ValueContaier.value;
        float range = 0.001f;
        


        for (int i = 0; i < W1.Length; i++)
        {
            W1[i] = data.W1[i] + Randomize(range, ref rnd);
        }
        for (int i = 0; i < W2.Length; i++)
        {
            W2[i] = data.W2[i] + Randomize(range, ref rnd);
        }
        for (int i = 0; i < b1.Length; i++)
        {
            b1[i] = data.b1[i] + Randomize(range, ref rnd);
        }
        for (int i = 0; i < b2.Length; i++)
        {
            b2[i] = data.b2[i] + Randomize(range, ref rnd);
       }
    }

    public void InitValues()
    {
        Random rnd = new Random();
        for (int i = 0; i < W1.Length; i++)
        {
            W1[i] = (float)rnd.NextDouble() - 0.5f;
        }
        for (int i = 0; i < W2.Length; i++)
        {
            W2[i] = (float)rnd.NextDouble() -0.5f;
        }
        for (int i = 0; i < b1.Length; i++)
        {
            b1[i] = (float)rnd.NextDouble() - 0.5f;
        }
        for (int i = 0; i < b2.Length; i++)
        {
            b2[i] = (float)rnd.NextDouble() - 0.5f;
        }
    }

    private void CalulateValues(float forDist, float forLeftDist,float forRightDist, float leftDist, float rightDist)
    {
        float a1 = ReLU(forDist * W1[0] + forLeftDist * W1[1] + forRightDist * W1[2] +    leftDist * W1[3] +  rightDist * W1[4]  + b1[0]);
        float a2 = ReLU(forDist * W1[5] + forLeftDist * W1[6] + forRightDist * W1[7] +    leftDist * W1[8] +  rightDist * W1[9]  + b1[1]);
        float a3 = ReLU(forDist * W1[10] + forLeftDist * W1[11] + forRightDist * W1[12] + leftDist * W1[13] + rightDist * W1[14] + b1[2]);
        float a4 = ReLU(forDist * W1[15] + forLeftDist * W1[16] + forRightDist * W1[17] + leftDist * W1[18] + rightDist * W1[19] + b1[3]);
        float a5 = ReLU(forDist * W1[20] + forLeftDist * W1[21] + forRightDist * W1[22] + leftDist * W1[23] + rightDist * W1[24] + b1[4]);




        l = Sigmoid(a1 * W2[0] + a2 * W2[1] + a3 * W2[2] + a4 * W2[3] + a5 * W2[4] + b2[0]);
        r = Sigmoid(a1 * W2[5] + a2 * W2[6] + a3 * W2[7] + a4 * W2[8] + a5 * W2[9] + b2[1]);
        f = Sigmoid(a1 * W2[10] + a2 * W2[11] + a3 * W2[12] + a4 * W2[13] + a5 * W2[14] + b2[2]);
    }
    public float GetValue(float forDist, float forLeftDist, float forRightDist,float leftDist,float rightDist)
    {
        CalulateValues(forDist, forLeftDist, forRightDist, leftDist, rightDist);
        // Debug.Log("L: ");Debug.Log(l);
        //Debug.Log("R: "); Debug.Log(r);
        float n = l;
        int index = 0;
        if (l < r)
        {
            n = r;
            index = 1;
        }
        if (f > n)
        {
            return 0;
        }
        return n == l ? -1 : 1;
       // return l < r  ? 1 : -1;
    }
    
    float Sigmoid(float x)
    {
        return 1 / (1 + Mathf.Exp(-x));
    }
    float ReLU(float x)
    {
        return x > 0 ? x : 0;
    }
    public void Save()
    {
        Values val = new Values(W1, W2, b1, b2);
        SaveSystem.SaveValues(val);
    }
    private float Randomize(float range, ref Random rnd)
    {
        float f = (1f / range)/2;
        return ((float)rnd.NextDouble()/f) - range;
    }
    public Values GetValues()
    {
        return new Values(W1, W2, b1, b2);
    }
}
[System.Serializable]
public class Values
{
    public float[] W1;
    public float[] W2;
    public float[] b1;
    public float[] b2;
    public Values(float[] _W1, float[] _W2, float[] _b1, float[] _b2)
    {
        W1 = _W1;
        W2 = _W2;
        b1 = _b1;
        b2 = _b2;
    }

}
