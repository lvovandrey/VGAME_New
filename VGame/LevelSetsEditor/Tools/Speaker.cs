using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.Tools
{
    public class Speaker
    {

        public static void Speak(string text)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            
                ReadOnlyCollection<InstalledVoice> voices = synthesizer.GetInstalledVoices();// (new CultureInfo("ru-RU"));

                synthesizer.SetOutputToDefaultAudioDevice();
                synthesizer.Volume = 100;  // 0...100
                synthesizer.TtsVolume = 100;
                synthesizer.Rate = 0;     // -10...10
                PromptBuilder prompt = new PromptBuilder();
                prompt.AppendText(text);
                synthesizer.Speak(prompt);
            
        }

    }
}
