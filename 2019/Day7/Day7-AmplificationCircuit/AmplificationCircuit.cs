using Day7_AmplificationCircuit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day7_AmplificationCircuit
{
    public class AmplificationCircuit : IDisposable
    {
        private readonly bool _hasFeedbackLoop;

        private readonly List<Amplifier> _amplifiers = new List<Amplifier>();
        private readonly List<SignalQueue> _signals = new List<SignalQueue>();

        public AmplificationCircuit(IEnumerable<int> program, IEnumerable<int> phaseSettings, bool withFeedbackLoop = false)
        {
            var settings = new List<int>(phaseSettings);
            _hasFeedbackLoop = withFeedbackLoop;

            if (withFeedbackLoop)
            {
                CreateSignals(settings.Count);
            }
            else
            {
                CreateSignals(settings.Count + 1);
            }

            CreateAmplifiers(program, settings);
        }

        private void CreateSignals(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                _signals.Add(new SignalQueue());
            }
        }

        private void CreateAmplifiers(IEnumerable<int> program, List<int> phaseSettings)
        {
            int last = phaseSettings.Count - 1;

            for (int i = 0; i <= last; i++)
            {
                var input = _signals[i];

                SignalQueue output;
                if (_hasFeedbackLoop && i == last)
                {
                    output = _signals[0];
                }
                else
                {
                    output = _signals[i + 1];
                }

                _amplifiers.Add(new Amplifier(program, phaseSettings[i], input, output));
            }
        }

        public int GetOutputSignal()
        {
            _signals[0].Set(0);

            var ampTasks = new List<Task>();

            foreach(var amp in _amplifiers)
            {
                ampTasks.Add(Task.Run(() => amp.Run()));
            }

            Task.WaitAll(ampTasks.ToArray());
            var lastSignal = _hasFeedbackLoop ? _signals[0] : _signals[_signals.Count - 1];

            return lastSignal.Get();
        }

        public void Dispose()
        {
            foreach(var signal in _signals)
            {
                signal.Dispose();
            }
        }
    }
}
