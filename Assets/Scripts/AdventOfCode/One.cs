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

        foreach (string input in inputs)
        {
            //ulong code = Convert.ToUInt64(input);

            int battery0 = -1;
            int battery1 = -1;

            for (int i = 9; i > -1; i--)
            {
                string num = i.ToString();

                if (input.Contains(num))
                {
                    int idx = input.IndexOf(num, StringComparison.Ordinal);

                    if (battery0 == -1 && idx != input.Length - 1)
                    {
                        battery0 = idx;

                        string newInput = input[(battery0 + 1)..];

                        if (newInput.Contains(num))
                        {
                            int idx2 = newInput.IndexOf(num, StringComparison.Ordinal);

                            battery1 = idx2 + idx + 1;
                        }
                    }
                    else if (battery1 == -1)
                        battery1 = idx;
                }

                if (battery0 != -1 && battery1 != -1)
                    break;
            }

            if (battery1 <= battery0)
            {
                string newInput = input[(battery0 + 1)..];
                battery1 = -1;

                for (int i = 9; i > -1; i--)
                {
                    string num = i.ToString();

                    if (newInput.Contains(num))
                    {
                        int idx = newInput.IndexOf(num, StringComparison.Ordinal);

                        if (battery1 == -1)
                            battery1 = idx + battery0 + 1;
                    }

                    if (battery0 != -1 && battery1 != -1)
                        break;
                }
            }
            //Debug.Log(battery0 + ", " + battery1);

            //Debug.Log(input[battery0] + "" + input[battery1]);

            string batteries = input[battery0] + "" + input[battery1];

            score += Convert.ToInt32(batteries);
        }
    }
}