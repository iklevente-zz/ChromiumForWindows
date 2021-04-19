using ChromiumForWindows_Updater;
using System;
using System.IO;

/*
 * Thank you mikey-t for this amazing C# class! https://gist.github.com/mikey-t/7999228
*/

public class Output

{

    private readonly string LogDirPath = MainWindow.chromiumPath + "\\log\\";



    private static Output _outputSingleton;

    private static Output OutputSingleton

    {

        get

        {

            if (_outputSingleton == null)

            {

                _outputSingleton = new Output();

            }

            return _outputSingleton;

        }

    }



    public StreamWriter SW { get; set; }



    public Output()

    {

        EnsureLogDirectoryExists();

        DeleteOldLog();

        InstantiateStreamWriter();

    }



    ~Output()

    {

        if (SW != null)

        {

            try

            {

                SW.Dispose();

            }

            catch (ObjectDisposedException) { } // object already disposed - ignore exception

        }

    }



    public static void WriteLine(string str)

    {

        Console.WriteLine(str);

        OutputSingleton.SW.WriteLine(str);

    }



    public static void Write(string str)

    {

        Console.Write(str);

        OutputSingleton.SW.Write(str);

    }



    private void InstantiateStreamWriter()

    {

        string filePath = Path.Combine(LogDirPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")) + "_updaterlog.txt";

        try

        {

            SW = new StreamWriter(filePath);

            SW.AutoFlush = true;

        }

        catch (UnauthorizedAccessException ex)

        {

            throw new ApplicationException(string.Format("Access denied. Could not instantiate StreamWriter using path: {0}.", filePath), ex);

        }

    }



    private void EnsureLogDirectoryExists()

    {

        if (!Directory.Exists(LogDirPath))

        {

            try

            {

                Directory.CreateDirectory(LogDirPath);

            }

            catch (UnauthorizedAccessException ex)

            {

                throw new ApplicationException(string.Format("Access denied. Could not create log directory using path: {0}.", LogDirPath), ex);

            }

        }

    }

    private void DeleteOldLog()
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(MainWindow.chromiumPath + "\\log\\");

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
    }

}