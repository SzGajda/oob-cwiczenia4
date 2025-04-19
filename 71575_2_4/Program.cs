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
        compressionDict = dict;
    }
    public string compressedData;
    public List<string> compressionDict;
}

class LZW : StringCompressor<LZWData>
{
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
        var source = data.compressedData;
        var dictionary = data.compressionDict;
        var resultCode = new List<string>();

        string znak = "";
        string pierwszeSlowoKodowe = "";
        string drugieSlowoKodowe = "";
        string roboczy = "";
        string pozostaly = "";
        string tymczasowy = "";
        string codeTemp = "";
        List<string> doRaportu = new List<string>();

        pozostaly = source;
        roboczy = pozostaly;

        do
        {
            if (pierwszeSlowoKodowe == "")
            {
                do
                {
                    znak = roboczy.Substring(0, 1);
                    if (!Char.IsDigit(znak[0]))
                    {
                        pozostaly = roboczy.Remove(0, 1);
                        roboczy = pozostaly;
                    }
                }
                while (!Char.IsDigit(znak[0]));

                do
                {
                    znak = roboczy.Substring(0, 1);
                    if (Char.IsDigit(znak[0]))
                    {
                        pozostaly = roboczy.Remove(0, 1);
                        tymczasowy = tymczasowy + znak;
                        roboczy = pozostaly;
                    }
                }
                while (Char.IsDigit(znak[0]));

                pierwszeSlowoKodowe = dictionary[Convert.ToInt32(tymczasowy) - 1];
                tymczasowy = "";

            }

            do
            {
                znak = roboczy.Substring(0, 1);
                if (!Char.IsDigit(znak[0]))
                {
                    pozostaly = roboczy.Remove(0, 1);
                    roboczy = pozostaly;
                }

            }
            while (!Char.IsDigit(znak[0]));


            do
            {
                znak = roboczy.Substring(0, 1);
                if (Char.IsDigit(znak[0]))
                {
                    pozostaly = roboczy.Remove(0, 1);
                    tymczasowy = tymczasowy + znak;
                    roboczy = pozostaly;

                }
            }
            while (Char.IsDigit(znak[0]) && roboczy.Length > 0);

            try
            {
                drugieSlowoKodowe = dictionary[Convert.ToInt32(tymczasowy) - 1];
                tymczasowy = "";
            }
            catch (ArgumentOutOfRangeException ex)
            {
                doRaportu.Add("> Argument Out Of Range Exception - " + tymczasowy + ", ");
                drugieSlowoKodowe = pierwszeSlowoKodowe;
                codeTemp = roboczy;
                tymczasowy = "";

                do
                {
                    znak = roboczy.Substring(0, 1);
                    if (!Char.IsDigit(znak[0]))
                    {
                        pozostaly = roboczy.Remove(0, 1);
                        roboczy = pozostaly;
                    }
                }
                while (!Char.IsDigit(znak[0]));

                do
                {
                    znak = roboczy.Substring(0, 1);
                    if (Char.IsDigit(znak[0]))
                    {
                        pozostaly = roboczy.Remove(0, 1);
                        tymczasowy = tymczasowy + znak;
                        roboczy = pozostaly;
                    }

                }
                while (Char.IsDigit(znak[0]));

                znak = dictionary[Convert.ToInt32(tymczasowy)];
                tymczasowy = znak.Substring(0, 1);
                drugieSlowoKodowe = drugieSlowoKodowe + tymczasowy;
                roboczy = codeTemp;
            }
            if (!dictionary.Contains(pierwszeSlowoKodowe + drugieSlowoKodowe.Substring(0, 1)))
            {
                dictionary.Add(pierwszeSlowoKodowe + drugieSlowoKodowe.Substring(0, 1));
            }
            resultCode.Add(pierwszeSlowoKodowe);

            if (tymczasowy.Length > 0)
            {
                tymczasowy = pierwszeSlowoKodowe + drugieSlowoKodowe.Substring(0, 1);
                pierwszeSlowoKodowe = tymczasowy;
                roboczy = codeTemp;
                tymczasowy = "";
            }
            else
            {
                pierwszeSlowoKodowe = drugieSlowoKodowe;
                roboczy = pozostaly;
            }
            drugieSlowoKodowe = "";
        }
        while (pozostaly.Length > 0);

        return string.Concat(resultCode);
    }
}



class cw4
{
    static void Main(string[] args)
    {
        StringCompressor<LZWData> lzw = new LZW();
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