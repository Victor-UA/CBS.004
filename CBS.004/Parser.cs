using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CBS._004
{
    class Parser
    {
        public Parser(string text)
        {
            Text = text;
        }

        public string Text { get; }


        public ICollection Parse(string pattern)
        {
            var regex = new Regex(pattern);
            var result = regex.Matches(Text); 
            return result;
        }

        public ICollection Parse2File(string caption, string pattern, string path, ICollection<string> regexGroups = null)
        {
            var result = Parse(pattern);
            SaveCollectionToFile(caption, result as MatchCollection, path, regexGroups);
            return result;
        }

        private void SaveCollectionToFile(string caption, MatchCollection collection, string path, ICollection<string> regexGroups = null)
        {
            using (var streamWriter = File.AppendText(path))
            {
                try
                {
                    streamWriter.WriteLine(caption);
                    foreach (Match item in collection)
                    {
                        StringBuilder builder = new StringBuilder();
                        if (regexGroups != null)
                        {
                            foreach (var @group in regexGroups)
                            {
                                builder.Append($"{item.Groups[@group]} : ");
                            }

                            builder.Remove(builder.Length - 3, 3);
                            streamWriter.WriteLine($"{builder}");
                        }
                        else
                        {
                            streamWriter.WriteLine(item.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    streamWriter.Close();
                }
            }            
        }
    }
}
