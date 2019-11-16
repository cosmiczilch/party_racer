using System.Collections.Generic;
using System.IO;
using TimiShared.Debug;
using TimiShared.Loading;

namespace TimiShared.Utils {

    public class CSVReader {

        public class CSVResult {
            public List<CSVItem> items;
            public List<string> keysPerItem;

            public CSVResult() {
                this.items = new List<CSVItem>();
                this.keysPerItem = new List<string>();
            }
        }

        public class CSVItem {
            public Dictionary<string, string> values = new Dictionary<string, string>();
        }

        // Reads a CSV that is formatted as comma separated values per line
        // The first line must contain the legend
        public static CSVResult ReadCSVFile(TimiSharedURI fileURI) {
            CSVResult result = null;
            using (Stream fileStream = FileLoader.GetFileStreamSync(fileURI, FileMode.Open, FileAccess.Read)) {

                if (fileStream == null) {
                    return null;
                }

                StreamReader streamReader = new StreamReader(fileStream);
                if (streamReader.Peek() < 0) {
                    DebugLog.LogErrorColor("Empty file", LogColor.grey);
                    return null;
                }

                result = new CSVResult();

                // Read the legend from the first line
                string legendLine = streamReader.ReadLine();
                string[] legend = legendLine.Split(',');
                for (int i = 0; i < legend.Length; ++i) {
                    result.keysPerItem.Add(legend[i]);
                }

                int lineNumber = 0;
                while (streamReader.Peek() >=0) {
                    ++lineNumber;

                    string line = streamReader.ReadLine();
                    string[] words = line.Split(',');

                    if (result.keysPerItem.Count != words.Length) {
                        DebugLog.LogWarningColor("Malformed item on line number: " + lineNumber, LogColor.grey);
                        continue;
                    }

                    CSVItem item = new CSVItem();
                    for (int i = 0; i < result.keysPerItem.Count; ++i) {
                        item.values[result.keysPerItem[i]] = words[i];
                    }
                    result.items.Add(item);
                }

                fileStream.Close();
            }

            return result;
        }
    }
}