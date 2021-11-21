using DataProcessing.Inputs;
using System;
using System.IO;
using Xunit;

namespace DataProcessing.Tests.Inputs
{
    public class TextFileInputTest
    {
        private string _inputData;

        private string[][] _outputData;

        public TextFileInputTest()
        {
            var newLine = Environment.NewLine;
            _inputData = $"0;01/11/2021;Aprovado{newLine}1;02/11/2021;Aprovado{newLine}3;03/11/2021;Reprovado";

            _outputData = new string[][]
            {
                new string[] { "0", "01/11/2021", "Aprovado"},
                new string[] { "1", "02/11/2021", "Aprovado"}, 
                new string[] { "3", "03/11/2021", "Reprovado"},
            };
            
        }

        [Fact]
        public void GetDataTest()
        {
            //Arrange
            var filename = Path.GetTempFileName();
            using(StreamWriter sw = new StreamWriter(filename))
                sw.WriteLine(_inputData);

            var TextFileInput = new TextFileInput(File.Open(filename, FileMode.Open));
            var reads = new string[3][];
            var counter = 0;

            //Act
            while(TextFileInput.HasData())
                reads[counter++] = TextFileInput.GetData();

            //Assert
            for(var i = 0; i < 3; i++)
            {
                Assert.Equal(_outputData[i][0] ,reads[i][0]);
                Assert.Equal(_outputData[i][1] ,reads[i][1]);
                Assert.Equal(_outputData[i][2] ,reads[i][2]);
            }

            TextFileInput.Close();
        }

    }
}