using System;
using System.IO;
using UnityEngine;

public class One : MonoBehaviour
{
    public string[] inputs;
    public long score;

    private void Start()
    {
        inputs = File.ReadAllLines("Assets/Resources/AdventOfCode/inputs.txt");

        foreach (string t in inputs)
        {
            int indexOf = t.IndexOf("-", StringComparison.Ordinal);

            long i0 = Convert.ToInt64(t[..indexOf]);

            long i1 = Convert.ToInt64(t[(indexOf + 1)..]);

            long idx = i0;

            while (idx <= i1)
            {
                string idxString = idx.ToString();

                string s0 = idxString[..(idxString.Length / 2)];
                string s1 = idxString[(idxString.Length / 2)..];

                if (s0 == s1)
                {
                    score += idx;
                }

                idx++;
            }
        }
    }
}