using OpenMcdf;
using System.IO;
using System.Text;

namespace VSConfiguratonHelper
{

        public  enum ProgramReturnCode
        {
            Success = 0,
            NoArg = -1,
            InvalidFileFormat = -2
        }

        public class ConfiguratonHelper
    {
            private const string SolutionConfigStreamName = "SolutionConfiguration";
            private const string ActiveConfigTokenName = "ActiveCfg";

        public string SolutionBasePath { get; set; } = string.Empty;





//        public string SolutionSuoPath { get; set; }= string.Empty;
        /*
        internal static int Main(string[] args)
        {
            try
            {
                ValidateCommandLineArgs(args);

                string activeSolutionConfig = ExtractActiveSolutionConfig(
                    new FileInfo(args.First()));

                throw new ProgramResultException(
                    activeSolutionConfig, ProgramReturnCode.Success);
            }
            catch (ProgramResultException e)
            {
                Console.Write(e.Message);
                return (int)e.ReturnCode;
            }
        }
        */

        private static void ValidateCommandLineArgs(string[] args)
            {
                if (args.Count() != 1) throw new ProgramResultException(
                    "There must be exactly one command-line argument, which " +
                    "is the path to an input Visual Studio Solution User " +
                    "Options (SUO) file.  The path should be enclosed in " +
                    "quotes if it contains spaces.", ProgramReturnCode.NoArg);
            }

            public string ActiveConfiguration
            {
                get
                {

               var suoPath = Path.Combine(SolutionBasePath, ".vs\\Web\\v17\\.suo");
                if (File.Exists(suoPath) == false) throw new Exception(string.Concat("Cannot find your SUO file at ", suoPath));
                    FileInfo fromSuoFile = new FileInfo(suoPath);
                    CompoundFile compoundFile;

                    try { compoundFile = new CompoundFile(fromSuoFile.FullName); }
                    catch (CFFileFormatException)
                    { throw CreateInvalidFileFormatProgramResultException(fromSuoFile); }

                    if (compoundFile.RootStorage.TryGetStream(
                        SolutionConfigStreamName, out CFStream compoundFileStream))
                    {
                        var data = compoundFileStream.GetData();
                        string dataAsString = Encoding.GetEncoding("UTF-16").GetString(data);
                        int activeConfigTokenIndex = dataAsString.LastIndexOf(ActiveConfigTokenName);

                        if (activeConfigTokenIndex < 0)
                            CreateInvalidFileFormatProgramResultException(fromSuoFile);

                        string afterActiveConfigToken =
                            dataAsString.Substring(activeConfigTokenIndex);

                        int lastNullCharIdx = afterActiveConfigToken.LastIndexOf('\0');
                        string ret = afterActiveConfigToken.Substring(lastNullCharIdx + 1);
                        return ret.Replace(";", "");
                    }
                    else throw CreateInvalidFileFormatProgramResultException(fromSuoFile);
                }
            }

            private static ProgramResultException CreateInvalidFileFormatProgramResultException(
                FileInfo invalidFile) => new ProgramResultException(
                    $@"The provided file ""{invalidFile.FullName}"" is not a valid " +
                    $@"SUO file with a ""{SolutionConfigStreamName}"" stream and an " +
                    $@"""{ActiveConfigTokenName}"" token.", ProgramReturnCode.InvalidFileFormat);
        }

        internal class ProgramResultException : Exception
        {
            internal ProgramResultException(string message, ProgramReturnCode returnCode)
                : base(message) => ReturnCode = returnCode;

            internal ProgramReturnCode ReturnCode { get; }
        }
    }

