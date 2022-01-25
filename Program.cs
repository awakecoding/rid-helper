using System;

namespace RidHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            string libraryName = "test";
            string importPath = RidHelper.GetDllImportPath(libraryName, false);
            Console.WriteLine("DllImport({0}) = {1}", libraryName, importPath);
        }
    }
}
