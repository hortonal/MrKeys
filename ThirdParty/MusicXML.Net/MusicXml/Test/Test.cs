using System;
using MusicXml;

namespace MusicXml.Test
{
    class Test
    {
        public static void Main()
        {
            XScore xcTest = new XScore(@"C:\Users\Alex\MrKeys\ThirdParty\MusicXML.Net\Promenade_Example.xml");

            foreach (Part part in xcTest.Parts)
                foreach (Measure measure in part.Measures)                
                    foreach (Note note in measure.Notes)
                    {
                        Console.WriteLine(note);                        
                    }
             
            Console.ReadLine();

        }
    }
}
