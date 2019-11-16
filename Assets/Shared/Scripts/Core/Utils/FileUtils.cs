using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TimiShared.Debug;
using TimiShared.Loading;
using UnityEngine;

namespace TimiShared.Utils {
    public static class FileUtils {

        public static bool DoesFileExist(TimiSharedURI uri) {
            if (uri.BasePathType == FileBasePathType.LocalDataPath ||
                uri.BasePathType == FileBasePathType.LocalPersistentDataPath ||
                uri.BasePathType == FileBasePathType.LocalStreamingAssetsPath) {

                return File.Exists(uri.GetFullPath());

            } else {
                string errorMessage = "Not supported to check file exists on base path type: " + uri.BasePathType;
                DebugLog.LogErrorColor(errorMessage, LogColor.grey);
                throw new NotImplementedException(errorMessage);
            }
        }

        public static bool DoesDirectoryExist(TimiSharedURI uri) {
            if (uri.BasePathType == FileBasePathType.LocalDataPath ||
                uri.BasePathType == FileBasePathType.LocalPersistentDataPath ||
                uri.BasePathType == FileBasePathType.LocalStreamingAssetsPath) {

                return Directory.Exists(uri.GetFullPath());

            } else {
                string errorMessage = "Not supported to check directory exists on base path type: " + uri.BasePathType;
                DebugLog.LogErrorColor(errorMessage, LogColor.grey);
                throw new NotImplementedException(errorMessage);
            }
        }

        public static void DeleteDirectory(TimiSharedURI uri) {
            if (uri.BasePathType == FileBasePathType.LocalDataPath ||
                uri.BasePathType == FileBasePathType.LocalPersistentDataPath ||
                (uri.BasePathType == FileBasePathType.LocalStreamingAssetsPath && Application.isEditor)) {

                Directory.Delete(uri.GetFullPath(), true);

            } else {
                string errorMessage = "Not supported to delete directory on base path type: " + uri.BasePathType;
                DebugLog.LogErrorColor(errorMessage, LogColor.grey);
                throw new NotImplementedException(errorMessage);
            }

        }

        public static void CreateDirectory(TimiSharedURI uri) {
            if (uri.BasePathType == FileBasePathType.LocalDataPath ||
                uri.BasePathType == FileBasePathType.LocalPersistentDataPath ||
                (uri.BasePathType == FileBasePathType.LocalStreamingAssetsPath && Application.isEditor)) {

                Directory.CreateDirectory(uri.GetFullPath());

            } else {
                string errorMessage = "Not supported to create directory on base path type: " + uri.BasePathType;
                DebugLog.LogErrorColor(errorMessage, LogColor.grey);
                throw new NotImplementedException(errorMessage);
            }

        }

        public static List<TimiSharedURI> GetDirectoriesInDirectory(TimiSharedURI directoryUri) {
            string[] directoryPathNames = Directory.GetDirectories(directoryUri.GetFullPath());
            List<TimiSharedURI> directoryURIs = new List<TimiSharedURI>();

            for (int i = 0; i < directoryPathNames.Length; ++i) {
                string directoryName = Path.GetFileName(directoryPathNames[i]);
                directoryURIs.Add(new TimiSharedURI(directoryUri.BasePathType, Path.Combine(directoryUri.RelativePath, directoryName)));
            }

            return directoryURIs;
        }

        public static List<TimiSharedURI> GetFilesInDirectory(TimiSharedURI directoryUri) {
            string[] filePathNames = Directory.GetFiles(directoryUri.GetFullPath());
            List<TimiSharedURI> fileURIs = new List<TimiSharedURI>();

            for (int i = 0; i < filePathNames.Length; ++i) {
                string fileName = Path.GetFileName(filePathNames[i]);
                string extension = Path.GetExtension(fileName);
                if (extension == ".meta" || extension == ".DS_Store") {
                    // Skip Unity meta files and mac DS_Store files
                    continue;
                }
                fileURIs.Add(new TimiSharedURI(directoryUri.BasePathType, Path.Combine(directoryUri.RelativePath, fileName)));
            }

            return fileURIs;
        }

