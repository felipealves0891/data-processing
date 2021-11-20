using DataProcessing.Inputs;
using DataProcessing.Outputs;
using DataProcessing.Transformations;
using System.Collections.Generic;

namespace DataProcessing
{
    public class Processor
    {
        private readonly IInput _input;

        private readonly IOutput _output;

        private List<ITransformation> _transformations;

        private int _readLines;

        public Processor(IInput input, IOutput output)
        {
            _input = input;
            _output = output;
            _transformations = new List<ITransformation>();
        }

        public Processor AddTransformation(ITransformation transformation)
        {
            if(_transformations.Contains(transformation))
                return this;

            _transformations.Add(transformation);
            return this;
        }

        public Processor RemoveTransformation(ITransformation transformation)
        {
            _transformations.Remove(transformation);
            return this;
        }

        public int Run()
        {
            _readLines = 0;

            while(_input.HasData())
            {
                var data = _input.GetData();
                
                foreach(var transformation in _transformations)
                    data = transformation.Transform(data);

                _output.Set(data);
                _readLines++;
            }

            _input.Close();
            _output.Close();
            
            return _readLines;
        }
    }
}