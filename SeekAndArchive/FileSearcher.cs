using System;
using System.Collections.Generic;
using System.IO;

namespace SeekAndArchive
{
    internal class FileSearcher
    {
        public IList<FileInfo> Search(string fileName, string directoryPath)
        {
            fileName = "*" + fileName + "*";
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            if (!directory.Exists)
            {
                throw new ArgumentException("Directoy does not exist");
            }
            return SearchInDir(fileName, directory);
        }
        private IList<FileInfo> SearchInDir(string fileName, DirectoryInfo directory)
        {
            FileInfo[] currentDirMatch = directory.GetFiles(fileName);
            List<FileInfo> fileInfoList = new List<FileInfo>(currentDirMatch);

            if (directory.GetDirectories() != null)
            {
                foreach (DirectoryInfo item in directory.GetDirectories())
                {
                    fileInfoList.AddRange(SearchInDir(fileName, item));
                }
            }

            return fileInfoList;

        }
    }

}
