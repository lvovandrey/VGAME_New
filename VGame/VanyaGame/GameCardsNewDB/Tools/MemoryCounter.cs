using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace VanyaGame.GameCardsNewDB.Tools
{
    public class MemoryCounter
    {

        public static bool IsEnoughtMemoryForLevelLoad(GameCardsNewDB.Struct.CardsNewDBLevel level)
        {
            int MaxMemoryMb;
            double SafetyFactor = 1.2;
            double RequiredMemoryMb = CalculateRequiredMemoryForLevel(level) / 1_048_576;
            var ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
            int MemoryAvalableMb = Convert.ToInt32(ramCounter.NextValue());

            if (Is64Bit)
                MaxMemoryMb = 4096;
            else
                MaxMemoryMb = 1024;
            MemoryAvalableMb = MemoryAvalableMb > MaxMemoryMb ? MaxMemoryMb : MemoryAvalableMb;

            return MemoryAvalableMb > SafetyFactor * (RequiredMemoryMb);
        }

        public static double CalculateRequiredMemoryForLevel(GameCardsNewDB.Struct.CardsNewDBLevel level)
        {
            double ImgMemoryKoef = 34;
            double BmpMemoryKoef = 2.5;
            double GifMemoryKoef = 80;

            double RequiredMemory = 0;

            foreach (var card in level.DbLevelRecord.Cards)
            {
                string filename = Sets.Settings.GetInstance().DefaultImage;
                if (File.Exists(card.ImageAddress)) filename = card.ImageAddress;
                if (!File.Exists(filename)) continue;

                long FileSize = new FileInfo(filename).Length;
                switch (Path.GetExtension(filename))
                {
                    case ".jpg":
                    case ".png":
                        RequiredMemory += ImgMemoryKoef * FileSize;
                        break;
                    case ".bmp":
                        RequiredMemory += BmpMemoryKoef * FileSize;
                        break;
                    case ".gif":
                        RequiredMemory += GifMemoryKoef * FileSize;
                        break;
                    default:
                        break;
                }
            }

            return RequiredMemory;
        }

        public static bool Is64Bit
        {

            get { return Marshal.SizeOf(typeof(IntPtr)) == 8; }
        }
    }
}
