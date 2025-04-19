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
        List<string> dictionary = new List<string>();
        string wynik = "";
        string roboczy = data;
        string znak = "";
        
        for (int x = 0; roboczy.Length > 0; x++)
        {
            znak = roboczy.Substring(x, x + 1);
            if (!dictionary.Contains(znak))
            {
                dictionary.Add(znak);
            }
        }
        int sequenceLength = 2;
        int nrIndex = 0;
        do
        {
            znak = roboczy.Substring(0, sequenceLength);
            for (; dictionary.Contains(znak); sequenceLength++)
            {
                znak = roboczy.Substring(0, sequenceLength);
            }
            nrIndex = dictionary.IndexOf(roboczy.Substring(0, sequenceLength - 1));
            wynik += nrIndex;
            dictionary.Add(znak);
            roboczy.Remove(0, sequenceLength - 1);
            sequenceLength = 2;
        }
        while (roboczy.Length >= sequenceLength)


        while (pozostaly.Length > 0);

        return new LZWData(String.Join(" ", resultCode), dictionary);
    }

    public string decompress(LZWData data)
    {
        if(data.compressedData == null | data.compressedData.Length==0)
        {
            throw new Exception("No data to decompress!");
        }
        if (data.compressionDict == null | data.compressionDict.Count < 2)
        {
            throw new Exception("Dictionary null or too short!");
        }
        Console.WriteLine("Compressed data passed validation");

        //Inicjalizacja slownika jednoznakowego
        if (data.compressionDict.Any(x => x.Length > 1))
        {
            List<string> dict = data.compressionDict.Where(x => x.Length == 1).ToList();
        } else
        {
            List<string> dict = data.compressionDict;
        }

        foreach (string key in data.compressionDict)
        {
            addCode(key);
        }

        List<string> workingDict = dict.ToList();
        string compressedData = data.compressedData;




        
        


         return "";
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