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

        var source = data;
        var dictionary = new List<string>();
        var resultCode = new List<string>();
        
        var mode = 1;

        string znak = "";
        string roboczy = "";
        string pozostaly = "";

        if (mode == 1)
        {
            pozostaly = source;
            do
            {
                roboczy = pozostaly;
                znak = roboczy.Substring(0, 1);
                if (!dictionary.Contains(znak))
                {
                    dictionary.Add(znak);
                }
                pozostaly = roboczy.Remove(0, 1);
            }
            while (pozostaly.Length > 0);
        }
        else if (mode == 2)
        {
            if (mode == 2)
            {
                Console.WriteLine("Wydaje się, że albo słownik jest za krótki, " +
                    "albo w ogóle go nie zdefiniowano!");
                return new LZWData("", new List<string>());
            }
            pozostaly = source;
        }
        pozostaly = source;
        int sequenceLength = 1;
        int indexNb = 0;
        do
        {
            roboczy = pozostaly;
            if (sequenceLength <= roboczy.Length)
            {
                znak = roboczy.Substring(0, sequenceLength);
                if (dictionary.Contains(znak))
                {
                    sequenceLength++;
                }
                else
                {
                    indexNb = dictionary.IndexOf(roboczy.Substring(0, sequenceLength - 1)) + 1;
                    resultCode.Add(indexNb.ToString());
                    dictionary.Add(znak);
                    pozostaly = roboczy.Remove(0, sequenceLength - 1);
                    sequenceLength = 1;
                }
            }
            else
            {
                znak = roboczy.Substring(0, 1);
                if (dictionary.Contains(znak))
                {
                    indexNb = dictionary.IndexOf(znak) + 1;
                    resultCode.Add(indexNb.ToString());
                    pozostaly = roboczy.Remove(0, 1);
                }
                else
                {
                    dictionary.Add(znak);
                    indexNb = dictionary.IndexOf(roboczy.Substring(0, sequenceLength - 1)) + 1;
                    resultCode.Add(indexNb.ToString());
                    pozostaly = roboczy.Remove(0, 1);
                }
            }
        }
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