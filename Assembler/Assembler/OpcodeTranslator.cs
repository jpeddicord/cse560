using System;
using System.IO;
using System.Collections.Generic;

public class OpcodeTranslator
{
	private Dictionary <string, Dictionary<string, string>> instructions;
	public OpcodeTranslator (string filename)
	{
		TextReader tr = new StreamReader(filename);
		
		string line;
		while ((line = tr.ReadLine()) != null) {
			string[] parts = line.Split(' ');
		}
	}
	
	
}

