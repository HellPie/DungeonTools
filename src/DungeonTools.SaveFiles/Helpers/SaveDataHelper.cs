using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DungeonTools.SaveFiles.Helpers {
    public static class SaveDataHelper {
        private static readonly byte[] Magic = {0x44, 0x30, 0x30, 0x31, 0x00, 0x00, 0x00, 0x00};

        public static DataType GetDataType(Stream stream) {
            if(!stream.CanRead || stream.Length <= Magic.Length) {
                return DataType.Unsupported;
            }

            if(HasMagic(stream)) {
                // Only encrypted data can start with Magic
                return DataType.UnsafeEncrypted;
            }

            if(!HasJsonData(stream)) {
                return DataType.Unsupported;
            }

            return HasNullPadding(stream) ? DataType.UnsafeJson : DataType.Json;
        }

        public static Stream CombineEncryptedData(Stream stream) {
            MemoryStream output = new MemoryStream();
            output.Write(Magic);
            stream.CopyTo(output);
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

        public static Stream ExtractEncryptedData(Stream stream) {
            MemoryStream output = new MemoryStream();
            stream.Seek(Magic.Length, SeekOrigin.Current);
            stream.CopyTo(output);
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

        public static Stream ExtractSafeJsonData(Stream stream) {
            MemoryStream output = new MemoryStream();
            stream.CopyTo(output);

            long nullPos = output.Length;
            using BinaryReader reader = new BinaryReader(output, Encoding.UTF8, true);
            while(reader.BaseStream.Position < reader.BaseStream.Length) {
                if(reader.ReadByte() != 0) {
                    continue;
                }

                nullPos = reader.BaseStream.Position;
                break;
            }

            output.Seek(0, SeekOrigin.Begin);
            output.SetLength(nullPos);
            return output;
        }

        private static bool HasMagic(Stream stream) {
            long position = stream.Position;
            bool hasMagic = Magic.SequenceEqual(ReadMagic(stream));
            stream.Seek(position, SeekOrigin.Begin);
            return hasMagic;
        }

        private static bool HasJsonData(Stream stream) {
            long position = stream.Position;
            using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true);
            while(reader.BaseStream.Position < reader.BaseStream.Length) {
                char ch = reader.ReadChar();
                if(char.IsWhiteSpace(ch)) {
                    continue;
                }

                stream.Seek(position, SeekOrigin.Begin);
                return ch == '{';
            }

            stream.Seek(position, SeekOrigin.Begin);
            return false;
        }

        private static bool HasNullPadding(Stream stream) {
            long position = stream.Position;
            using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true);
            while(reader.BaseStream.Position < reader.BaseStream.Length) {
                if(reader.ReadByte() != 0) {
                    continue;
                }

                stream.Seek(position, SeekOrigin.Begin);
                return true;
            }

            stream.Seek(position, SeekOrigin.Begin);
            return false;
        }

        private static IEnumerable<byte> ReadMagic(Stream stream) {
            byte[] magic = new byte[Magic.Length];
            stream.Read(magic, 0, magic.Length);
            return magic;
        }
    }
}
