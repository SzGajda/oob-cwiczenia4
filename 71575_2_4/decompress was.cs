using System.Collections.Generic;
using System;
public class Whatever
{
    string decompress(LZWData data)
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
