﻿namespace Редактор_сайт
{
    public class Ofform
    {
        static string abz;
        static public int index = 0;
        static bool pred_adz = false;//фолс если текст,тру если диалог
        static bool pred_pust = true;//фолс если текст или диалог, тру если пустая строка
        static string[] text;
        static string text2;

        public static string Start(string[] texty)//чтение из файла
        {
            pred_adz = false;
            pred_pust = true;
            text = texty;
            text2 = "";

            Reading();//основная функция

            text2.Trim();

            return text2;
        }
        public static void Reading()//читаем по абзацу 
        {
            for (int i = 0; i < text.Length; i++)
            {
                abz = text[i].Replace("\r", "");
                if(abz.Trim()!="")
                {
abz = abz.TrimStart() + "\n";

                ZamenaChertochke();//замена маленького тире на большое
                Tab();//табуляция
                Abz();//Абзацы между текстом и диалогами
                Tochka();
                Zaglav();
                //Console.Write(abz);

                text2 += abz;
                index++;
                }
                
            }
        }
        public static void ZamenaChertochke()//замена маленького тире на большое
        {
            if ((abz[0] == '-' || abz[0] == '—') && abz.Length > 1 && char.IsLetter(abz[1]))
                abz = abz[0] + " " + abz[1..(abz.Length - 1)];

            abz = abz.Replace("- ", "— ");
            abz = abz.Replace("— ", "— ");
        }

        public static void Tab()//табуляция
        {
            if (abz != "<tab>\n" && abz.Length >= 5 && abz[0..5] != "<tab>" //не пустая строка с таб и длина больше 5 и таба нет
                || abz.Length < 5 && abz != "\n" && abz != "\r\n")//или длина меньше 5 и не пустая строка
                abz = "<tab>" + abz.TrimStart();//добавляем таб
        }
        public static void Abz()//абзац
        {
            if (abz.Length >= 8 && abz[0..8] == "<tab>***")//на случай если встретился разделитель *** или более 3
            {
                abz = abz.Substring(5);//обрезаем таб
                abz = "<center>" + abz.Substring(0, abz.Length - 1) + "</center>";//размещаем по центру 

                if (!pred_pust)//если перед этим не стояла пустая строка, то создаем ее
                    abz = "\n" + abz;

                abz = abz + "\n"; //добавляем в конце пустую строку 

                pred_pust = true;

            }
            else
            {
                if ((abz == "\n" || abz == "<tab>\n") && pred_pust == true)//если подряд две пустые строки, то пропускаем 
                    abz = "";
                else
                {
                    if (pred_pust == false && abz != "\n" && abz != "<tab>\n" && abz != "<tab>"//не пустая строка или до этого была не пустая и
                        && (abz.TrimStart()[5] == '—' && pred_adz == false //(диалог и предыдущий абзац - текст или 
                        || abz.TrimStart()[5] != '—' && pred_adz == true))//текст и предыдущий абзац - диалог)
                        abz = "\n" + abz.TrimStart();//разделяем их абзацем

                    if (abz == "\n" || abz == "<tab>\n" || abz == "<tab>")//если пустая строка
                        pred_pust = true;//тру если пусто
                    else
                    {
                        pred_pust = false;//фолс если текст или диалог

                        if (abz[0..2] != "\n" && abz.TrimStart()[5] == '—'
                            || abz[0..2] == "\n" && abz.TrimStart()[7] == '—')
                            pred_adz = true;//тру если диалог
                        else
                            pred_adz = false;//фолс если текст
                    }
                }
            }
        }

        public static void Tochka()//проставление точек.
        {
            if (abz != "")
            {
                abz = abz.Substring(0, abz.Length - 1).TrimEnd();
                if (abz != "" && abz[abz.Length - 1] != '.' && abz[abz.Length - 1] != '!'
                    && abz[abz.Length - 1] != '?' && abz[abz.Length - 1] != ':' && abz[abz.Length - 1] != '>'
                    && abz[abz.Length - 1] != '"' && abz[abz.Length - 1] != '»' && abz[abz.Length - 1] != '…')
                {
                    if (!(abz.Length > 3 && abz[abz.Length - 1] == '.' && abz[abz.Length - 2] == '.' && //просто инвертирую. условие в том, что это не ?.. !.. или ...
                        (abz[abz.Length - 3] == '.' || abz[abz.Length - 3] == '?' || abz[abz.Length - 3] == '!')))
                        abz = abz.TrimEnd() + ".";
                }
                abz = abz + "\n";
            }
        }
        public static void Zaglav()
        {
            char buk;
            for (int i = 1; i < abz.Length - 1; i++)
            {
                if (abz[i] == '>')
                {
                    if (abz[i + 1] == '—' && char.IsLetter(abz[i + 3]) && !char.IsUpper(abz[i + 3]))
                    {
                        buk = char.ToUpper(abz[i + 3]);
                        abz = abz[0..(i + 3)] + buk + abz[(i + 4)..abz.Length];
                    }
                    if (abz[i + 1] != '—' && char.IsLetter(abz[i + 1]) && !char.IsUpper(abz[i + 1]))
                    {
                        buk = char.ToUpper(abz[i + 1]);
                        abz = abz[0..(i + 1)] + buk + abz[(i + 2)..abz.Length];
                    }
                }
                if ((abz[i - 1] == '.' || abz[i - 1] == '?' || abz[i - 1] == '!') && abz[i] == ' '
                    && char.IsLetter(abz[i + 1]) && !char.IsUpper(abz[i + 1]))//предполагается, что мы сейчас на пробеле
                {
                    buk = char.ToUpper(abz[i + 1]);
                    abz = abz[0..(i + 1)] + buk + abz[(i + 2)..abz.Length];
                }
            }
        }
    }
}