using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungestFile
{
    public static class FileValidator
    {
        public static bool ValidatePath(string path, bool directories = true, bool files = true)
        {
            // Check if it contains any Invalid Characters.
            if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            {
                try
                {
                    // Exceptions from FileInfo Constructor:
                    //   System.ArgumentNullException:
                    //     fileName is null.
                    //
                    //   System.Security.SecurityException:
                    //     The caller does not have the required permission.
                    //
                    //   System.ArgumentException:
                    //     The file name is empty, contains only white spaces, or contains invalid
                    //     characters.
                    //
                    //   System.IO.PathTooLongException:
                    //     The specified path, file name, or both exceed the system-defined maximum
                    //     length. For example, on Windows-based platforms, paths must be less than
                    //     248 characters, and file names must be less than 260 characters.
                    //
                    //   System.NotSupportedException:
                    //     fileName contains a colon (:) in the middle of the string.
                    FileInfo fileInfo = new FileInfo(path);

                    bool throwEx = false;


                    if(files && fileInfo.Exists)
                    {
                        // Exceptions using FileInfo.Length:
                        //   System.IO.IOException:
                        //     System.IO.FileSystemInfo.Refresh() cannot update the state of the file or
                        //     directory.
                        //
                        //   System.IO.FileNotFoundException:
                        //     The file does not exist.-or- The Length property is called for a directory.
                        throwEx = fileInfo.Length == -1;

                        // Exceptions using FileInfo.IsReadOnly:
                        //   System.UnauthorizedAccessException:
                        //     Access to fileName is denied.
                        //     The file described by the current System.IO.FileInfo object is read-only.
                        //     -or- This operation is not supported on the current platform.
                        //     -or- The caller does not have the required permission.
                        throwEx = fileInfo.IsReadOnly;

                        return true;
                    }
                    
                    if(directories)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(path);
                        if (dirInfo.Exists)
                        {
                            // Exceptions using FileInfo.IsReadOnly:
                            //   System.UnauthorizedAccessException:
                            //     Access to fileName is denied.
                            //     The file described by the current System.IO.FileInfo object is read-only.
                            //     -or- This operation is not supported on the current platform.
                            //     -or- The caller does not have the required permission.
                            FileAttributes fa = dirInfo.Attributes;
                            if((fa & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                throwEx = false;
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }

                    return false;

                }
                catch (ArgumentNullException)
                {
                    // System.ArgumentNullException:
                    //     fileName is null.
                }
                catch (System.Security.SecurityException)
                {
                    // System.Security.SecurityException:
                    //     The caller does not have the required permission.
                }
                catch (ArgumentException)
                {
                    // System.ArgumentException:
                    //     The file name is empty, contains only white spaces, or contains invalid
                    //     characters.
                }
                catch (UnauthorizedAccessException)
                {
                    // System.UnauthorizedAccessException:
                    //     Access to fileName is denied.
                }
                catch (PathTooLongException)
                {
                    // System.IO.PathTooLongException:
                    //     The specified path, file name, or both exceed the system-defined maximum
                    //     length. For example, on Windows-based platforms, paths must be less than
                    //     248 characters, and file names must be less than 260 characters.
                }
                catch (NotSupportedException)
                {
                    // System.NotSupportedException:
                    //     fileName contains a colon (:) in the middle of the string.
                }
                catch (FileNotFoundException)
                {
                    // System.FileNotFoundException
                    //     The exception that is thrown when an attempt to access a file that does not
                    //     exist on disk fails.
                }
                catch (IOException)
                {
                    // System.IO.IOException:
                    //     An I/O error occurred while opening the file.
                }
                catch (Exception)
                {
                    // Unknown Exception. Might be due to wrong case or null checks.
                }
            }
            else
            {
                // Path contains invalid characters
            }
            return false;
        }
    }
}
