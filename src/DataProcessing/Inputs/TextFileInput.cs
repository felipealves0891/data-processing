using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DataProcessing.Inputs
{
    public class TextFileInput : IInput
    {
        private char _delimiter;

        private string? _line;

        private StreamReader _stream;

        private List<string> _columns = new List<string>();

        private string _column = string.Empty;

        private bool _isEscape = false;

        public TextFileInput(Stream stream, char delimiter = ';')
        {
            _stream = new StreamReader(stream, Encoding.GetEncoding("iso-8859-1"));
            _delimiter = delimiter;
        }

        public bool HasData()
        {
            _line = _stream.ReadLine();
            return _line != null;
        }

        public string[] GetData()
        {
            if(_line == null)
                return new string[] {};
            
            _columns.Clear();
            _column = string.Empty;
            _isEscape = false;

            for (int i = 0; i < _line.Length; i++)
            {
                if(_line[i] == '"')
                {
                    _isEscape = !_isEscape;
                    continue;
                }
                
                if(_line[i] == _delimiter && !_isEscape)
                {
                    _columns.Add(_column);
                    _column = string.Empty;
                }
                else
                {
                    _column += _line[i];
                }
            }

            _columns.Add(_column);
            return _columns.ToArray();
        }

        public void Close()
        {
            _stream.Dispose();
        }

    }
}