using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        //리스트의 크기만큼 반복
        while (n > 1)
        {
            n--;
            //0보다 크거나 같고 int32.MaxValue 보다 작은 32비트 부호 있는 정수 입니다.
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}