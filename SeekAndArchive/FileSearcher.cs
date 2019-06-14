﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SeekAndArchive
{
    class FileSearcher
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
            var currentDirMatch = directory.GetFiles(fileName);
            List<FileInfo> fileInfoList = new List<FileInfo>(currentDirMatch);

            if (directory.GetDirectories() != null)
            {
                foreach (var item in directory.GetDirectories())
                {
                    fileInfoList.AddRange(SearchInDir(fileName, item));
                }
            }

            return fileInfoList;

        }
    }

}
