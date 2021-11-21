using System;
using System.IO;
using System.Text;

namespace DataProcessing.Inputs
{
    public class TextFileInput : IInput
    {
        private char _delimiter;

        private string? _line;

        private StreamReader _stream;

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

            return _line.Split(_delimiter);
        }

        public void Close()
        {
            _stream.Dispose();
        }

    }
}