        public static IEnumerator CopyDirectoryContents(TimiSharedURI sourceDirectory, TimiSharedURI destinationDirectory) {
            if (destinationDirectory.BasePathType == FileBasePathType.LocalDataPath ||
                destinationDirectory.BasePathType == FileBasePathType.LocalPersistentDataPath ||
                (destinationDirectory.BasePathType == FileBasePathType.LocalStreamingAssetsPath && Application.isEditor)) {


                if (!FileUtils.DoesDirectoryExist(sourceDirectory)) {
                    DebugLog.LogWarningColor("Source directory does not exist: " + sourceDirectory.GetFullPath(), LogColor.grey);
                    yield break;
                }

                if (!FileUtils.DoesDirectoryExist(destinationDirectory)) {
                    FileUtils.CreateDirectory(destinationDirectory);
                }

                List<TimiSharedURI> fileURIs = FileUtils.GetFilesInDirectory(sourceDirectory);
                List<TimiSharedURI> directoryURIs = FileUtils.GetDirectoriesInDirectory(sourceDirectory);

                for (int i = 0; i < fileURIs.Count; ++i) {
                    // TODO: Make this parallel for all the files
                    yield return FileUtils.CopyFile(fileURIs[i], destinationDirectory);
                }
                for (int i = 0; i < directoryURIs.Count; ++i) {
                    TimiSharedURI destination = new TimiSharedURI(destinationDirectory.BasePathType,
                            Path.Combine(destinationDirectory.RelativePath, directoryURIs[i].FileName));
                    // TODO: Make this parallel for all the files
                    yield return FileUtils.CopyDirectoryContents(directoryURIs[i], destination);
                }

                yield break;

            } else {
                string errorMessage = "Not supported to write on base path type: " + destinationDirectory.BasePathType;
                DebugLog.LogErrorColor(errorMessage, LogColor.grey);
                throw new NotImplementedException(errorMessage);
            }
        }

        public static IEnumerator CopyFile(TimiSharedURI sourceFileURI, TimiSharedURI destinationDirectoryURI) {
            if (destinationDirectoryURI.BasePathType == FileBasePathType.LocalDataPath ||
                destinationDirectoryURI.BasePathType == FileBasePathType.LocalPersistentDataPath ||
                (destinationDirectoryURI.BasePathType == FileBasePathType.LocalStreamingAssetsPath && Application.isEditor)) {

                FileLoadRequest fileLoadRequest = FileLoader.GetFileStreamAsync(sourceFileURI, FileMode.Open, FileAccess.Read);
                if (fileLoadRequest != null) {
                    fileLoadRequest.StartRequest();
                    yield return fileLoadRequest;
                    string contents = FileUtils.GetStreamContents(fileLoadRequest.LoadedFileStream);

                    TimiSharedURI destinationFileURI = new TimiSharedURI(destinationDirectoryURI.BasePathType,
                            Path.Combine(destinationDirectoryURI.RelativePath, sourceFileURI.FileName));
                    using (Stream destinationFileStream = FileLoader.GetFileStreamSync(destinationFileURI, FileMode.Create, FileAccess.Write)) {
                        if (destinationFileStream != null) {
                            FileUtils.PutStreamContents(destinationFileStream, contents);
                        }
                        destinationFileStream.Close();
                    }

                }

                yield break;

            } else {
                string errorMessage = "Not supported to write on base path type: " + destinationDirectoryURI.BasePathType;
                DebugLog.LogErrorColor(errorMessage, LogColor.grey);
                throw new NotImplementedException(errorMessage);
            }
        }

        public static string GetStreamContents(Stream stream) {
            stream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

        public static void PutStreamContents(Stream stream, string contents) {
            StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.Write(contents);
            streamWriter.Flush();
            stream.Flush();
        }

        public static void WriteFile(TimiSharedURI fileURI, string contents) {
            using (Stream fileStream = FileLoader.GetFileStreamSync(fileURI, FileMode.Create, FileAccess.ReadWrite)) {
                using (StreamWriter streamWriter = new StreamWriter(fileStream)) {

                    streamWriter.Write(contents);
                    fileStream.Flush();
                    streamWriter.Flush();
                }
            }
        }
    }
}
