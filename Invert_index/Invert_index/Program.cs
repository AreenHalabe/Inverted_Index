// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information


using static System.Net.Mime.MediaTypeNames;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Inverted_Index
{
    class Program
    {
        public static Dictionary<int, string> Documents = new Dictionary<int, string>();
        
        
        public static List<Program> terms = new List<Program>();

        public static List<int> Positions ;


        public string Word;
        public Dictionary<int, List<int>> WordPositionsInDocument ;

        public Program(string word) { 
            this.Word = word;
            WordPositionsInDocument= new Dictionary<int, List<int>>();
        }
        

        public static void Main(string[] args)
        {

        GetAllDocument(@"D:\Mobail\Invert_index\Invert_index\Documents");

            foreach(var documant in Documents) 
            {
                var path = documant.Value;

                var text = File.ReadAllText(path);

                var words = text.ToLower().Split(' ');
                
                int position = 1;
                foreach(var w in words)
                {
                   bool isWordStopWords = IsStopWords(w);

                    if (isWordStopWords)
                    {
                        ++position;
                    }
                    else
                    {
                        bool findwordinDectionary = FindWordInDectionary(w, position, documant.Key);
                        if (findwordinDectionary)
                        {
                            ++position;
                        }
                        else
                        {
                            Positions = new List<int>();
                            Program P = new(w);
                            Positions.Add(position);
                            P.WordPositionsInDocument.Add(documant.Key, Positions);
                            ++position;
                            terms.Add(P);
                        }

                    }

                   

                }
            }



           
            
            using (StreamWriter writer = new StreamWriter("D:\\Mobail\\Invert_index\\Invert_index\\Result\\pos_inverted_index.txt")) {
                foreach(var ob in terms)
                {
                writer.WriteLine(ob.Word + '{');

                foreach(var p in ob.WordPositionsInDocument)
                {
                        writer.Write(p.Key + " : ");
                    foreach(int i in p.Value)
                    {
                            writer.Write(i + " , ");
                    }
                        writer.WriteLine();
                }
                    writer.WriteLine('}');

                }

            };




        }





        static void GetAllDocument(string Folder)
        {
            string[] files = Directory.GetFiles(Folder);
            int documentId = 1;

            foreach (string s in files)
            {
                Documents.Add(documentId, s);
                ++documentId;
            }
        }

        static bool FindWordInDectionary(string word , int position , int documentId)
        {
            bool found = false;

            bool foundInSameDocumentID = false;
            foreach(var ob in terms)
            {
                if(ob.Word == word)
                {
                    foreach(var po in ob.WordPositionsInDocument)
                    {
                        if(po.Key == documentId)
                        {
                            po.Value.Add(position);
                            foundInSameDocumentID |= true;
                            break;
                        }

                    }

                    if(!foundInSameDocumentID)
                    {
                        Positions = new List<int>();

                        Positions.Add(position);

                        ob.WordPositionsInDocument.Add(documentId, Positions);
                    }


                    found = true;
                    
                    break;
                }
            }
            return found;

        }

        static bool IsStopWords(string word)
        {
            string[] stopWord = new string[] { "be","has","have","can","these","do","to","it" ,"we","is", "are", "am", "could", "will", "in" , "of","for" , "an" ,"the" , "a","what" , "how" , "when" , "where" , "on" ,"and","all" };
            bool found = false;
            foreach(string w in stopWord)
            {
                if (w == word)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

    }
}



      
