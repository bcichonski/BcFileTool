using BcFileTool.Library.Constants;
using BcFileTool.Library.Interfaces.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace BcFileTool.Library.Services
{
    public class ExifTagReaderService : IExifTagReaderService
    {
        const int FileBufferSize = 4096;

        public DateTime ReadCreationTags(string path)
        {
            using (var fileStream = new FileStream(path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                FileBufferSize,
                true))
            {
                var imageObject = Image.Identify(fileStream);

                var source = imageObject?.Metadata?.ExifProfile?.GetValue(ExifTag.DateTime);
                if (source != null)
                {
                    var tag = ReadTag(source);
                    return tag;
                }
            }
            return DateTime.MinValue;
        }

        DateTime ReadTag(ExifValue source)
        {
            DateTime tag = DateTime.MinValue;
            string tagString = string.Empty;
            switch (source.DataType)
            {
                case ExifDataType.Ascii:
                    tagString = source.Value?.ToString() ?? string.Empty;
                    break;
                default:
                    if (source.IsArray)
                    {
                        tagString = Encoding.Unicode.GetString((byte[])source.Value);
                        tagString = tagString.Replace("\0", "");
                    }
                    break;
            }

            var datetagstring = GetFirstDigits(tagString, Const.DateFormat.Length);

            if(datetagstring.Length == Const.DateFormat.Length)
            {
                tag = ParseDate(datetagstring);
            }

            return tag;
        }

        

        public string GetFirstDigits(string tagString, int count)
        {
            var i = 0;
            var firstdigts = string.Empty;
            while(i < tagString.Length && firstdigts.Length < count)
            {
                if(char.IsDigit(tagString[i]))
                {
                    firstdigts += tagString[i];
                }
                i++;
            }
            return firstdigts;
        }

        public DateTime ParseDate(string str)
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParseExact(str,
                    Const.DateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out date);
            return date;
        }
    }
}
