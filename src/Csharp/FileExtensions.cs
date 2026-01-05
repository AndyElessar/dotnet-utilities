namespace Utilities;

public static partial class UtilitiesExtensions
{
    extension(Stream stream)
    {
        /// <summary>
        /// Converts the stream to a byte array.
        /// </summary>
        /// <returns>A byte array containing the stream data.</returns>
        public byte[] ToBytes()
        {
            if(stream is MemoryStream ms)
                return ms.ToArray();

            byte[] bytes;
            using(var bufferedStream = new BufferedStream(stream))
            using(var memoryStream = new MemoryStream())
            {
                bufferedStream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// Asynchronously converts the stream to a byte array.
        /// </summary>
        /// <param name="cancelToken">A cancellation token to observe.</param>
        /// <returns>A task representing the asynchronous operation, containing the byte array.</returns>
        public async ValueTask<byte[]> ToBytesAsync(CancellationToken cancelToken = default)
        {
            if(stream is MemoryStream ms)
                return ms.ToArray();

            byte[] bytes;
            using(var bufferedStream = new BufferedStream(stream))
            using(var memoryStream = new MemoryStream())
            {
                await bufferedStream.CopyToAsync(memoryStream, cancelToken);
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }
    }

    extension(Path)
    {
        /// <summary>
        /// Gets a safe temporary file path using a random file name.
        /// </summary>
        /// <returns>A unique temporary file path.</returns>
        public static string GetSafeTempFilePath() => Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    }

    extension(Directory)
    {
        /// <summary>
        /// Copies a directory and its contents to a new location.
        /// </summary>
        /// <param name="sourceDir">The source directory path.</param>
        /// <param name="destinationDir">The destination directory path.</param>
        /// <param name="recursive">Whether to copy subdirectories recursively.</param>
        /// <exception cref="DirectoryNotFoundException">Thrown when the source directory does not exist.</exception>
        public static void Copy(string sourceDir, string destinationDir, bool recursive)
        {
            var dir = new DirectoryInfo(sourceDir);

            if(!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            Directory.CreateDirectory(destinationDir);

            foreach(FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            if(recursive)
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach(DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    Directory.Copy(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
