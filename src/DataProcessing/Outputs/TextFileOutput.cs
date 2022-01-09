using System;
using System.IO;
using System.Text;

namespace DataProcessing.Outputs
{
    public class TextFileOutput : IOutput
    {
        private StreamWriter _stream;

        private char _delimiter;

        public TextFileOutput(Stream stream, char delimiter = ';')
        {
            _stream = new StreamWriter(stream, Encoding.GetEncoding("iso-8859-1"));
            _delimiter = delimiter;
        }

        public void Set(string[] data)
        {
            var builder = new StringBuilder();
            builder.Append(data[0]);

            for(var i = 1; i < data.Length; i++)
            {
                builder.Append(_delimiter);

                if(data[i].IndexOf(_delimiter) > -1)
                {
                    builder.Append('"');
                    builder.Append(data[i]);
                    builder.Append('"');
                }
                else
                {
                    builder.Append(data[i]);
                }
                
            } 

            _stream.WriteLine(builder);
        }

        public void Close()
        {
            _stream.Dispose();
        }
    }
}