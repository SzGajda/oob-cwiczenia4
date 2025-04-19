// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading;
using System.Linq;


interface StringCompressor<T>
{
    T compress(string data);
    string decompress(T compressedData);
}

public struct LZWData
{
    public LZWData(string data, List<string> dict)
    {
        compressedData = data;
        compressionDict = dict.Where(x => x.Length == 1).ToList(); //Inicjalizacja slownika jednoznakowego
    }
    public string compressedData;
    public List<string> compressionDict;
}

class LZW : StringCompressor<LZWData>
{
    private string[] dict = new string[128];
    private int index = 0;
    
    public LZW(Collection<string> initDict?)
    {
        if(initDict != null)
        {
            initDict.ForEach(x=> addCode(x));
        }
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

    public LZWData compress(string data)
    {
        string wynik = "";
        string roboczy = data;
        string znak = "";
        
        for (int x = 0; roboczy.Length > 0; x++)
        {
            znak = roboczy.Substring(x, x + 1);
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
                    break
                }
                znak = roboczy.Substring(0, sequenceLength);
            }

            nrIndex = dictionary.IndexOf(roboczy.Substring(0, sequenceLength - 1));
            wynik += nrIndex.ToString() + " ";
            if (sequenceLength > roboczy.Length)
            {
                break
            }
            dictionary.Add(znak);
            roboczy.Remove(0, sequenceLength - 1);
            sequenceLength = 2;

        }
        while (roboczy.Length >= sequenceLength)

        if (roboczy.Length == 1)
        {
            znak = roboczy.Substring(0, 1);
            nrIndex = dictionary.IndexOf(roboczy.Substring(0, 1));
            wynik += nrIndex;
        }


        while (pozostaly.Length > 0);

        return new LZWData(String.Join(" ", resultCode), dictionary);
    }

    public string decompress(string data)
    {
        if(data.compressedData == null | data.compressedData.Length==0)
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
                workingDict.Add()
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
        Console.WriteLine(data.GetType);
        Console.WriteLine(data.compressedData);
        data.compressionDict.ForEach(x => Console.Write(x));
        Console.WriteLine();

        var str = lzw.decompress(data);
        Console.WriteLine(str);



        Console.ReadKey();

    }
}