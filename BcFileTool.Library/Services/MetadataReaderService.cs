using BcFileTool.Library.Constants;
using BcFileTool.Library.Interfaces.Services;
using MetadataExtractor;
using MetadataExtractor.Formats.Avi;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.QuickTime;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BcFileTool.Library.Services
{
    public class MetadataReaderService : IMetadataReaderService
    {
        static readonly string[] PictureExts = new string[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".tiff",
            ".gif",
            ".raw"
        };

        static readonly string[] AviExts = new string[]
        {
            ".avi"
        };

        static readonly string[] QTFormats = new string[] {
            ".mp4",
            ".m4v",
            ".mov"
        };

        bool IsFormat(string path, string[] formatList)
        {
            var ext = Path.GetExtension(path);
            return formatList.Contains(ext.ToLowerInvariant());
        }

        public bool IsDateValid(DateTime date)
        {
            return date.Year > AssumedMinimalYearValue
                    && date.Year <= AssumedMaximalYearValue;
        }

        public DateTime ReadCreationTags(string path)
        {
            IEnumerable<MetadataExtractor.Directory> metaDirs = null;
            try
            {

                if (IsFormat(path, PictureExts))
                {
                    metaDirs = ImageMetadataReader.ReadMetadata(path);
                }
                else
                if (IsFormat(path, AviExts))
                {
                    metaDirs = AviMetadataReader.ReadMetadata(path);
                }
                else
                if (IsFormat(path, QTFormats))
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        metaDirs = QuickTimeMetadataReader.ReadMetadata(fs);
                    }
                }
            } catch
            {
                //well, that is not ok, we coud at least log it somewhere if it only was so important to us..
            }

            var date = DateTime.MaxValue;

            if (metaDirs != null)
            {
                foreach (var directory in metaDirs)
                {
                    if (directory is FileMetadataDirectory)
                    {
                        continue;
                    }
                    foreach (var tag in directory.Tags)
                    {
                        if (tag.Name.ToLowerInvariant().Contains("date"))
                        {
                            DateTime fdate;
                            try
                            {
                                if (directory.TryGetDateTime(tag.Type, out fdate))
                                {
                                    if (fdate < date && IsDateValid(fdate))
                                    {
                                        date = fdate;
                                    }
                                }
                            }
                            catch
                            {
                                //well, that is not ok, we coud at least log it somewhere if it only was so important to us..
                            }
                        }
                    }
                }
            }

            if (date == DateTime.MaxValue)
            {
                date = DateTime.MinValue;//just to be safe
            }

            return date;
        }

        public string GetFirstDigits(string tagString, int count)
        {
            var i = 0;
            var firstdigts = string.Empty;
            while (i < tagString.Length && firstdigts.Length < count)
            {
                if (char.IsDigit(tagString[i]))
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

        private const int AssumedMinimalYearValue = 1900;
        static readonly int AssumedMaximalYearValue = DateTime.Today.Year;
    }
}
