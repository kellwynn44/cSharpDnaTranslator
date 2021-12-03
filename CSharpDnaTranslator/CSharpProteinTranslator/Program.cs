using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;

namespace CSharpProteinTranslator
{
    class Program
    {

        static void Main(string[] args)
        {
            //create a dictionary using IDictionary<TKey, TValue> Interface
            Dictionary<string, string> codonDictionary = new Dictionary<string, string>();

            //now we open a streamreader to add the key/value pairs to the dictionary
            const string FILENAME = @"C:\Users\Michelle\Documents\VSapps\codonTable.dat";
            FileStream inFile = new FileStream(FILENAME, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);

            //we must create variables to hold the individual lines and the array we will create from each line
            string line;
            string[] lines;

            //now we start reading the lines, splitting them into an array which is then used to fill our codonDictionary inside of a while loop
            line = reader.ReadLine();
            while (line != null)
            {
                //lines = line.Split(" "); <-- I tried to split it on the spaces but the program wouldn't do it (there was only one index in the array)
                lines = line.Split(',');    //so I went into my .dat file and added commas with no spaces between the elements
                string codon = lines[0];
                string initial = lines[1];
                codonDictionary.Add(codon, initial);

                //make sure we advance to the next line so we don't create an infinite loop
                line = reader.ReadLine();
            }
            //don't forget to close the filestream and reader! (in opposite order)
            reader.Close();
            inFile.Close();

            //now let's check that the dictionary was filled correctly
            foreach (KeyValuePair<string, string> codon in codonDictionary)
            {
                WriteLine(codon.Key + " : " + codon.Value);             //IT WORKED!
            }

            //finally, we need to use this dictionary to translate a dna sequence into its corresponding amino acids
            //to make this process a little easier we should read in the dna file as one long string
            //we open a new stream reader...
            const string DNAFILEPATH = @"C:\Users\Michelle\Documents\VSapps\Nep3.fa";
            FileStream dnaInFile = new FileStream(DNAFILEPATH, FileMode.Open, FileAccess.Read);
            StreamReader dnaReader = new StreamReader(dnaInFile);
            string dnaString = dnaReader.ReadToEnd();

            //close the filestream and reader because we are finished with the .fa file
            dnaReader.Close();
            dnaInFile.Close();

            //check what the value of dnaString is
            //WriteLine(dnaString);

            //it looks like there are newline characters in it, so let's filter them out
            string newDnaString = "";
            foreach (char c in dnaString)
            {
                if (c == 'A' || c == 'G' || c == 'C' || c == 'T')
                {
                    newDnaString += c;
                }
            }

            //lets check the value of newDnaString
            //WriteLine(newDnaString);

            //much better; there are no more newline characters

            //let's check the length of chars and make sure chars % 3 = 0 (it does!)
            int length = dnaString.Length;  //2421
            int triplets = length / 3;      //807
            //WriteLine(triplets.ToString());

            //now we can loop through the string and take each pair of three characters to search the dictionary for their corresponding amino acid initial
            //BUT we need to save our amino acids to a new file which means we need to open a filestream and streamwriter
            //let's create a new file called translatedSequence:
            FileStream newFile = new FileStream(@"C:\Users\Michelle\Documents\VSapps\translatedSequence.txt", FileMode.CreateNew, FileAccess.Write);
            StreamWriter writer = new StreamWriter(newFile);

            //we need sets of three
            for (int x = 0; x < (newDnaString.Length - 3); x += 3)
            {
                //lets grab sets of three using the Substring method
                string triplet = "";
                triplet += newDnaString.Substring(x, 3);
                //now we are ready to pass the string value to our dictionary as a key, retrieve the corresponding value, and record this value in our new file
                //we can use the TryGetValue c# function to do this; if the key is found it returns a boolean value of true plus the dictionary value as an out parameter
                string dictValue;
                bool hasValue = codonDictionary.TryGetValue(triplet, out dictValue);
                if (hasValue)
                {
                    writer.Write(dictValue);
                }
                else
                {
                    WriteLine(triplet);  //<-- this will write to the console to help me debug
                }
            }
            //we need to close our filestream and writer
            writer.Close();
            newFile.Close();
        }
    }
}
