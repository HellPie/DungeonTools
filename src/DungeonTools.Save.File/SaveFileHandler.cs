using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DungeonTools.Save.File {
    public static class SaveFileHandler {
        private static readonly byte[] Magic1 = {0x44, 0x30, 0x30, 0x31};
        private static readonly byte[] Magic2 = {0x00, 0x00, 0x00, 0x00};

        public static bool IsFileEncrypted(Stream stream) {
            if(!stream.CanRead) throw new ArgumentException();
            if(!stream.CanSeek) throw new ArgumentException();

            return StartsWithMagic(stream);
        }

        public static bool StartsWithMagic(Stream stream) {
            if(stream.Length < Magic1.Length + Magic2.Length) return false;

            long position = stream.Position;
            byte[] magic1 = new byte[Magic1.Length];
            stream.Read(magic1);

            byte[] magic2 = new byte[Magic2.Length];
            stream.Read(magic2);

            if(!Magic1.SequenceEqual(magic1)) {
                stream.Position = position;
                return false;
            }

            if(!Magic2.SequenceEqual(magic2)) {
                throw new Exception("File started with magic but next 4 bytes were non-zero");
            }

            return true;
        }

        public static Stream RemoveTrailingZeroes(Stream stream) {
            using BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, true); // This is not text

            while(reader.BaseStream.Position < reader.BaseStream.Length) {
                if(reader.ReadByte() != 0) {
                    continue;
                }

                // Remove this zero and any after this. supposed to be text but the cipher adds some junk
                stream.SetLength(reader.BaseStream.Position - 1);
                break;
            }

            stream.Position = 0;
            return stream;
        }

        public static Stream PrependMagicToEncrypted(Stream stream) {
            MemoryStream output = new MemoryStream();
            output.Write(Magic1);
            output.Write(Magic2);
            stream.CopyTo(output);
            return output;
        }
    }
}
