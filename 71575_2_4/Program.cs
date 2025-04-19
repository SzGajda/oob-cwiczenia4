// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading;
using System.Linq;


interface StringCompressor
{
    string compress(string data);
    string decompress(string compressedData);
}

class LZW : StringCompressor
{
    private string[] dict = new string[128];
    private int index = 0;
    
    public LZW(List<string> initDict)
    {
        if(initDict != null)
        {
            initDict.ForEach(x=> addCode(x));
        }
    }

    public LZW()
    {
    }

    private void addCode(string code)
    {
        if(code == null)
        {
            throw new ArgumentNullException("code is null");
        }
        if (dict.Contains(code))
        {
            throw new ArgumentException("code already exists");
        }
        try
        {
            dict[index] = code;
        }
        catch (IndexOutOfRangeException e)
        {
            if(dict.Length > 1025)
            {
                throw new Exception("something wrong");
            }
            Console.WriteLine("Extending dict array *2");
            Array.Resize(ref dict, dict.Length*2);
            dict[index] = code;
        }
        index += 1;
    }

    public string compress(string data)
    {
        string wynik = "";
        string roboczy = data;
        string znak = "";
        
        for (int x = 0; roboczy.Length > x; x++)
        {
            znak = roboczy.Substring(x, 1);
            if (!dict.Contains(znak))
            {
                addCode(znak);
            }
        }

        List<string> dictionary = dict.ToList();
        int sequenceLength = 2;
        int nrIndex = 0;

        do
        {
            znak = roboczy.Substring(0, sequenceLength);
            while (dictionary.Contains(znak))
            {
                sequenceLength++;
                if (sequenceLength > roboczy.Length)
                {
                    break;
                }
                znak = roboczy.Substring(0, sequenceLength);
            }

            nrIndex = dictionary.IndexOf(roboczy.Substring(0, sequenceLength - 1));
            wynik += nrIndex.ToString() + " ";
            if (sequenceLength > roboczy.Length)
            {
                break;
            }
            dictionary.Add(znak);
            roboczy.Remove(0, sequenceLength - 1);
            sequenceLength = 2;

        } while (roboczy.Length >= sequenceLength);

        if (roboczy.Length == 1)
        {
            znak = roboczy.Substring(0, 1);
            nrIndex = dictionary.IndexOf(roboczy.Substring(0, 1));
            wynik += nrIndex;
        }


        return String.Join(" ", wynik);
    }

    public string decompress(string data)
    {
        if(data == null | data.Length==0)
        {
            throw new Exception("No data to decompress!");
        }
        if(!data.All(n=>Char.IsDigit(n) & Char.IsWhiteSpace(n)))
        {
            throw new Exception("Must be digits and whitespaces!");
        }
        
        Console.WriteLine("Compressed data passed validation");

        //Inicjalizacja slownika jednoznakowego
        List<string> workingDict = dict.ToList();

        List<string> codes = data.Split(" ").ToList();

        string result = "";

        while (codes.Count>0)
        {
            int code1 = int.Parse(codes[0]);
            int code2 = int.Parse(codes[1]);
            string word1 = workingDict[code1];
            string word2 = "";
            result = string.Concat(result, word1);

            try //ababab case
            {
                word2 = workingDict[code2];
            } catch (ArgumentOutOfRangeException e) {
                word2 = word1;
            }

            if (!workingDict.Contains(string.Concat(word1, word2[0])))
            {
                workingDict.Add(string.Concat(word1, word2[0]));
            } else
            {
                Console.WriteLine("waht");
            }

            codes.RemoveAt(0);

        }
         return result;
    }
}



class cw4
{
    static void Main(string[] args)
    {
        LZW lzw = new LZW();
        var data = lzw.compress("franek poszedl kupic lody, wrocil z kielbasa");
        
        Console.WriteLine(data);

        var str = lzw.decompress(data);
        Console.WriteLine(str);



        Console.ReadKey();

    }
}