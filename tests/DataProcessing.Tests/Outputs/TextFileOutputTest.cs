using DataProcessing.Outputs;
using System;
using System.IO;
using Xunit;

namespace DataProcessing.Tests.Outputs
{
    public class TextFileOutputTest
    {
        private string _inputData;

        private string[][] _outputData;

        public TextFileOutputTest()
        {
            var newLine = Environment.NewLine;
            _inputData = $"0;01/11/2021;Aprovado{newLine}1;02/11/2021;Aprovado{newLine}3;03/11/2021;Reprovado{newLine}";

            _outputData = new string[][]
            {
                new string[] { "0", "01/11/2021", "Aprovado"},
                new string[] { "1", "02/11/2021", "Aprovado"}, 
                new string[] { "3", "03/11/2021", "Reprovado"},
            };
        }

        [Fact]
        public void SetTest()
        {
            //Arrange
            var filename = Path.GetTempFileName();
            var textFileOutput = new TextFileOutput(File.Open(filename, FileMode.Open));

            //Act
            foreach(var line in _outputData)
                textFileOutput.Set(line);

            textFileOutput.Close();

            //Assert
            
            var allText = File.ReadAllText(filename);
            Assert.Equal(_inputData, allText);

            
        }
        
    }
}