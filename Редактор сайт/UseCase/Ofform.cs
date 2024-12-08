using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace Редактор_сайт.UseCase
{
    public class Ofform
    {
        private bool _addTab;
        private bool _addParagraph;
        private string _inputText;
        private string _outputText;
        private string[] _text;
        private bool[] _isDialog;
        private int _numberParagraph;
        
        public Ofform()
        {

        }
        public Data Format(Data inputData)
        {
            _addTab = inputData.addTab;
            _addParagraph = inputData.addParagraph;
            _inputText = inputData.text.Replace("\r", "");
            _outputText = "";

            _text = _inputText.Split('\n')
                            .Select(line => line.Trim())
                            .Where(line => !string.IsNullOrWhiteSpace(line))
                            .ToArray();
            _isDialog = new bool[_text.Length];

            Reading();//основная функция

            Data outputData = new Data()
            {
                text = _outputText,
                addTab = _addTab,
                addParagraph = _addParagraph,
            };
            return outputData;
        }
        private void Reading()//читаем по абзацу 
        {
            if (_text == null || _text.Length == 0) return;

            for (_numberParagraph = 0; _numberParagraph < _text.Length; _numberParagraph++)
            {
                if (_text[_numberParagraph].Trim() != "")
                {
                    _text[_numberParagraph] = _text[_numberParagraph].TrimStart();

                    RemoveBracketsContent();// Удаляем всякие теги в < >
                    ReplaceDashes(); // Заменяем все возможные тире на обычное тире
                    ReplaceGuillemotsWithQuotes(); // Заменяем кавычки
                    ReplaceDashWithTireWithSpace(); // Заменяем дефис с пробелом на тире с пробелом
                    CapitalizeFirstLetters(); // Заменяем на заглавные буквы
                    FinalizeSentences(); // Ставим точку
                    IsDialog(); // Проверяем, является ли строка диалогом

                    // Добавляем абзац
                    if (_addParagraph && _numberParagraph > 0
                        && _isDialog[_numberParagraph] != _isDialog[_numberParagraph - 1])
                    {
                        AddParagraphSpacing();
                    }

                    // Добавляем таб и центр
                    if (_addTab)
                    {
                        AddTab();
                        AddCenter();
                    }
                    else
                        AddParagraphCenter();

                    _outputText += _text[_numberParagraph] + "\n";
                }

            }
        }
        private void RemoveBracketsContent()
        {
            // Удаляем все, что находится в <...>
            _text[_numberParagraph] = Regex.Replace(_text[_numberParagraph], "<.*?>", string.Empty).Trim();
        }
        private void ReplaceDashes()
        {
            // Заменяем разные варианты тире на обычное тире
            _text[_numberParagraph] = _text[_numberParagraph]
                .Replace("–", "-")
                .Replace("—", "-")
                .Replace("―", "-")
                .Replace("−", "-");
        }
        private void IsDialog()
        {
            // Проверяем, начинается ли строка с тире
            if (_text[_numberParagraph].StartsWith('—'))
            {
                _isDialog[_numberParagraph] = true; // Если начинается, устанавливаем true
            }
            else
            {
                _isDialog[_numberParagraph] = false; // Иначе false
            }
        }
        private void ReplaceDashWithTireWithSpace()
        {
            if (_text[_numberParagraph].StartsWith("-") && !_text[_numberParagraph].StartsWith("- "))
            {
                _text[_numberParagraph] = "— " + _text[_numberParagraph].Substring(1);
            }

            // Заменяем дефис с пробелом на тире с пробелом
            _text[_numberParagraph] = _text[_numberParagraph].Replace("- ", "— ");

        }
        private void AddTab()
        {
            if (!Regex.IsMatch(_text[_numberParagraph], @"\*\*") && !_text[_numberParagraph].StartsWith("   \n"))
            {
                // Добавляем <tab> в начало строки
                _text[_numberParagraph] = "<tab>" + _text[_numberParagraph];
            }

        }
        private void AddCenter()
        {
            if (Regex.IsMatch(_text[_numberParagraph], @"\*\*")) // Проверка на наличие только * в строке
            {
                // Заменяем строку на форматированную
                _text[_numberParagraph] = $"\n<center>{_text[_numberParagraph].Replace(".","")}</center>";
            }
        }
        private void AddParagraphCenter()
        {
            if (Regex.IsMatch(_text[_numberParagraph], @"\*\*")) // Проверка на наличие только * в строке
            {
                // Заменяем строку на форматированную
                _text[_numberParagraph] = $"\n{_text[_numberParagraph].Replace(".", "")}\n";
            }

        }
        private void AddParagraphSpacing()
        {
            if (!Regex.IsMatch(_text[_numberParagraph], @"\*\*"))
            {
                if(!(Regex.IsMatch(_text[_numberParagraph-1], @"\*\*") && _isDialog[_numberParagraph]))// Надо пропустить, если предыдущее центер, а эта диалог
                { 
                    if (_addTab)
                    {
                        _text[_numberParagraph] = "   \n<tab>" + _text[_numberParagraph];
                    }
                    else
                    {
                        // Добавляем три пробела перед текстом и новую строку
                        _text[_numberParagraph] = "   \n" + _text[_numberParagraph];
                    }
                }
            }

        }
        private void FinalizeSentences()
        {
            // Получаем последний символ абзаца
            char lastChar = _text[_numberParagraph].TrimEnd().LastOrDefault();
            if (lastChar == '"')
            {
                char lastLastChar = _text[_numberParagraph].Length > 2 ? _text[_numberParagraph][_text[_numberParagraph].Length - 2] : '"';
                // Проверяем, есть ли уже знак в конце предложения
                if (!IsPunctuation(lastLastChar))
                {
                    _text[_numberParagraph] = _text[_numberParagraph].TrimEnd() + ".";
                }
            }
            else
            {
                // Проверяем, есть ли уже знак в конце предложения
                if (!IsPunctuation(lastChar))
                {
                    _text[_numberParagraph] = _text[_numberParagraph].TrimEnd() + ".";
                }
            }

        }
        private bool IsPunctuation(char character)
        {
            // Проверяем, если символ является знаком препинания
            return character == '.' || character == '!' || character == '?' || character == ':' ||
                   character == '…' || character == ',' || character == '*';
        }
        private void ReplaceGuillemotsWithQuotes()
        {
            // Заменяем кавычки-елочки на двойные кавычки
            _text[_numberParagraph] = _text[_numberParagraph]
                .Replace("«", "\"")   // Заменяем открывающую елочку
                .Replace("»", "\"");   // Заменяем закрывающую елочку

        }
        private void CapitalizeFirstLetters()
        {
            // Обработка начала строки
            if (!string.IsNullOrWhiteSpace(_text[_numberParagraph]) &&
                char.IsLower(_text[_numberParagraph][0]))
            {
                _text[_numberParagraph] = char.ToUpper(_text[_numberParagraph][0]) + _text[_numberParagraph].Substring(1);
            }

            if (_text[_numberParagraph].StartsWith("— "))
            {
                // Делаем первую букву заглавной
                if (_text[_numberParagraph].Length > 2 && !char.IsUpper(_text[_numberParagraph][2]))
                {
                    _text[_numberParagraph] = "— " + char.ToUpper(_text[_numberParagraph][2]) + _text[_numberParagraph].Substring(3);
                }
            }


            // Обработка заглавной буквы после знаков . ! ?
            for (int j = 1; j < _text[_numberParagraph].Length; j++)
            {
                // Если предыдущий символ - знак препинания и есть следующий символ
                if (".!?".Contains(_text[_numberParagraph][j - 1]) &&
                    char.IsLower(_text[_numberParagraph][j]) && 
                    (_text[_numberParagraph].Length>3 & _text[_numberParagraph][j-1]!='.'& _text[_numberParagraph][j - 2] != '.'))
                {
                    _text[_numberParagraph] = _text[_numberParagraph].Substring(0, j) +
                                char.ToUpper(_text[_numberParagraph][j]) +
                                _text[_numberParagraph].Substring(j + 1);
                }
            }
        }
    }
}