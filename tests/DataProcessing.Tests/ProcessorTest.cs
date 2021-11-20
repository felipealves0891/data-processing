using DataProcessing.Inputs;
using DataProcessing.Outputs;
using DataProcessing.Transformations;
using Moq;
using System;

using Xunit;

namespace DataProcessing.Tests
{
    public class ProcessorTest
    {
        private string[][] _inputData;

        private string[][] _outputData;

        public ProcessorTest()
        {
            _inputData = new string[][]
            {
                new string[] { "0", "01/11/2021", "Aprovado"},
                new string[] { "1", "02/11/2021", "Aprovado"}, 
                new string[] { "3", "03/11/2021", "Reprovado"},
            };

            _outputData = new string[][]
            {
                new string[] { "0", "2021-11-01", "1"},
                new string[] { "1", "2021-11-02", "1"}, 
                new string[] { "3", "2021-11-03", "0"},
            };
        }

        [Fact]
        public void RunTest()
        {
            //Arrange
            var mockInput = new Mock<IInput>();

            mockInput.SetupSequence(x => x.HasData())
                    .Returns(true)
                    .Returns(true)
                    .Returns(true)
                    .Returns(false)
                    .Throws(new Exception("Loop deve encerrar após o false"));

            mockInput.SetupSequence(x => x.GetData())
                    .Returns(_inputData[0])
                    .Returns(_inputData[1])
                    .Returns(_inputData[2])
                    .Throws(new Exception("Consulta após o fim dos dados"));

            mockInput.Setup(x => x.Close()).Verifiable();

            var mockOutput = new Mock<IOutput>();

            mockOutput.Setup(x => x.Set(It.Is<string[]>(s => s == _outputData[0]))).Verifiable();
            mockOutput.Setup(x => x.Set(It.Is<string[]>(s => s == _outputData[1]))).Verifiable();
            mockOutput.Setup(x => x.Set(It.Is<string[]>(s => s == _outputData[2]))).Verifiable();

            mockOutput.Setup(x => x.Close()).Verifiable();

            var mockTrans = new Mock<ITransformation>();

            mockTrans.SetupSequence(x => x.Transform(It.IsAny<string[]>()))
                .Returns(_outputData[0])
                .Returns(_outputData[1])
                .Returns(_outputData[2])
                .Throws(new Exception("São esperados 3 entradas"));

            var processor = new Processor(mockInput.Object, mockOutput.Object);
            processor.AddTransformation(mockTrans.Object);

            //Act
            processor.Run();

            //Assert
            mockInput.Verify();
            mockTrans.Verify();
            mockOutput.Verify();

        }
    }
}