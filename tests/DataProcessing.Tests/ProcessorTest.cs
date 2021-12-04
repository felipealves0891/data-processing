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
                new string[] { "2", "04/11/2021", "Pendente"}
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
            var mockInput = ConfigureMockInput();
            var mockOutput = ConfigureMockOutput();
            var mockTrans = ConfigureMockTransformation();
            var processor = new Processor(mockInput.Object, mockOutput.Object);

            processor.AddTransformation(mockTrans.Object);

            var eventCounter = 0;
            processor.Track += (sender, statistic) => {
                eventCounter++;
            };

            //Act
            var lines = processor.Run();

            //Assert
            Assert.Equal(4, lines);
            Assert.Equal(4, eventCounter);
            mockInput.Verify();
            mockTrans.Verify();
            mockOutput.Verify();
        }

        private Mock<IInput> ConfigureMockInput()
        {
            var mockInput = new Mock<IInput>();

            mockInput.SetupSequence(x => x.HasData())
                    .Returns(true)
                    .Returns(true)
                    .Returns(true)
                    .Returns(true)
                    .Returns(false)
                    .Throws(new Exception("Loop deve encerrar após o false"));

            mockInput.SetupSequence(x => x.GetData())
                    .Returns(_inputData[0])
                    .Returns(_inputData[1])
                    .Returns(_inputData[2])
                    .Returns(_inputData[3])
                    .Throws(new Exception("Consulta após o fim dos dados"));

            mockInput.Setup(x => x.Close()).Verifiable();

            return mockInput;
        }

        private Mock<IOutput> ConfigureMockOutput()
        {
            var mockOutput = new Mock<IOutput>();

            mockOutput.Setup(x => x.Set(It.Is<string[]>(s => s == _outputData[0]))).Verifiable();
            mockOutput.Setup(x => x.Set(It.Is<string[]>(s => s == _outputData[1]))).Verifiable();
            mockOutput.Setup(x => x.Set(It.Is<string[]>(s => s == _outputData[2]))).Verifiable();

            mockOutput.Setup(x => x.Close()).Verifiable();

            return mockOutput;
        }

        private Mock<ITransformation> ConfigureMockTransformation()
        {
            var mockTrans = new Mock<ITransformation>();

            mockTrans.SetupSequence(x => x.Transform(It.IsAny<string[]>()))
                .Returns(_outputData[0])
                .Returns(_outputData[1])
                .Returns(_outputData[2])
                .Returns(new string[] {})
                .Throws(new Exception("São esperados 3 entradas"));

            return mockTrans;
        }

        [Fact]
        public void ExistsTransformationTest()
        {
            //Arrange
            Mock<ITransformation> mockTrans1 = new Mock<ITransformation>();
            Mock<ITransformation> mockTrans2 = new Mock<ITransformation>();
            Mock<IInput> mockInput = new Mock<IInput>();
            Mock<IOutput> mockOutput = new Mock<IOutput>();

            //Act
            Processor processor = new Processor(mockInput.Object, mockOutput.Object);
            processor.AddTransformation(mockTrans1.Object);

            //Assert
            Assert.True(processor.ExistsTransformation(mockTrans1.Object), 
                "Valida se a transformação 1 existe!");
            Assert.False(processor.ExistsTransformation(mockTrans2.Object), 
                "Valida se a transformação 2 não existe!");
        }

        [Fact]
        public void RemoveTransformation()
        {
            //Arrange
            Mock<ITransformation> mockTrans1 = new Mock<ITransformation>();
            Mock<IInput> mockInput = new Mock<IInput>();
            Mock<IOutput> mockOutput = new Mock<IOutput>();

            //Act
            Processor processor = new Processor(mockInput.Object, mockOutput.Object);
            ITransformation transformation = mockTrans1.Object;
            processor.AddTransformation(transformation);
            Assert.True(processor.ExistsTransformation(transformation), 
                "Confirma que a transformação foi adicionada!");
            processor.RemoveTransformation(transformation);

            //Assert
            Assert.False(processor.ExistsTransformation(transformation), 
                "Valida se a transformação foi removida!");
        }
    }
}